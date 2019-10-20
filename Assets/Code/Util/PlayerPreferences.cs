using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MinimalMiner.Util
{
    /// <summary>
    /// Handles the loading, saving, and utilization of player preferences
    /// </summary>
    public class PlayerPreferences : MonoBehaviour
    {
        public InputDefinitions Controls
        {
            get; private set;
        }

        private void Awake()
        {
            InputDefinitions input = new InputDefinitions
            {
                Menu_Pause = KeyCode.Escape,
                Ship_Forward = KeyCode.W,
                Ship_Reverse = KeyCode.S,
                Ship_CW = KeyCode.D,
                Ship_CCW = KeyCode.A,
                Ship_Fire = KeyCode.Space
            };
            Controls = input;
        }
    }
}