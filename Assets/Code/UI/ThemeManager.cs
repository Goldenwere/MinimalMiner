using UnityEngine;
using MinimalMiner.Util;

namespace MinimalMiner.Themes
{
    /// <summary>
    /// Handles updates to themes in-game
    /// </summary>
    public class ThemeManager : MonoBehaviour
    {
        public Theme[] Themes
        {
            get; private set;
        }

        public delegate void UpdateTheme(Theme theme);
        public static event UpdateTheme updateTheme;

        private void OnEnable()
        {
            EventManager.onSelectTheme += SelectTheme;
        }

        private void OnDisable()
        {
            EventManager.onSelectTheme -= SelectTheme;
        }

        public void SelectTheme(int themeIndex)
        {
            updateTheme(Themes[themeIndex]);
        }
    }
}