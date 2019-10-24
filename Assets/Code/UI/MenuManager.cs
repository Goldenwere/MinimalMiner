#pragma warning disable 0649

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MinimalMiner.Util;
using System.Collections.Generic;

namespace MinimalMiner.UI
{
    /// <summary>
    /// Handles in-game menu interfaces
    /// </summary>
    public class MenuManager : MonoBehaviour
    {
        #region Fields
        // Canvases
        [SerializeField] private GameObject canvasMain;
        [SerializeField] private GameObject canvasSettings;
        [SerializeField] private GameObject canvasPlay;
        [SerializeField] private GameObject canvasPause;
        [SerializeField] private GameObject canvasDeath;

        // Collections of UI elements that need theme updates
        private List<TextMeshProUGUI> primaryHeadings;
        private List<TextMeshProUGUI> secondaryHeadings;
        private List<TextMeshProUGUI> bodyText;
        private List<Button> buttons;
        private List<TMP_Dropdown> dropdowns;
        #endregion

        #region Methods
        /// <summary>
        /// Grab UI elements before theme is initially set
        /// </summary>
        private void Awake()
        {
            GrabUIElements();
        }

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start()
        {
            canvasSettings.SetActive(false);
            canvasPlay.SetActive(false);
            canvasPause.SetActive(false);
            canvasDeath.SetActive(false);
            canvasMain.SetActive(true);
            SetupUIElements();
        }

        /// <summary>
        /// Handles subscribing to events
        /// </summary>
        private void OnEnable()
        {
            PlayerPreferences.UpdateTheme += UpdateTheme;
            EventManager.OnUpdateGameState += UpdateGameState;
        }

        /// <summary>
        /// Handles unsubscribing to events
        /// </summary>
        private void OnDisable()
        {
            PlayerPreferences.UpdateTheme -= UpdateTheme;
            EventManager.OnUpdateGameState -= UpdateGameState;
        }

        /// <summary>
        /// Grabs various UI elements for theme updating
        /// </summary>
        private void GrabUIElements()
        {
            primaryHeadings = new List<TextMeshProUGUI>();
            secondaryHeadings = new List<TextMeshProUGUI>();
            bodyText = new List<TextMeshProUGUI>();
            buttons = new List<Button>();
            dropdowns = new List<TMP_Dropdown>();

            List<GameObject> canvases = new List<GameObject>()
            {
                canvasMain,
                canvasSettings,
                canvasPlay,
                canvasPause,
                canvasDeath
            };

            foreach(GameObject c in canvases)
            {
                primaryHeadings.AddRange(c.FindComponentsInChildrenWithTag<TextMeshProUGUI>("UI_text_primaryHead"));
                secondaryHeadings.AddRange(c.FindComponentsInChildrenWithTag<TextMeshProUGUI>("UI_text_secondaryHead"));
                bodyText.AddRange(c.FindComponentsInChildrenWithTag<TextMeshProUGUI>("UI_text_body"));

                buttons.AddRange(c.GetComponentsInChildren<Button>());
                dropdowns.AddRange(c.GetComponentsInChildren<TMP_Dropdown>());
            }
        }

        /// <summary>
        /// Sets up various UI elements that depend on options that may change or have loaded values that differ from defaults
        /// </summary>
        private void SetupUIElements()
        {
            PlayerPreferences playerPrefs = GameObject.FindWithTag("managers").GetComponent<PlayerPreferences>();

            foreach (TMP_Dropdown dropdown in dropdowns)
            {
                if (dropdown.name == "Dropdown_Theme")
                {
                    for (int i = 0; i < playerPrefs.Themes.Count; i++)
                    {
                        dropdown.options[i].text = playerPrefs.Themes[i].themeName;
                    }
                }
            }
        }

        /// <summary>
        /// Handles updates to the theme
        /// </summary>
        /// <param name="theme">The new theme</param>
        private void UpdateTheme(Theme theme)
        {
            // Update primary headings
            foreach(TextMeshProUGUI t in primaryHeadings)
                t.color = theme.text_primaryHead;

            // Update secondary headings
            foreach(TextMeshProUGUI t in secondaryHeadings)
                t.color = theme.text_secondaryHead;

            // Update body text
            foreach(TextMeshProUGUI t in bodyText)
                t.color = theme.text_body;

            // Update button colors
            foreach(Button b in buttons)
            {
                ColorBlock c = b.colors;

                c.normalColor = theme.button_normal;
                c.highlightedColor = theme.button_hover;
                c.pressedColor = theme.button_active;
                c.selectedColor = theme.button_focus;
                c.disabledColor = theme.button_disabled;

                b.colors = c;
            }

            // update dropdown colors
            foreach(TMP_Dropdown d in dropdowns)
            {
                ColorBlock c = d.colors;

                c.normalColor = theme.button_normal;
                c.highlightedColor = theme.button_hover;
                c.pressedColor = theme.button_active;
                c.selectedColor = theme.button_focus;
                c.disabledColor = theme.button_disabled;

                d.colors = c;
            }
        }

        /// <summary>
        /// Handles updates to the game-state
        /// </summary>
        /// <param name="newState">The desired game-state</param>
        /// <param name="prevState">The previous game-state</param>
        private void UpdateGameState(GameState newState, GameState prevState)
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
        #endregion
    }
}