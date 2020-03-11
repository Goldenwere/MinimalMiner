#pragma warning disable 0649

using UnityEngine;
using MinimalMiner.Util;

namespace MinimalMiner.Entity
{
    /// <summary>
    /// Maintains the player ship in the scene
    /// </summary>
    public class PlayerManager : MonoBehaviour
    {
        #region Fields
        [SerializeField] private GameObject playerObj;      // The player object in the scene
        [SerializeField] private Player player;             // The player class in the scene
        #endregion

        #region Methods
        /// <summary>
        /// Handles subscribing to events
        /// </summary>
        private void OnEnable()
        {
            EventManager.OnUpdateGameState += UpdateGameState;
        }

        /// <summary>
        /// Handles unsubscribing to events
        /// </summary>
        private void OnDisable()
        {
            EventManager.OnUpdateGameState -= UpdateGameState;
        }

        /// <summary>
        /// Called when the current GameState is updated
        /// </summary>
        /// <param name="newState">The new GameState after updating</param>
        /// <param name="prevState">The previous GameState before updating</param>
        private void UpdateGameState(GameState newState, GameState prevState)
        {
            // Enable player at the start of gameplay
            if (newState == GameState.play && prevState != GameState.pause)
            {
                playerObj.SetActive(true);
                player.enabled = true;
                player.ResetPlayer();
            }

            // Disable player at the end of gameplay
            else if (newState == GameState.main && prevState == GameState.pause
                || newState == GameState.main && prevState == GameState.death
                || newState == GameState.main && prevState == GameState.main)
            {
                playerObj.SetActive(false);
                player.enabled = false;
            }
        }

        public void SetupPlayer(ShipConfiguration config)
        {
            player.SetupPlayer(config);
        }
        #endregion
    }
}