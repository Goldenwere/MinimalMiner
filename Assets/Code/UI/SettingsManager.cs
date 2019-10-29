using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using MinimalMiner;
using MinimalMiner.Util;

namespace MinimalMiner.UI
{
    /// <summary>
    /// Handles functionalities associated with the settings menu
    /// </summary>
    public class SettingsManager : MonoBehaviour
    {
        private PlayerPreferences prefs;
        private SettingsState currState;
        private string currControl;

        [SerializeField] GameObject canvasSettingsPrimary;
        [SerializeField] GameObject canvasSettingsControls;
        [SerializeField] List<Button> controls;

        /// <summary>
        /// Detects input once per frame when in the active controls state
        /// </summary>
        private void Update()
        {
            if (currState == SettingsState.controls_active)
            {
                if (Input.anyKeyDown)
                {
                    foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
                    {
                        if (Input.GetKey(kcode))
                        {
                            prefs.UpdateControls(kcode, currControl);
                            currState = SettingsState.controls_inactive;
                            UpdateControlState(currControl);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates the current settings state
        /// </summary>
        /// <param name="state">The value of the state in the SettingsState enum</param>
        public void UpdateSettingsState(int state)
        {
            currState = (SettingsState)state;

            switch (currState)
            {
                case SettingsState.controls_inactive:
                    canvasSettingsPrimary.SetActive(false);
                    canvasSettingsControls.SetActive(true);
                    break;
                case SettingsState.controls_active:
                    break;
                case SettingsState.settings:
                default:
                    canvasSettingsPrimary.SetActive(true);
                    canvasSettingsControls.SetActive(false);
                    break;
            }
        }

        /// <summary>
        /// Updates the current control state
        /// </summary>
        /// <param name="control">The name of the control being modified</param>
        public void UpdateControlState(string control)
        {
            currControl = control;

            // If changed to the active control SettingsState (must update before the current control), blank the appropriate button and disable buttons
            if (currState == SettingsState.controls_active)
            {
                foreach (Button b in controls)
                    b.interactable = false;

                switch (currControl)
                {
                    case "Ship_Forward":
                        GameObject.Find("Button_Thrust").GetComponentInChildren<Text>().text = "";
                        break;
                    case "Ship_Reverse":
                        break;
                    case "Ship_CW":
                        GameObject.Find("Button_Turn-Right").GetComponentInChildren<Text>().text = "";
                        break;
                    case "Ship_CCW":
                        GameObject.Find("Button_Turn-Left").GetComponentInChildren<Text>().text = "";
                        break;
                    case "Ship_Fire":
                        GameObject.Find("Button_Fire").GetComponentInChildren<Text>().text = "";
                        break;
                    case "Menu_Pause":
                        GameObject.Find("Button_Pause").GetComponentInChildren<Text>().text = "";
                        break;
                }
            }

            // If changed to the inactive control SettingsState, make the buttons interactable again and update the appropriate button's text with the new control
            else
            {
                foreach (Button b in controls)
                    b.interactable = true;

                switch (currControl)
                {
                    case "Ship_Forward":
                        GameObject.Find("Button_Thrust").GetComponentInChildren<Text>().text = prefs.Controls.Ship_Forward.ToString();
                        break;
                    case "Ship_Reverse":
                        break;
                    case "Ship_CW":
                        GameObject.Find("Button_Turn-Right").GetComponentInChildren<Text>().text = prefs.Controls.Ship_CW.ToString();
                        break;
                    case "Ship_CCW":
                        GameObject.Find("Button_Turn-Left").GetComponentInChildren<Text>().text = prefs.Controls.Ship_CCW.ToString();
                        break;
                    case "Ship_Fire":
                        GameObject.Find("Button_Fire").GetComponentInChildren<Text>().text = prefs.Controls.Ship_Fire.ToString();
                        break;
                    case "Menu_Pause":
                        GameObject.Find("Button_Pause").GetComponentInChildren<Text>().text = prefs.Controls.Menu_Pause.ToString();
                        break;
                }
            }
        }
    }
}