#pragma warning disable 0649

using UnityEngine;

namespace MinimalMiner.Util
{
    /// <summary>
    /// Holds materials for SVG sprites and raster sprites for other managers to utilize on theme changes
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
    }
}