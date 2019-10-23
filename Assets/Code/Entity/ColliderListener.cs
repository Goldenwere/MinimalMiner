#pragma warning disable 0649

using UnityEngine;

namespace MinimalMiner.Entity
{
    /// <summary>
    /// Handles collisions on an object's child collider
    /// </summary>
    public class ColliderListener : MonoBehaviour
    {
        public delegate void OnCollisionDetectedDelegate(Collision2D collision);
        /// <summary>
        /// Informs subscribed objects that a collision has been detected
        /// </summary>
        public event OnCollisionDetectedDelegate OnCollisionDetected = delegate { };

        /// <summary>
        /// Called when a 2D collision has been detected
        /// </summary>
        /// <param name="collision">The properties of the collision</param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollisionDetected(collision);
        }
    }
}