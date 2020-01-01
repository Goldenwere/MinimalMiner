using System.Collections.Generic;
using UnityEngine;
using MinimalMiner;

namespace MinimalMiner.Entity
{
    public struct PlayerInventory
    {
        public float MaxWeight;
        public float CurrentWeight;
        public Dictionary<ItemMaterial, SlotInformation> Inventory;
    }

    public struct SlotInformation
    {
        public Vector2 GridPosition;
        public int Amount;
    }
}