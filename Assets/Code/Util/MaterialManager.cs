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
    }
}