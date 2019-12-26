using UnityEngine;

namespace MinimalMiner.Util
{
    /// <summary>
    /// Collection of constants that other classes can depend on
    /// </summary>
    public struct SceneConstants
    {
        /// <summary>
        /// Defines boundary size for confining open space (as opposed to path-based areas)
        /// Note: Will become deprecated when StarSystem information is introduced (likely in the same struct)
        /// </summary>
        public static Vector2 BoundarySize
        {
            get { return new Vector2(75f, 75f); }
        }

        /// <summary>
        /// Defines padding for spawning bodies (e.g. asteroids) inside boundaries (usage is BoundarySize - BodySpawnPadding)
        /// </summary>
        public static float BodySpawnPadding
        {
            get { return 10f; }
        }

        /// <summary>
        /// Defines padding for player when spawning bodies (e.g. asteroids)
        /// </summary>
        public static Vector2 PlayerSafeZone
        {
            get { return new Vector2(2f, 2f); }
        }

        /// <summary>
        /// The smooth time for various elements (such as target UI element transition and asteroid healthbar transition)
        /// </summary>
        public static float SmoothTime
        {
            get { return 10f; }
        }
    }
}