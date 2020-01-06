#pragma warning disable 0649

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MinimalMiner.UI
{
    public class ShipDesignerUI : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown weaponSelectionDropdown;
        [SerializeField] private Slider[] individualWeaponSliders;
        [SerializeField] private TMP_Dropdown[] individualWeaponDropdowns;
        [SerializeField] private ShipComponent[] valueIndicatorTitles;
        [SerializeField] private TextMeshProUGUI[] valueIndicatorTexts;
        private Dictionary<ShipComponent, TextMeshProUGUI> valueIndicators;
        private float value;

        /// <summary>
        /// Awake is called before Start
        /// </summary>
        private void Awake()
        {
            // Build dictionary from serialized arrays
            valueIndicators = new Dictionary<ShipComponent, TextMeshProUGUI>();
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
        /// <param name="indicator">Must be a valid ShipComponent</param>
        public void OnValueChanged(string indicator)
        {
            if (Enum.TryParse(indicator, out ShipComponent comp))
                if (valueIndicators.ContainsKey(comp))
                    valueIndicators[comp].text = Math.Round(value, 2).ToString();
        }

        /// <summary>
        /// Called when the slider for weapon count changes - redfines the options for the weapon selection
        /// </summary>
        public void OnWeaponCountChanged()
        {
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

            for (int i = 0; i < value; i++)
                options.Add(new TMP_Dropdown.OptionData("Weapon " + i));

            weaponSelectionDropdown.options = options;
            weaponSelectionDropdown.SetValueWithoutNotify(0);
        }

        /// <summary>
        /// Called when the weapon dropdown changes - need to refresh weapon data
        /// </summary>
        /// <param name="val"></param>
        public void OnWeaponSelectionChanged(int val)
        {
            for (int i = 0; i < individualWeaponSliders.Length; i++)
                individualWeaponSliders[i].SetValueWithoutNotify(individualWeaponSliders[i].minValue);

            for (int i = 0; i < individualWeaponDropdowns.Length; i++)
                individualWeaponDropdowns[i].SetValueWithoutNotify(0);
        }
    }
}