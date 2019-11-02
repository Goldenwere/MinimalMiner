using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System;

namespace MinimalMiner.Util
{
    /// <summary>
    /// Handles the loading, saving, and utilization of player preferences
    /// </summary>
    public class PreferencesManager : MonoBehaviour
    {
        #region Fields & Properties
        public delegate void UpdateThemeHandler(Theme theme);
        /// <summary>
        /// Notifies subscribed objects that the current Theme has been changed
        /// </summary>
        public static event UpdateThemeHandler UpdateTheme;

        private PlayerPreferences prefs;

        /// <summary>
        /// The player's control preferences
        /// </summary>
        public InputDefinitions Controls
        {
            get
            {
                return prefs.Controls;
            }
        }
        
        /// <summary>
        /// The themes that the player has installed
        /// </summary>
        public List<Theme> Themes
        {
            get; private set;
        }

        /// <summary>
        /// The player's current theme preference
        /// </summary>
        public Theme CurrentTheme
        {
            get; private set;
        }
        #endregion

        /// <summary>
        /// Sets up the preferences when Awake is called
        /// </summary>
        private void Awake()
        {
            #region Preferences
            // Define prefs
            prefs = new PlayerPreferences();

            // Enter the preferences directory
            string prefsPath = Application.persistentDataPath;
            DirectoryInfo prefsDir = new DirectoryInfo(prefsPath);
            FileInfo[] prefsFiles = prefsDir.GetFiles("*.*", SearchOption.AllDirectories);
            bool prefsFound = false;

            // Find the preferences file
            foreach(FileInfo file in prefsFiles)
                if (file.Name.Contains("preferences.xml"))
                    prefsFound = true;

            // If found, read from it
            if (prefsFound)
            {
                prefs = ReadFromPreferences();
            }

            // Otherwise, create one
            else
            {
                InputDefinitions input = new InputDefinitions
                {
                    Menu_Pause = KeyCode.Escape,
                    Ship_Forward = KeyCode.W,
                    Ship_Reverse = KeyCode.S,
                    Ship_CW = KeyCode.D,
                    Ship_CCW = KeyCode.A,
                    Ship_Fire = KeyCode.Space
                };

                prefs.Controls = input;
                prefs.CurrentTheme = 0;
                WriteToPreferences();
            }
            #endregion

            #region Themes
            Themes = new List<Theme>();

            // Enter the themes directory
            string path = Application.streamingAssetsPath + "/Themes";
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] allFiles = null;

            if (directoryInfo.Exists)
            {
                allFiles = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);

                // Read all the themes in the directory
                foreach (FileInfo file in allFiles)
                {
                    if (file.Extension.Contains("theme"))
                    {
                        Themes.Add(ThemeReader.GetTheme(file));
                    }
                }

                // If there are none, create a default theme
                if (Themes.Count == 0)
                    Themes.Add(new Theme("Default"));
            }

            else
            {
                directoryInfo.Create();
                Themes.Add(new Theme("Default"));
            }
            #endregion
        }

        /// <summary>
        /// Called before the first frame
        /// </summary>
        private void Start()
        {
            int index = 0;
            if (prefs.CurrentTheme < Themes.Count)
                index = prefs.CurrentTheme;

            SelectTheme(index);
        }

        /// <summary>
        /// Handles subscribing to events
        /// </summary>
        private void OnEnable()
        {
            EventManager.OnSelectTheme += SelectTheme;
            EventManager.OnUpdateGameState += UpdateGameState;
        }

        /// <summary>
        /// Handles unsubscribing to events
        /// </summary>
        private void OnDisable()
        {
            EventManager.OnSelectTheme -= SelectTheme;
            EventManager.OnUpdateGameState -= UpdateGameState;
        }

        /// <summary>
        /// Handles the updating of the player's current theme and passing the update to all objects that are theme-able
        /// </summary>
        /// <param name="themeIndex">The index of the theme selected in the settings menu</param>
        private void SelectTheme(int themeIndex)
        {
            CurrentTheme = Themes[themeIndex];
            prefs.CurrentTheme = themeIndex;
            if (CurrentTheme.themeName != "Default")
                CurrentTheme = ThemeReader.AssignSprites(CurrentTheme);
            WriteToPreferences();
            UpdateTheme(CurrentTheme);
        }

        /// <summary>
        /// Handles updates to the game-state
        /// </summary>
        /// <param name="newState">The desired game-state</param>
        /// <param name="prevState">The previous game-state</param>
        private void UpdateGameState(GameState newState, GameState prevState)
        {
            if (newState == GameState.play)
            {
                UpdateTheme(CurrentTheme);
            }
        }

        /// <summary>
        /// Handles updates to the control preferences
        /// </summary>
        /// <param name="key">The new key to assign to the controls</param>
        /// <param name="control">The control being modified</param>
        public void UpdateControls(KeyCode key, string control)
        {
            InputDefinitions newControls = Controls;

            switch (control)
            {
                case "Ship_Forward":
                    newControls.Ship_Forward = key;
                    break;
                case "Ship_Reverse":
                    newControls.Ship_Reverse = key;
                    break;
                case "Ship_CW":
                    newControls.Ship_CW = key;
                    break;
                case "Ship_CCW":
                    newControls.Ship_CCW = key;
                    break;
                case "Ship_Fire":
                    newControls.Ship_Fire = key;
                    break;
                case "Menu_Pause":
                    newControls.Menu_Pause = key;
                    break;
            }

            prefs.Controls = newControls;
            WriteToPreferences();
        }

        /// <summary>
        /// Writes to the [persistentDataPath]/preferences.xml file using the current prefs
        /// </summary>
        public void WriteToPreferences()
        {
            string prefsPath = Application.persistentDataPath;
            FileStream stream = null;
            XmlSerializer serializer = null;

            try
            {
                // Create and write to the file
                stream = File.Create(prefsPath + "/preferences.xml");
                serializer = new XmlSerializer(typeof(PlayerPreferences));
                serializer.Serialize(stream, prefs);
            }

            catch (Exception e)
            {
                print(e.Message + "\n" + e.StackTrace);
            }

            finally
            {
                if (stream != null)
                    stream.Close();
            }
        }

        /// <summary>
        /// Reads from the [persistentDataPath]/preferences.xml file and sets the current prefs
        /// </summary>
        public PlayerPreferences ReadFromPreferences()
        {
            PlayerPreferences prefs = new PlayerPreferences();

            string prefsPath = Application.persistentDataPath;
            StreamReader reader = null;
            StringReader sr = null;
            XmlSerializer serializer = null;
            XmlTextReader xmlReader = null;

            try
            {
                // Read the file
                reader = new StreamReader(File.OpenRead(prefsPath + "/preferences.xml"));
                string data = reader.ReadToEnd();
                sr = new StringReader(data);

                // Deserialize the file
                serializer = new XmlSerializer(typeof(PlayerPreferences));
                xmlReader = new XmlTextReader(sr);
                prefs = (PlayerPreferences)serializer.Deserialize(xmlReader);
            }

            catch (Exception e)
            {
                print(e.Message + "\n" + e.StackTrace);
            }

            finally
            {
                if (reader != null)
                    reader.Close();
                if (sr != null)
                    sr.Close();

                if (xmlReader != null)
                    xmlReader.Close();
            }

            return prefs;
        }
    }
}
 