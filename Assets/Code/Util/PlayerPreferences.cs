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
    }
}