#pragma warning disable CS0649, CS0108

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MinimalMiner.Entity;
using MinimalMiner.Util;

namespace MinimalMiner.Design
{
    /// <summary>
    /// Represents a low-level version of the Player class for manipulation in the ship designer
    /// </summary>
    public class LowLevelPlayer : MonoBehaviour
    {
        public ShipConfiguration Config { get; set; }
        public Sprite ShipSprite
        {
            get { return renderer.sprite; }
            set { renderer.sprite = value; }
        }

        [SerializeField] private SpriteRenderer renderer;
        [SerializeField] private EventManager eventMgr;
        private float fireTimer;

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start()
        {
            Config = new ShipConfiguration(ShipSprite, eventMgr);
        }
    }
}