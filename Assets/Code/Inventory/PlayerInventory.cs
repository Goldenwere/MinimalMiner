using System.Collections.Generic;
using UnityEngine;
using MinimalMiner;

namespace MinimalMiner.Inventory
{
    /// <summary>
    /// Defines the overall inventory information of a player
    /// </summary>
    public struct PlayerInventory
    {
        /// <summary>
        /// The maximum weight of the player inventory
        /// </summary>
        public float MaxWeight;

        /// <summary>
        /// The current weight of the player inventory
        /// <para>Note: CurrentWeight is calculated based on all the items in the inventory</para>
        /// </summary>
        public float CurrentWeight
        {
            get 
            {
                float w = 0;
                for (int x = 0; x < SizeX; x++)
                    for (int y = 0; y < SizeY; y++)
                        if (Inventory.ContainsKey(new Vector2(x, y)))
                            w += Inventory[new Vector2(x, y)].SlotWeight;

                return w;
            }
        }

        /// <summary>
        /// The x-dimension of the inventory
        /// </summary>
        public int SizeX;

        /// <summary>
        /// The y-dimension of the inventory
        /// </summary>
        public int SizeY;

        /// <summary>
        /// The actual inventory
        /// </summary>
        public Dictionary<Vector2, SlotInformation> Inventory;

        /// <summary>
        /// Creates a player inventory with defined size properties
        /// </summary>
        /// <param name="weight">The maximum weight of the inventory</param>
        /// <param name="x">The maximum x-size of the inventory</param>
        /// <param name="y">The maximum y-size of the inventory</param>
        public PlayerInventory(float weight, int x, int y)
        {
            MaxWeight = weight;
            SizeX = x;
            SizeY = y;
            Inventory = new Dictionary<Vector2, SlotInformation>();
        }

        /// <summary>
        /// Returns the next empty slot in an inventory
        /// </summary>
        /// <param name="inv"></param>
        /// <returns></returns>
        public static Vector2 NextEmptySlot(PlayerInventory inv)
        {
            // Iterate over the inventory and return a slot if the inventory does not contain a key for it
            for(int i = 0; i < inv.SizeX; i++)
                for (int j = 0; j < inv.SizeY; j++)
                    if (!inv.Inventory.ContainsKey(new Vector2(i, j)))
                        return new Vector2(i, j);

            // Return (-1, -1) if there are no empty slots
            return new Vector2(-1, -1);
        }

        public static Vector2 FindItemSlot(PlayerInventory inv, Item item)
        {
            // Iterate over the inventory and return a slot if the inventory does not contain a key for it
            for (int i = 0; i < inv.SizeX; i++)
            {
                for (int j = 0; j < inv.SizeY; j++)
                {
                    Vector2 working = new Vector2(i, j);
                    if (inv.Inventory.ContainsKey(working))
                        if (inv.Inventory[working].m_Item.Material == item.Material)
                            return new Vector2(i, j);
                }
            }

            return new Vector2(-1, -1);
        }
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

        /// <summary>
        /// Creates slot information with a defined item and amount
        /// </summary>
        /// <param name="item">The item for the slot</param>
        /// <param name="amt">The amount in the slot</param>
        public SlotInformation(Item item, int amt)
        {
            Amount = amt;
            m_Item = item;
        }
    }
}