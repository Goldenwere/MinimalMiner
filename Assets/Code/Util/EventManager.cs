using System;
using UnityEngine;

namespace MinimalMiner.Util
{
    /// <summary>
    /// Represents the different game states (menus, paused vs gameplay, etc.)
    /// </summary>
    public enum GameState
    {
        main,
        settings,
        play,
        pause,
        death
    }

    /// <summary>
    /// <para>Handles incoming events from UI for other managers to subscribe to</para>
    /// Master GameState is tracked here
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        public delegate void OnUpdateTheme(int themeIndex);
        public static event OnUpdateTheme onUpdateTheme;

        public delegate void OnUpdateGameState(GameState newState, GameState prevState);
        public static event OnUpdateGameState onUpdateGameState;

        private GameState currState;

        public void UpdateGameState(GameState newState)
        {
            onUpdateGameState(newState, currState);
            currState = newState;
        }

        public void UpdateTheme(int themeIndex)
        {
            onUpdateTheme(themeIndex);
        }
    }
}