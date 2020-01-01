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
        public float Value { get; set; }

        /// <summary>
        /// The weight of the item
        /// </summary>
        public float Weight { get; set; }
    }

    /// <summary>
    /// Raw material, typically dropped by asteroids
    /// </summary>
    public class RawMaterial : Item
    {
        /// <summary>
        /// The material associated with this item
        /// </summary>
        public ItemMaterial Material { get; set; }

        /// <summary>
        /// Create a RawMaterial with an associated ItemMaterial
        /// </summary>
        public RawMaterial(ItemMaterial mat)
        {
            Material = mat;
        }
    }
}