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
        }

        public void UpdateGameState(GameState newState)
        {
            OnUpdateGameState(newState, currState);
            currState = newState;
        }

        public void UpdateTheme(int themeIndex)
        {
            OnSelectTheme(themeIndex);
        }
    }
}