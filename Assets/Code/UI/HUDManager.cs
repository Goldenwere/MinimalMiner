#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MinimalMiner.Util;

namespace MinimalMiner.UI
{
    public class HUDManager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI healthText;

        private void OnEnable()
        {
            EventManager.OnUpdateHUDElement += UpdateElement;
        }

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
            if (element == HUDElement.health)
            {
                healthText.text = content;
            }
        }
    }
}