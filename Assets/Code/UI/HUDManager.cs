#pragma warning disable 0649

using UnityEngine;
using TMPro;
using MinimalMiner.Util;
using UnityEngine.UI;

namespace MinimalMiner.UI
{
    /// <summary>
    /// Handles the in-game HUD
    /// </summary>
    public class HUDManager : MonoBehaviour
    {
        #region Fields
        // Updatable HUD elements
        [SerializeField] private TextMeshProUGUI armorText;
        [SerializeField] private TextMeshProUGUI shieldText;

        // Minimap-related elements
        [SerializeField] private Camera minimapCam;
        [SerializeField] private SpriteRenderer minimapIcon;
        [SerializeField] private RawImage minimapImage;

        // Other HUD elements
        [SerializeField] private GameObject targetSoftLock;

        private float timer;    // Used for preventing jitteriness of soft lock target element
        #endregion

        #region Methods
        /// <summary>
        /// Handles subscribing to events
        /// </summary>
        private void OnEnable()
        {
            EventManager.OnUpdateHUDElement += UpdateElement;
            EventManager.OnUpdateTarget += UpdateTarget;
            PreferencesManager.UpdateTheme += UpdateTheme;
            EventManager.OnUpdateGameState += UpdateGameState;
        }

        /// <summary>
        /// Handles unsubscribing to events
        /// </summary>
        private void OnDisable()
        {
            EventManager.OnUpdateHUDElement -= UpdateElement;
            EventManager.OnUpdateTarget -= UpdateTarget;
            PreferencesManager.UpdateTheme -= UpdateTheme;
            EventManager.OnUpdateGameState -= UpdateGameState;
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        private void Update()
        {
            timer += Time.deltaTime;
        }

        /// <summary>
        /// Called when the current GameState is updated
        /// </summary>
        /// <param name="newState">The new GameState after updating</param>
        /// <param name="prevState">The previous GameState before updating</param>
        private void UpdateGameState(GameState newState, GameState prevState)
        {
            if (newState != GameState.play && newState != GameState.pause)
                UpdateTarget(false, null, null, null);
        }

        /// <summary>
        /// Called when the current Theme is updated
        /// </summary>
        /// <param name="theme">The new GameTheme properties</param>
        private void UpdateTheme(Theme theme)
        {
            if (theme.spriteImage_player != null)
                minimapIcon.sprite = theme.spriteImage_player;
            minimapIcon.material.color = theme.spriteColor_player;
        }

        /// <summary>
        /// Updates a HUD element
        /// </summary>
        /// <param name="element">The element to update</param>
        /// <param name="content">The content to update the element with</param>
        private void UpdateElement(HUDElement element, string content)
        {
            if (element == HUDElement.armor)
                armorText.text = content;

            else if (element == HUDElement.shield)
                shieldText.text = content;
        }

        /// <summary>
        /// Updates the soft-lock target HUD element
        /// </summary>
        /// <param name="isEnabled">Whether the element is enabled or disabled</param>
        /// <param name="target">The target transform being locked on to</param>
        /// <param name="source">The source transform (i.e. the player in most circumstances)</param>
        /// <param name="rigidbody">The target's rigidbody</param>
        private void UpdateTarget(bool isEnabled, Transform target, Transform source, Rigidbody2D rigidbody)
        {
            if (isEnabled)
            {
                if (!targetSoftLock.activeInHierarchy)
                {
                    targetSoftLock.SetActive(true);
                    if (timer > 0.5f)
                        targetSoftLock.transform.position = source.position;
                    timer = 0;
                }

                Vector2 predict = new Vector2(targetSoftLock.transform.position.x, targetSoftLock.transform.position.y)
                     + (rigidbody.velocity.normalized * (rigidbody.velocity.magnitude * Time.deltaTime));
                targetSoftLock.transform.position = Vector3.Lerp(predict, target.position, SceneConstants.SmoothTime * Time.deltaTime);
                targetSoftLock.transform.localScale = Vector3.Lerp(targetSoftLock.transform.localScale, target.localScale, SceneConstants.SmoothTime * Time.deltaTime);
            }

            else
            {
                if (targetSoftLock.activeInHierarchy)
                {
                    targetSoftLock.SetActive(false);
                    targetSoftLock.transform.localScale = Vector3.one;
                }
            }
        }
        #endregion
    }
}