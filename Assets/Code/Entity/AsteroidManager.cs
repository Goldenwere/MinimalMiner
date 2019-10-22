#pragma warning disable 0649

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
        [SerializeField] private GameObject[] asteroidPrefabs;

        private void Start()
        {
            asteroids = new List<GameObject>();
            SpawnAsteroids();
        }

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

        /// <summary>
        /// Spawns asteroids in the scene
        /// </summary>
        private void SpawnAsteroids()
        {
            // Currently hard-coding max-asteroids until some sort of StarSystem info struct is created
            for (int i = 0; i < 10; i++)
            {
                asteroids.Add(SpawnAsteroid());
            }
        }

        /// <summary>
        /// Spawns an asteroid
        /// </summary>
        /// <returns>The asteroid that was spawned</returns>
        public GameObject SpawnAsteroid()
        {
            int prefab = Random.Range(0, 8);
            int size = Random.Range(0, 3);
            Vector2 velocity = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
            Vector3 position;
            do
            {
                position = new Vector3(Random.Range(-6f, 6f), Random.Range(-4f, 4f), 0);
            }
            while (position.x > -1f && position.x < 1f || position.y > -1f && position.y < 1f);

            GameObject asteroid = Instantiate(asteroidPrefabs[prefab]);

            Asteroid behaviour = asteroid.GetComponent<Asteroid>();

            behaviour.Setup(AsteroidType.general, (AsteroidSize)size, velocity, position, this);

            return asteroid;
        }

        /// <summary>
        /// Spawns an asteroid
        /// </summary>
        /// <param name="type">The type of asteroid to spawn</param>
        /// <param name="maxSize">The maximum size of the asteroid (exclusive)</param>
        /// <param name="originalVelocity">The original velocity to base new velocity off of</param>
        /// <param name="originalPosition">The original position to base new position off of</param>
        /// <returns>The asteroid that was spawned</returns>
        public GameObject SpawnAsteroid(AsteroidType type, int maxSize, Vector2 originalVelocity, Vector3 originalPosition)
        {
            int prefab = Random.Range(0, 8);
            int size = Random.Range(0, maxSize);
            Vector2 velocity = new Vector2(originalVelocity.x + Random.Range(-5f, 5f), originalVelocity.y + Random.Range(-5f, 5f));

            GameObject asteroid = Instantiate(asteroidPrefabs[prefab]);

            Asteroid behaviour = asteroid.GetComponentInChildren<Asteroid>();

            behaviour.Setup(type, (AsteroidSize)size, velocity, originalPosition, this);

            return asteroid;
        }
    }
}