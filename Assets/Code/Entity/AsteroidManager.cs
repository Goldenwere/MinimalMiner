using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MinimalMiner.Entity
{
    /// <summary>
    /// Maintains the asteroids in a scene when in the playing state
    /// </summary>
    public class AsteroidManager : MonoBehaviour
    {
        private List<GameObject> asteroids;

        /// <summary>
        /// Updates the asteroids list after a collision depletes the health of an asteroid
        /// </summary>
        /// <param name="asteroid">The asteroid whose health has been depleted</param>
        /// <param name="newAsteroids">A new list of asteroids if the parent asteroid was large</param>
        public void OnAsteroidDestruction(GameObject asteroid, List<GameObject> newAsteroids)
        {
            // Remove current asteroid
            asteroids.Remove(asteroid);
            Destroy(asteroid);

            // Add new asteroids if any
            foreach (GameObject g in newAsteroids)
            {
                asteroids.Add(g);
            }
        }
    }
}