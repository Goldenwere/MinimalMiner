using UnityEngine;

namespace MinimalMiner.Util
{
    /// <summary>
    /// Handles the loading, saving, and utilization of player preferences
    /// </summary>
    public class PlayerPreferences : MonoBehaviour
    {
        public InputDefinitions Controls
        {
            get; private set;
        }

        public Theme[] Themes
        {
            get; private set;
        }

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

        public delegate void UpdateThemeHandler(Theme theme);
        public static event UpdateThemeHandler UpdateTheme;

        private void OnEnable()
        {
            EventManager.OnSelectTheme += SelectTheme;
        }

        private void OnDisable()
        {
            EventManager.OnSelectTheme -= SelectTheme;
        }

        public void SelectTheme(int themeIndex)
        {
            UpdateTheme(Themes[themeIndex]);
        }
    }
}