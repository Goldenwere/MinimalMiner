﻿#pragma warning disable 0649
#pragma warning disable 0108

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
        #region Fields
        // Core variables
        private GameState currState;
        private AsteroidManager asteroidMgr;

        // Asteroid variables
        [SerializeField] private ColliderListener colliderListener;
        [SerializeField] private Rigidbody2D rigidbody;
        [SerializeField] private SpriteRenderer sprite;
        private float currHealth = 10f;

        // Asteroid characteristics
        private AsteroidType type;
        private AsteroidSize size;
        #endregion

        #region Methods
        /// <summary>
        /// Handles the start of the object before the first frame
        /// </summary>
        private void OnEnable()
        {
            EventManager.OnUpdateGameState += UpdateGameState;
            currState = GameObject.FindWithTag("managers").GetComponent<EventManager>().CurrState;
            PlayerPreferences.UpdateTheme += UpdateTheme;
            colliderListener.OnCollisionDetected += OnCollisionDetected;
        }

        /// <summary>
        /// Handles subscribing to events
        /// </summary>
        private void OnDisable()
        {
            EventManager.OnUpdateGameState -= UpdateGameState;
            PlayerPreferences.UpdateTheme -= UpdateTheme;
            colliderListener.OnCollisionDetected -= OnCollisionDetected;
        }

        /// <summary>
        /// Handles collision between this asteroid and another object in the scene
        /// </summary>
        /// <param name="collision">Holds the collision information</param>
        private void OnCollisionDetected(Collision2D collision)
        {
            if (collision.gameObject.tag == "asteroid")
            {
                collision.gameObject.GetComponentInParent<Asteroid>().TakeDamage(1f);
                TakeDamage(1f);
            }

            else if (collision.gameObject.tag == "player")
            {
                collision.gameObject.GetComponentInParent<Player>().TakeDamage(1f);
                TakeDamage(1f);
            }
        }

        /// <summary>
        /// Called when the current GameState is updated
        /// </summary>
        /// <param name="newState">The new GameState after updating</param>
        /// <param name="prevState">The previous GameState before updating</param>
        private void UpdateGameState(GameState newState, GameState prevState)
        {
            currState = newState;
        }

        /// <summary>
        /// Called when the current Theme is updated
        /// </summary>
        /// <param name="theme">The new GameTheme properties</param>
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

            if (!(size == AsteroidSize.small))
            {
                int amountToSpawn = Random.Range(2, 5);

                for (int i = 0; i < amountToSpawn; i++)
                {
                    newAsteroids.Add(asteroidMgr.SpawnAsteroid(type, (int)size, rigidbody.velocity, transform.position));
                }
            }

            return newAsteroids;
        }

        /// <summary>
        /// Sets up the asteroid upon instantiation
        /// </summary>
        /// <param name="type">The type that the asteroid should be</param>
        /// <param name="size">The size of the asteroid</param>
        /// <param name="velocity">The initial velocity of the asteroid</param>
        /// <param name="position">The initial position of the asteroid</param>
        /// <param name="manager">The asteroid manager that spawned this asteroid</param>
        public void Setup(AsteroidType type, AsteroidSize size, Sprite sprite, Vector2 velocity, Vector3 position, AsteroidManager manager)
        {
            this.type = type;
            this.size = size;

            switch (size)
            {
                case AsteroidSize.large:
                    transform.localScale = new Vector3(4f, 4f, 4f);
                    break;
                case AsteroidSize.medium:
                    transform.localScale = new Vector3(2f, 2f, 2f);
                    break;
                case AsteroidSize.small:
                default:
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    break;
            }

            GetComponent<Rigidbody2D>().AddForce(velocity * 10f);
            transform.position = position;
            asteroidMgr = manager;
            this.sprite.sprite = sprite;
        }

        /// <summary>
        /// Contributes damage to the asteroid
        /// </summary>
        /// <param name="damageDone">The damage to contribute</param>
        public void TakeDamage(float damageDone)
        {
            currHealth -= damageDone;

            if (currHealth <= 0)
            {
                asteroidMgr.OnAsteroidDestruction(gameObject, Split());
            }
        }
        #endregion
    }
}