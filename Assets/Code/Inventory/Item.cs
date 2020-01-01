using MinimalMiner;

namespace MinimalMiner.Entity
{
    /// <summary>
    /// The base class that all items inherit from
    /// </summary>
    public class Item
    {
        public float Value { get; set; }
        public float Weight { get; set; }
    }

    /// <summary>
    /// Raw material, typically dropped by asteroids
    /// </summary>
    public class RawMaterial : Item
    {
        public ItemMaterial Material { get; set; }
    }
}