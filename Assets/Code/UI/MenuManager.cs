using UnityEngine;
using MinimalMiner.Util;

namespace MinimalMiner.UI
{
    public class MenuManager : MonoBehaviour
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
            print("MenuManager says hi" + themeIndex);
        }
    }
}