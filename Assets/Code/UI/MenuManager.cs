using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MinimalMiner.Util;
using MinimalMiner.Themes;

namespace MinimalMiner.UI
{
    /// <summary>
    /// Handles in-game menu interfaces
    /// </summary>
    public class MenuManager : MonoBehaviour
    {
        // Canvases
        [SerializeField] private GameObject canvasMain;
        [SerializeField] private GameObject canvasSettings;
        [SerializeField] private GameObject canvasPlay;
        [SerializeField] private GameObject canvasPause;
        [SerializeField] private GameObject canvasDeath;

        // Collections of UI elements that need theme updates
        private TextMeshProUGUI[] mainText;
        private TextMeshProUGUI[] settingsText;
        private TextMeshProUGUI[] playText;
        private TextMeshProUGUI[] pauseText;
        private TextMeshProUGUI[] deathText;
        private Button[] mainButtons;
        private Button[] settingsButtons;
        private Button[] playButtons;
        private Button[] pauseButtons;
        private Button[] deathButtons;
        private TMP_Dropdown[] settingsDropdowns;

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        void Start()
        {
            canvasSettings.SetActive(false);
            canvasPlay.SetActive(false);
            canvasPause.SetActive(false);
            canvasDeath.SetActive(false);
            canvasMain.SetActive(true);
            GrabUIElements();
        }

        private void OnEnable()
        {
            EventManager.onUpdateTheme += UpdateTheme;
        }

        private void OnDisable()
        {
            EventManager.onUpdateTheme -= UpdateTheme;
        }

        /// <summary>
        /// Grabs various UI elements for theme updating
        /// </summary>
        private void GrabUIElements()
        {
            mainText = canvasMain.GetComponentsInChildren<TextMeshProUGUI>();
            settingsText = canvasSettings.GetComponentsInChildren<TextMeshProUGUI>();
            playText = canvasPlay.GetComponentsInChildren<TextMeshProUGUI>();
            pauseText = canvasPause.GetComponentsInChildren<TextMeshProUGUI>();
            deathText = canvasDeath.GetComponentsInChildren<TextMeshProUGUI>();

            mainButtons = canvasMain.GetComponentsInChildren<Button>();
            settingsButtons = canvasSettings.GetComponentsInChildren<Button>();
            playButtons = canvasPlay.GetComponentsInChildren<Button>();
            pauseButtons = canvasPause.GetComponentsInChildren<Button>();
            deathButtons = canvasDeath.GetComponentsInChildren<Button>();

            settingsDropdowns = canvasSettings.GetComponentsInChildren<TMP_Dropdown>();
        }

        public void UpdateTheme(int themeIndex)
        {
            print("MenuManager says hi" + themeIndex);
        }
    }
}