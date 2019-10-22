using UnityEngine;

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
        public Theme[] Themes
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
        }

        /// <summary>
        /// Handles subscribing to events
        /// </summary>
        private void OnEnable()
        {
            EventManager.OnSelectTheme += SelectTheme;
        }

        /// <summary>
        /// Handles unsubscribing to events
        /// </summary>
        private void OnDisable()
        {
            EventManager.OnSelectTheme -= SelectTheme;
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
    }
}