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
        public event OnCollisionDetectedDelegate OnCollisionDetected = delegate { };

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollisionDetected(collision);
        }
    }
}