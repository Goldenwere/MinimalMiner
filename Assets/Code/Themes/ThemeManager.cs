using UnityEngine;
using MinimalMiner.Util;

namespace MinimalMiner.Themes
{
    /// <summary>
    /// Handles updates to themes in-game
    /// </summary>
    public class ThemeManager : MonoBehaviour
    {
        private void OnEnable()
        {
            EventManager.onUpdateTheme += UpdateTheme;
        }

        private void OnDisable()
        {
            EventManager.onUpdateTheme -= UpdateTheme;
        }

        public void UpdateTheme(int themeIndex)
        {
            print("ThemeManager says hi" + themeIndex);
        }
    }
}