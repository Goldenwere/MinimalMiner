#pragma warning disable 0649

using UnityEngine;

namespace MinimalMiner.Util
{
    /// <summary>
    /// EventManager passes event related information to managers that depend on them. This primarily consists of UI and Input calls.
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        public delegate void OnSelectTheme(int themeIndex);
        public static event OnSelectTheme onSelectTheme;

        public delegate void OnUpdateGameState(GameState newState, GameState prevState);
        public static event OnUpdateGameState onUpdateGameState;

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
            onUpdateGameState(newState, currState);
            currState = newState;
        }

        public void UpdateTheme(int themeIndex)
        {
            onSelectTheme(themeIndex);
        }
    }
}