#pragma warning disable 0649

using UnityEngine;
using MinimalMiner.Util;

namespace MinimalMiner.Entity
{
    public class Projectile : MonoBehaviour
    {
        private Vector3 vel;

        private float aliveTimer;
        private GameState currState;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private ColliderListener colliderListener;

        private void OnEnable()
        {
            EventManager.OnUpdateGameState += UpdateGameState;
            currState = GameObject.FindWithTag("managers").GetComponent<EventManager>().CurrState;
            PlayerPreferences.UpdateTheme += UpdateTheme;
            colliderListener.OnCollisionDetected += OnCollisionDetected;
        }

        private void OnDisable()
        {
            EventManager.OnUpdateGameState -= UpdateGameState;
            PlayerPreferences.UpdateTheme -= UpdateTheme;
            colliderListener.OnCollisionDetected -= OnCollisionDetected;
        }

        private void Update()
        {
            if (currState == GameState.play)
            {
                transform.position += vel;

                // Increment timer and destroy object when object has existed for longer than 3 seconds
                aliveTimer += Time.deltaTime;
                if (aliveTimer > 3f)
                {
                    Destroy(gameObject);
                }
            }
        }

        /// <summary>
        /// Handles collision between this projectile and another object in the scene
        /// </summary>
        private void OnCollisionDetected(Collision2D collision)
        {

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
            this.vel = vel;
        }
    }
}