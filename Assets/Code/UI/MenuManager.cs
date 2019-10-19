#pragma warning disable 0649

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
            ThemeManager.updateTheme += UpdateTheme;
            EventManager.onUpdateGameState += UpdateGameState;
        }

        private void OnDisable()
        {
            ThemeManager.updateTheme -= UpdateTheme;
            EventManager.onUpdateGameState -= UpdateGameState;
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

        /// <summary>
        /// Handles updates to the theme
        /// </summary>
        /// <param name="theme">The new theme</param>
        public void UpdateTheme(Theme theme)
        {

        }

        /// <summary>
        /// Handles updates to the game-state
        /// </summary>
        /// <param name="newState">The desired game-state</param>
        /// <param name="prevState">The previous game-state</param>
        public void UpdateGameState(GameState newState, GameState prevState)
        {
            // Deactivate previous UI
            switch (prevState)
            {
                case GameState.settings:
                    canvasSettings.SetActive(false);
                    break;
                case GameState.play:
                    canvasPlay.SetActive(false);
                    break;
                case GameState.pause:
                    canvasPause.SetActive(false);
                    break;
                case GameState.death:
                    canvasDeath.SetActive(false);
                    break;
                case GameState.main:
                    canvasMain.SetActive(false);
                    break;
                default:
                    canvasDeath.SetActive(false);
                    canvasPause.SetActive(false);
                    canvasPlay.SetActive(false);
                    canvasSettings.SetActive(false);
                    break;
            }

            // Activate current UI
            switch (newState)
            {
                case GameState.settings:
                    canvasSettings.SetActive(true);
                    break;
                case GameState.play:
                    canvasPlay.SetActive(true);
                    break;
                case GameState.pause:
                    canvasPause.SetActive(true);
                    break;
                case GameState.death:
                    canvasDeath.SetActive(true);
                    break;
                case GameState.main:
                default:
                    canvasMain.SetActive(true);
                    break;
            }
        }
    }
}