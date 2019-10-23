#pragma warning disable 0649
#pragma warning disable 0108

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MinimalMiner.Util
{
    /// <summary>
    /// Maintains the camera in the scene
    /// </summary>
    public class CameraManager : MonoBehaviour
    {
        #region Fields
        [SerializeField] private GameObject target;
        [SerializeField] private Camera camera;
        [SerializeField] private float dampTime;
        private Vector3 velocity = Vector3.zero;
        private GameState currState;
        #endregion

        #region Methods
        /// <summary>
        /// Handles subscribing to events
        /// </summary>
        private void OnEnable()
        {
            EventManager.OnUpdateGameState += UpdateGameState;
            PlayerPreferences.UpdateTheme += UpdateTheme;
        }

        /// <summary>
        /// Handles unsubscribing to events
        /// </summary>
        private void OnDisable()
        {
            EventManager.OnUpdateGameState -= UpdateGameState;
            PlayerPreferences.UpdateTheme -= UpdateTheme;
        }

        /// <summary>
        /// Updates camera tracking at a fixed interval
        /// </summary>
        private void FixedUpdate()
        {
            if (currState == GameState.play)
            {
                Vector3 point = camera.WorldToViewportPoint(target.transform.position);
                Vector3 delta = target.transform.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
                Vector3 destination = transform.position + delta;
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            }
        }

        /// <summary>
        /// Handles updates to the theme
        /// </summary>
        /// <param name="theme">The new theme</param>
        private void UpdateTheme(Theme theme)
        {
            camera.backgroundColor = theme.img_backgroundColor;
        }

        /// <summary>
        /// Handles updates to the game-state
        /// </summary>
        /// <param name="newState">The desired game-state</param>
        /// <param name="prevState">The previous game-state</param>
        private void UpdateGameState(GameState newState, GameState prevState)
        {
            currState = newState;
        }
        #endregion
    }
}