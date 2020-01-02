#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using MinimalMiner.Util;

namespace MinimalMiner.UI
{
    /// <summary>
    /// Handles functionalities associated with the settings menu
    /// </summary>
    public class SettingsManager : MonoBehaviour
    {
        // Core fields to manipulate
        private PreferencesManager prefs;                               // Used for updating controls
        private SettingsState currState;                                // The current SettingsState
        private string currControl;                                     // The current control to modify

        // Canvases that need enabled/disabled appropriately
        [SerializeField] private GameObject canvasSettingsPrimary;      // The main settings canvas gameobject
        [SerializeField] private GameObject canvasSettingsControls;     // The controls settings canvas gameobject
        [SerializeField] private GameObject canvasSettingsGraphics;     // The graphics settings canvas gameobject

        // UI elements that need updating
        [SerializeField] private List<Button> controls;                 // A list of the buttons related to controls in the controls canvas
        private Dictionary<string, TextMeshProUGUI> controlText;        // A collection of the texts associated with the controls' buttons
        [SerializeField] private TextMeshProUGUI framerateDisplay;

        // Assign these in the inspector in matching order
        [SerializeField] private string[] controlText_key;              // used for controlText
        [SerializeField] private TextMeshProUGUI[] controlText_value;   // used for controlText

        /// <summary>
        /// Sets up the controlText collection before Start is first called
        /// </summary>
        private void Awake()
        {
            controlText = new Dictionary<string, TextMeshProUGUI>();
            
            for (int i = 0; i < controlText_key.Length; i++)
            {
                controlText.Add(controlText_key[i], controlText_value[i]);
            }
        }

        /// <summary>
        /// Sets up the controls menu and current states before the first frame
        /// </summary>
        private void Start()
        {
            // Find the player preferences
            prefs = GameObject.FindWithTag("managers").GetComponent<PreferencesManager>();

            // Update the buttons' texts
            controlText["Button_Thrust"].text = prefs.Controls.Ship_Forward.ToString();
            controlText["Button_Turn-Right"].text = prefs.Controls.Ship_CW.ToString();
            controlText["Button_Turn-Left"].text = prefs.Controls.Ship_CCW.ToString();
            controlText["Button_Fire"].text = prefs.Controls.Ship_Fire.ToString();
            controlText["Button_Pause"].text = prefs.Controls.Menu_Pause.ToString();
            controlText["Button_Dampener"].text = prefs.Controls.Ship_Dampener.ToString();

            // Set up current states
            currControl = "";
            UpdateSettingsState(0);
        }

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
                case SettingsState.graphics:
                    canvasSettingsPrimary.SetActive(false);
                    canvasSettingsControls.SetActive(false);
                    canvasSettingsGraphics.SetActive(true);
                    break;
                case SettingsState.controls_inactive:
                    canvasSettingsPrimary.SetActive(false);
                    canvasSettingsControls.SetActive(true);
                    canvasSettingsGraphics.SetActive(false);
                    break;
                case SettingsState.controls_active:
                    // Handled with UpdateControlState
                    break;
                case SettingsState.settings:
                default:
                    canvasSettingsPrimary.SetActive(true);
                    canvasSettingsControls.SetActive(false);
                    canvasSettingsGraphics.SetActive(false);
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
                        controlText["Button_Thrust"].text = "";
                        break;
                    case "Ship_Reverse":
                        break;
                    case "Ship_CW":
                        controlText["Button_Turn-Right"].text = "";
                        break;
                    case "Ship_CCW":
                        controlText["Button_Turn-Left"].text = "";
                        break;
                    case "Ship_Fire":
                        controlText["Button_Fire"].text = "";
                        break;
                    case "Menu_Pause":
                        controlText["Button_Pause"].text = "";
                        break;
                    case "Ship_Dampener":
                        controlText["Button_Dampener"].text = "";
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
                        controlText["Button_Thrust"].text = prefs.Controls.Ship_Forward.ToString();
                        break;
                    case "Ship_Reverse":
                        break;
                    case "Ship_CW":
                        controlText["Button_Turn-Right"].text = prefs.Controls.Ship_CW.ToString();
                        break;
                    case "Ship_CCW":
                        controlText["Button_Turn-Left"].text = prefs.Controls.Ship_CCW.ToString();
                        break;
                    case "Ship_Fire":
                        controlText["Button_Fire"].text = prefs.Controls.Ship_Fire.ToString();
                        break;
                    case "Menu_Pause":
                        controlText["Button_Pause"].text = prefs.Controls.Menu_Pause.ToString();
                        break;
                    case "Ship_Dampener":
                        controlText["Button_Dampener"].text = prefs.Controls.Ship_Dampener.ToString();
                        break;
                }
            }
        }

        /// <summary>
        /// Updates the post processing preset to be used
        /// </summary>
        /// <param name="preset">The index of the preset to use</param>
        public void UpdatePostProcPreset(int preset)
        {
            prefs.UpdatePostProcPreset(preset);
        }

        /// <summary>
        /// Updates the vsync preference
        /// </summary>
        /// <param name="toggled">Whether the toggle was checked or not</param>
        public void UpdateVsync(bool toggled)
        {
            prefs.UpdateVsync(toggled);
        }

        /// <summary>
        /// Updates the target framerate
        /// </summary>
        /// <param name="rate">The framerate to set to</param>
        public void UpdateTargetFramerate(float rate)
        {
            prefs.UpdateTargetFramerate(rate);
            framerateDisplay.text = ((int)rate).ToString();
        }

        /// <summary>
        /// Updates antialiasing on the post processing layer
        /// </summary>
        /// <param name="option">The index of the option in the enum</param>
        public void UpdateAntiAlias(int option)
        {
            prefs.UpdateAntiAlias((PostProcessLayer.Antialiasing)option);
        }
    }
}