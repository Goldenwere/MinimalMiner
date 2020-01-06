#pragma warning disable 0649

using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MinimalMiner.UI
{
    public class ShipDesignerUI : MonoBehaviour
    {
        [SerializeField] private string[] valueIndicatorTitles;
        [SerializeField] private TextMeshProUGUI[] valueIndicatorTexts;
        private Dictionary<string, TextMeshProUGUI> valueIndicators;
        private float value;

        /// <summary>
        /// Awake is called before Start
        /// </summary>
        private void Awake()
        {
            // Build dictionary from serialized arrays
            valueIndicators = new Dictionary<string, TextMeshProUGUI>();
            for (int i = 0; i < valueIndicatorTitles.Length; i++)
                valueIndicators.Add(valueIndicatorTitles[i], valueIndicatorTexts[i]);
        }

        /// <summary>
        /// Sets the current working value (used by designer UI)
        /// </summary>
        /// <param name="val">The value to be set</param>
        public void SetValue(float val)
        {
            value = val;
        }

        /// <summary>
        /// Sets the current working value (used by designer UI)
        /// </summary>
        /// <param name="val">The value to be set</param>
        public void SetValue(int val)
        {
            value = val;
        }

        /// <summary>
        /// Defines what the working value is (used by designer UI)
        /// </summary>
        /// <param name="indicator">Must be a valid key in valueIndicators</param>
        public void OnValueChanged(string indicator)
        {
            if (valueIndicators.ContainsKey(indicator))
                valueIndicators[indicator].text = Math.Round(value, 2).ToString();
        }
    }
}