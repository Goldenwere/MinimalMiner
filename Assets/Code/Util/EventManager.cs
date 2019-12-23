#pragma warning disable 0649

using System;
using UnityEngine;

namespace MinimalMiner.Util
{
    /// <summary>
    /// EventManager passes event related information to managers that depend on them. This primarily consists of UI and Input calls.
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        #region Fields & Properties
        public delegate void OnSelectThemeDelegate(int themeIndex);
        /// <summary>
        /// Informs subscribed objects that a Theme was selected
        /// </summary>
        public static event OnSelectThemeDelegate OnSelectTheme = delegate { };

        public delegate void OnUpdateGameStateDelegate(GameState newState, GameState prevState);
        /// <summary>
        /// Informs subscribed objects that the current GameState was changed
        /// </summary>
        public static event OnUpdateGameStateDelegate OnUpdateGameState = delegate { };

        public delegate void OnUpdateHUDElementDelegate(HUDElement element, string content);
        /// <summary>
        /// Informs subscribed objects that a HUD element needs updated
        /// </summary>
        public static event OnUpdateHUDElementDelegate OnUpdateHUDElement = delegate { };

        /// <summary>
        /// The current GameState that the game is in
        /// </summary>
        public GameState CurrState
        {
            get; private set;
        }

        /// <summary>
        /// The current control preferences
        /// </summary>
        public InputDefinitions Controls
        {
            get; private set;
        }

        // Refers to the current player preferences
        [SerializeField] private PreferencesManager playerPrefs;
        #endregion

        #region Methods
        /// <summary>
        /// Handles the start of the object before the first frame
        /// </summary>
        private void Start()
        {
            Controls = playerPrefs.Controls;
            UpdateGameState(GameState.main);
        }

        /// <summary>
        /// Updates once per frame
        /// </summary>
        private void Update()
        {
            if (CurrState == GameState.play)
            {
                if (Input.GetKeyDown(Controls.Menu_Pause))
                {
                    UpdateGameState(GameState.pause);
                }
            }

            else if (CurrState == GameState.pause)
            {
                if (Input.GetKeyDown(Controls.Menu_Pause))
                {
                    UpdateGameState(GameState.play);
                }
            }
        }

        /// <summary>
        /// Updates the current GameState and passes this update to relevant objects
        /// </summary>
        /// <param name="newState">The new state to update to</param>
        public void UpdateGameState(GameState newState)
        {
            GameState prevState = CurrState;
            CurrState = newState;
            OnUpdateGameState(newState, prevState);
            UpdateTimeState();
        }

        /// <summary>
        /// Updates the current GameState and passes this update to relevant objects
        /// </summary>
        /// <param name="desiredState">The desired state to update to</param>
        public void UpdateGameState(string desiredState)
        {
            Enum.TryParse<GameState>(desiredState, out GameState newState);
            GameState prevState = CurrState;
            CurrState = newState;
            OnUpdateGameState(newState, prevState);
            UpdateTimeState();
        }

        /// <summary>
        /// Updates the time scale of the game based on incoming GameState
        /// </summary>
        private void UpdateTimeState()
        {
            if (CurrState == GameState.pause)
            {
                Time.timeScale = 0;
            }

            else
            {
                Time.timeScale = 1;
            }
        }

        /// <summary>
        /// Handles a theme selection in the settings menu
        /// </summary>
        /// <param name="themeIndex">The index of the theme selected in the settings menu</param>
        public void UpdateTheme(int themeIndex)
        {
            OnSelectTheme(themeIndex);
        }

        /// <summary>
        /// Quits the game
        /// </summary>
        public void CallQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Can be used to notify the HUD to update an element
        /// </summary>
        /// <param name="element"></param>
        public void UpdateHUDElement(HUDElement element, string content)
        {
            OnUpdateHUDElement(element, content);
        }
        #endregion
    }
}