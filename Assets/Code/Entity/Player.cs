using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MinimalMiner.Util;

namespace MinimalMiner.Entity
{
    /// <summary>
    /// Defines the player's instantiated ship
    /// </summary>
    public class Player : MonoBehaviour
    {
        private GameState currState;

        // Ship physics for current movement
        private Vector3 shipPos;
        private Vector3 shipDir;
        private Vector3 shipVel;
        private Vector3 shipAcc;

        // Ship physics "limitations"
        private float shipRotSpd;
        private float shipAccRate;
        private float shipDragRate;
        private float shipMaxSpd;

        private void OnEnable()
        {
            EventManager.onUpdateGameState += UpdateGameState;
        }

        private void OnDisable()
        {
            EventManager.onUpdateGameState -= UpdateGameState;
        }

        private void UpdateGameState(GameState newState, GameState prevState)
        {
            currState = newState;
        }
    }
}