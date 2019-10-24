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
    public class PlayerPreferences : MonoBehaviour
    {
        #region Fields & Properties
        public delegate void UpdateThemeHandler(Theme theme);
        /// <summary>
        /// Notifies subscribed objects that the current Theme has been changed
        /// </summary>
        public static event UpdateThemeHandler UpdateTheme;

        /// <summary>
        /// The player's control preferences
        /// </summary>
        public InputDefinitions Controls
        {
            get; private set;
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

        // Temporary hard-coded themes
        private Theme darkTheme;
        private Theme lightTheme;

        [SerializeField] string dump;
        #endregion

        /// <summary>
        /// Sets up the preferences when Awake is called
        /// </summary>
        private void Awake()
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
            Controls = input;

            Themes = new List<Theme>();

            string path = Application.streamingAssetsPath + "/Themes";
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] allFiles = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);

            foreach (FileInfo file in allFiles)
            {
                if (file.Extension.Contains("theme"))
                {
                    StreamReader reader = null;
                    StringReader sr = null;
                    XmlSerializer serializer = null;
                    XmlTextReader xmlReader = null;
                    Theme theme;
                    try
                    {
                        Type type = typeof(Theme);
                        reader = new StreamReader(file.OpenRead());
                        string data = reader.ReadToEnd();
                        sr = new StringReader(data);
                        serializer = new XmlSerializer(type);
                        xmlReader = new XmlTextReader(sr);
                        theme = (Theme)serializer.Deserialize(xmlReader);
                        theme.img_backgroundNormal = theme.GetSprite(theme.themeName, ThemeFileName.backgroundNormal.ToString());

                        Themes.Add(theme);
                    }
                    catch (Exception ex)
                    {
                        print(ex);
                    }
                    finally
                    {
                        if (xmlReader != null)
                            xmlReader.Close();
                        if (sr != null)
                            sr.Close();
                        if (reader != null)
                            reader.Close();
                    }
                }
            }

            CurrentTheme = Themes[0];
        }

        /// <summary>
        /// Called before the first frame
        /// </summary>
        private void Start()
        {
            UpdateTheme(Themes[0]);
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
    }
}
 