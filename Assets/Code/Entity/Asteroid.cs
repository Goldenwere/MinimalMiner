using System.Collections;
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
        [SerializeField] private SpriteRenderer sprite;

        private void OnEnable()
        {
            EventManager.onUpdateGameState += UpdateGameState;
            PlayerPreferences.updateTheme += UpdateTheme;
        }

        private void OnDisable()
        {
            EventManager.onUpdateGameState -= UpdateGameState;
            PlayerPreferences.updateTheme -= UpdateTheme;
        }

        private void Update()
        {
            if (currState == GameState.play)
            {
                transform.position += Vel;
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
    }
}