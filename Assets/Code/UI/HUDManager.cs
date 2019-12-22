#pragma warning disable 0649

using UnityEngine;
using TMPro;
using MinimalMiner.Util;

namespace MinimalMiner.UI
{
    /// <summary>
    /// Handles the in-game HUD
    /// </summary>
    public class HUDManager : MonoBehaviour
    {
        #region Fields
        // Updatable HUD elements
        [SerializeField] TextMeshProUGUI armorText;
        [SerializeField] TextMeshProUGUI shieldText;
        #endregion

        #region Methods
        /// <summary>
        /// Handles subscribing to events
        /// </summary>
        private void OnEnable()
        {
            EventManager.OnUpdateHUDElement += UpdateElement;
        }

        /// <summary>
        /// Handles unsubscribing to events
        /// </summary>
        private void OnDisable()
        {
            EventManager.OnUpdateHUDElement -= UpdateElement;
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
        #endregion
    }
}