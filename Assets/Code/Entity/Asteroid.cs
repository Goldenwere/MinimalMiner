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
        /// The position of the asteroid
        /// </summary>
        public Vector3 Pos
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

        private void Start()
        {
            EventManager.onUpdateGameState += UpdateGameState;
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
    }
}