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
        /// <summary>
        /// The type that the asteroid is
        /// </summary>
        public AsteroidType Type
        {
            get; private set;
        }

        /// <summary>
        /// The velocity of the asteroid
        /// </summary>
        public Vector3 Vel
        {
            get; private set;
        }

        // Needed for tracking the current state of the asteroid
        private GameState currState;
        private int currHealth;
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
                transform.position += Vel;
            }
        }

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
        public void Setup(AsteroidType type)
        {
            Type = type;
        }

        private void UpdateGameState(GameState newState, GameState prevState)
        {
            currState = newState;
        }

        private void UpdateTheme(Theme theme)
        {
            sprite.material.color = theme.sprite_asteroid;
        }

        private List<GameObject> Split()
        {
            List<GameObject> newAsteroids = new List<GameObject>();

            return newAsteroids;
        }
    }
}