#pragma warning disable 0649

using System.Collections.Generic;
using UnityEngine;
using MinimalMiner.Util;

namespace MinimalMiner.Entity
{
    /// <summary>
    /// Defines an asteroid gameobject in the scene
    /// </summary>
    public class Asteroid : MonoBehaviour
    {
        // Needed for tracking the current state of the asteroid
        private GameState currState;
        private int currHealth;
        private Vector3 vel;
        private AsteroidType type;
        private AsteroidSize size;
        private AsteroidManager asteroidMgr;
        [SerializeField] private SpriteRenderer sprite;

        private void OnEnable()
        {
            EventManager.OnUpdateGameState += UpdateGameState;
            PlayerPreferences.UpdateTheme += UpdateTheme;
        }

        private void OnDisable()
        {
            EventManager.OnUpdateGameState -= UpdateGameState;
            PlayerPreferences.UpdateTheme -= UpdateTheme;
        }

        private void Update()
        {
            if (currState == GameState.play)
            {
                transform.position += vel;
            }
        }

        /// <summary>
        /// Handles collision between this asteroid and another object in the scene
        /// </summary>
        /// <param name="collision">Holds the collision information</param>
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "asteroid")
            {
                // TO-DO: Handle bouncing for same-layer asteroids
            }

            else if (collision.gameObject.tag == "player")
            {
                // TO-DO: Handle health decrementation
            }

            if (currHealth <= 0)
            {
                asteroidMgr.OnAsteroidDestruction(gameObject, Split());
            }
        }

        /// <summary>
        /// Used when first spawning an asteroid
        /// </summary>
        /// <param name="type">The type that the asteroid should be</param>
        /// <param name="size">The size of the asteroid</param>
        /// <param name="velocity">The initial velocity of the asteroid</param>
        /// <param name="position">The initial position of the asteroid</param>
        public void Setup(AsteroidType type, AsteroidSize size, Vector3 velocity, Vector3 position)
        {
            this.type = type;
            this.size = size;

            switch(size)
            {
                case AsteroidSize.large:
                    sprite.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    break;
                case AsteroidSize.medium:
                    sprite.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                    break;
                case AsteroidSize.small:
                default:
                    sprite.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    break;
            }

            this.vel = velocity;
            transform.position = position;
        }

        /// <summary>
        /// Called when the current GameState is changed
        /// </summary>
        /// <param name="newState">The new GameState</param>
        /// <param name="prevState">The previous GameState</param>
        private void UpdateGameState(GameState newState, GameState prevState)
        {
            currState = newState;
        }

        /// <summary>
        /// Called when the game theme is changed
        /// </summary>
        /// <param name="theme">The new theme properties</param>
        private void UpdateTheme(Theme theme)
        {
            sprite.material.color = theme.sprite_asteroid;
        }

        /// <summary>
        /// Handles splitting an asteroid if an asteroid is large
        /// </summary>
        /// <returns>A list of newly split asteroids (returns null if a small asteroid)</returns>
        private List<GameObject> Split()
        {
            List<GameObject> newAsteroids = new List<GameObject>();

            int amountToSpawn = Random.Range(2, 5);

            for (int i = 0; i < amountToSpawn; i++)
            {
                newAsteroids.Add(asteroidMgr.SpawnAsteroid(type, (int)size));
            }

            return newAsteroids;
        }
    }
}