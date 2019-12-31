#pragma warning disable 0649

using UnityEngine;
using System.Collections.Generic;

namespace MinimalMiner.Util
{
    /// <summary>
    /// Holds materials for SVG sprites and raster sprites for other managers to utilize on theme changes; Also holds default sprites
    /// </summary>
    public class MaterialManager : MonoBehaviour
    {
        #region Materials
        [SerializeField] private Material mat_vector;

        /// <summary>
        /// Used by vector sprites (.SVG)
        /// </summary>
        public Material Mat_Vector
        {
            get { return mat_vector; }
        }



        [SerializeField] private Material mat_vect_grad;

        /// <summary>
        /// Used by vector sprites that have gradients (.SVG)
        /// </summary>
        public Material Mat_VectorGradient
        {
            get { return mat_vect_grad; }
        }



        [SerializeField] private Material mat_raster;

        /// <summary>
        /// Used by raster sprites (.PNG, .TGA, etc.)
        /// </summary>
        public Material Mat_Raster
        {
            get { return mat_raster; }
        }
        #endregion

        [SerializeField] private Sprite default_Player;

        /// <summary>
        /// The default player sprite
        /// </summary>
        public Sprite Default_Player
        {
            get { return default_Player; }
        }

        [SerializeField] private List<Sprite> default_Asteroids;

        /// <summary>
        /// The default asteroid sprites
        /// </summary>
        public List<Sprite> Default_Asteroids
        {
            get { return default_Asteroids; }
        }

        #region Items
        [SerializeField] private List<Sprite> default_Elements;

        /// <summary>
        /// The default element drop sprites
        /// </summary>
        public List<Sprite> Default_Elements
        {
            get { return default_Elements; }
        }

        [SerializeField] private List<Sprite> default_Silicates;

        /// <summary>
        /// The default silicate drop sprites
        /// </summary>
        public List<Sprite> Default_Silicates
        {
            get { return default_Silicates; }
        }

        [SerializeField] private List<Sprite> default_GeneralItems;

        /// <summary>
        /// The default general item drop sprites
        /// </summary>
        public List<Sprite> Default_GeneralItems
        {
            get { return default_GeneralItems; }
        }
        #endregion
    }
}