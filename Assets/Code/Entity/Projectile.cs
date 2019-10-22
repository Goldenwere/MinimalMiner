#pragma warning disable 0649
#pragma warning disable 0108

using UnityEngine;
using MinimalMiner.Util;

namespace MinimalMiner.Entity
{
    public class Projectile : MonoBehaviour
    {
        private float aliveTimer;
        private GameState currState;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private ColliderListener colliderListener;
        [SerializeField] private Rigidbody2D rigidbody;

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
            if (collision.gameObject.tag == "asteroid")
            {
                collision.gameObject.GetComponent<Asteroid>().TakeDamage(5f);
            }

            Destroy(gameObject);
        }

        private void UpdateGameState(GameState newState, GameState prevState)
        {
            currState = newState;
        }

        private void UpdateTheme(Theme theme)
        {
            sprite.material.color = theme.sprite_player;
        }

        public void Setup(Vector2 vel)
        {
            rigidbody.AddForce(vel);
        }
    }
}