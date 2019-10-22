#pragma warning disable 0649

using UnityEngine;

namespace MinimalMiner.Util
{
    /// <summary>
    /// EventManager passes event related information to managers that depend on them. This primarily consists of UI and Input calls.
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        public delegate void OnSelectThemeDelegate(int themeIndex);
        public static event OnSelectThemeDelegate OnSelectTheme = delegate { };

        public delegate void OnUpdateGameStateDelegate(GameState newState, GameState prevState);
        public static event OnUpdateGameStateDelegate OnUpdateGameState = delegate { };

        private GameState currState;
        public InputDefinitions Controls
        {
            get; private set;
        }
        [SerializeField] private PlayerPreferences playerPrefs;

        private void Start()
        {
            Controls = playerPrefs.Controls;
            UpdateGameState(GameState.play);
        }

        private void Update()
        {
            if (currState == GameState.play)
            {
                if (Input.GetKeyDown(Controls.Menu_Pause))
                {
                    UpdateGameState(GameState.pause);
                }
            }

            else if (currState == GameState.pause)
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
        /// <param name="newState"></param>
        public void UpdateGameState(GameState newState)
        {
            OnUpdateGameState(newState, currState);
            currState = newState;
        }

        /// <summary>
        /// Handles a theme selection in the settings menu
        /// </summary>
        /// <param name="themeIndex">The index of the theme selected in the settings menu</param>
        public void UpdateTheme(int themeIndex)
        {
            OnSelectTheme(themeIndex);
        }
    }
}