#pragma warning disable 0649

using UnityEngine;
using MinimalMiner.Util;

namespace MinimalMiner.Entity
{
    public class Bullet : MonoBehaviour
    {
        public Vector3 Vel
        {
            get; private set;
        }

        private float aliveTimer;
        private GameState currState;
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

                // Increment timer and destroy object when object has existed for longer than 3 seconds
                aliveTimer += Time.deltaTime;
                if (aliveTimer > 3f)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void UpdateGameState(GameState newState, GameState prevState)
        {
            currState = newState;
        }

        private void UpdateTheme(Theme theme)
        {
            sprite.material.color = theme.sprite_player;
        }

        public void Setup(Vector3 vel)
        {
            Vel = vel;
        }
    }
}