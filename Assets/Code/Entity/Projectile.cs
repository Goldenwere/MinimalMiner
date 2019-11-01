#pragma warning disable 0649
#pragma warning disable 0108

using UnityEngine;
using MinimalMiner.Util;

namespace MinimalMiner.Entity
{
    /// <summary>
    /// Defines a projectile gameobject in the scene
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        #region Fields
        // Core variables
        private float aliveTimer;
        private GameState currState;

        // Projectile variables
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private ColliderListener colliderListener;
        [SerializeField] private Rigidbody2D rigidbody;
        #endregion

        #region Methods
        /// <summary>
        /// Handles subscribing to events
        /// </summary>
        private void OnEnable()
        {
            EventManager.OnUpdateGameState += UpdateGameState;
            currState = GameObject.FindWithTag("managers").GetComponent<EventManager>().CurrState;
            PreferencesManager.UpdateTheme += UpdateTheme;
            colliderListener.OnCollisionDetected += OnCollisionDetected;
        }

        /// <summary>
        /// Handles unsubscribing to events
        /// </summary>
        private void OnDisable()
        {
            EventManager.OnUpdateGameState -= UpdateGameState;
            PreferencesManager.UpdateTheme -= UpdateTheme;
            colliderListener.OnCollisionDetected -= OnCollisionDetected;
        }

        /// <summary>
        /// Updates once per frame
        /// </summary>
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
            sprite.material.color = theme.spriteColor_player;
        }

        /// <summary>
        /// Sets up the projectile on instantiation
        /// </summary>
        /// <param name="vel"></param>
        public void Setup(Vector2 vel)
        {
            rigidbody.AddForce(vel);
        }
        #endregion
    }
}