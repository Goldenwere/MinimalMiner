using System;
using UnityEngine;

namespace MinimalMiner.Util
{
    public class EventManager : MonoBehaviour
    {
        public delegate void OnUpdateTheme(int themeIndex);
        public static event OnUpdateTheme onUpdateTheme;

        private void Start()
        {
            onUpdateTheme(0);
        }
    }
}