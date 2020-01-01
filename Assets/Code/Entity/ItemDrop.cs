#pragma warning disable 0649

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
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Animator animator;

        // TO-DO: Add UpdateTheme handler

        /// <summary>
        /// Used for setting up an item drop upon spawning
        /// </summary>
        /// <param name="position">The position of the drop</param>
        /// <param name="mat">The item material of the drop</param>
        /// <param name="sp">The sprite for the item drop</param>
        public void SpawnDrop(Vector3 position, ItemMaterial mat, Sprite sp)
        {
            transform.position = position;
            Material = mat;
            sprite.sprite = sp;
            animator.SetFloat("Delay", Random.Range(0f, 1f));
        }
    }
}