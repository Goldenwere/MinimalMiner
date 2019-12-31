using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MinimalMiner;

namespace MinimalMiner.Entity
{
    /// <summary>
    /// Represents an item drop in the game scene
    /// </summary>
    public class ItemDrop : MonoBehaviour
    {
        /// <summary>
        /// The material that an item drop is
        /// </summary>
        public ItemMaterial Material { get; private set; }

        /// <summary>
        /// Used for setting up an item drop upon spawning
        /// </summary>
        /// <param name="position">The position of the drop</param>
        /// <param name="mat">The item material of the drop</param>
        public void SpawnDrop(Vector3 position, ItemMaterial mat)
        {
            transform.position = position;
            Material = mat;
        }
    }
}