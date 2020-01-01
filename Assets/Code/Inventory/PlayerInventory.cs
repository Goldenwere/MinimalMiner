using System.Collections.Generic;
using UnityEngine;
using MinimalMiner;

namespace MinimalMiner.Inventory
{
    /// <summary>
    /// Defines the overall inventory of a player
    /// </summary>
    public struct PlayerInventory
    {
        public float MaxWeight;
        public float CurrentWeight;
        public Dictionary<Vector2, SlotInformation> Inventory;
    }

    /// <summary>
    /// Defines information for an inventory slot
    /// </summary>
    public struct SlotInformation
    {
        /// <summary>
        /// The amount of the item being held in the slot
        /// </summary>
        public int Amount;

        /// <summary>
        /// The item in the slot
        /// </summary>
        public Item m_Item;

        /// <summary>
        /// The weight being contributed by this slot
        /// </summary>
        public float SlotWeight
        {
            get { return m_Item.Weight * Amount; }
        }
    }
}