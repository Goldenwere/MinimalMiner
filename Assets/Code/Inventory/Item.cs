using MinimalMiner;

namespace MinimalMiner.Inventory
{
    /// <summary>
    /// The base class that all items inherit from
    /// </summary>
    public class Item
    {
        /// <summary>
        /// The value of the item
        /// </summary>
        public float Value
        {
            get { return Definitions.ItemValues[Material]; }
        }

        /// <summary>
        /// The weight of the item
        /// </summary>
        public float Weight
        {
            get { return Definitions.ItemWeights[Material]; }
        }

        /// <summary>
        /// The category of the item
        /// </summary>
        public ItemCategory Category
        {
            get { return Definitions.ItemCategories[Material]; }
        }

        /// <summary>
        /// The item as a definition
        /// </summary>
        public ItemMaterial Material { get; set; }

        /// <summary>
        /// Creates an instance of an item
        /// </summary>
        /// <param name="mat">The material of the item</param>
        public Item(ItemMaterial mat)
        {
            Material = mat;
        }
    }
}