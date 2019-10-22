#pragma warning disable 0649
#pragma warning disable 0108

using UnityEngine;
using MinimalMiner.Util;

namespace MinimalMiner.Entity
{
    /// <summary>
    /// Defines the player's instantiated ship
    /// </summary>
    public class Player : MonoBehaviour
    {
        #region Fields
        // Core variables
        private GameState currState;
        private PlayerPreferences playerPrefs;
        private EventManager eventMgr;

        // Ship variables
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Rigidbody2D rigidbody;
        private float playerHealth;

        // Variables for ship physics
        private Vector2 shipAcc;
        private float shipRotSpd;
        private float shipMaxSpd;
        private float shipAccRate;
        private float shipDragRate;

        // Variables for ship firing
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private AudioSource bulletSound;
        [SerializeField] private Transform firesource;
        private float fireTimer;
        private float fireRate;
        private float projectileSpeed;
        #endregion

        #region Methods
        /// <summary>
        /// Handles the start of the object before the first frame
        /// </summary>
        private void Start()
        {
            fireRate = 0.2f;
            projectileSpeed = 100f;
            shipRotSpd = 1f;
            shipMaxSpd = 10f;
            shipAccRate = 1f;

            GameObject managers = GameObject.FindWithTag("managers");
            playerPrefs = managers.GetComponent<PlayerPreferences>();
            eventMgr = managers.GetComponent<EventManager>();

            ResetPlayer();
        }

        /// <summary>
        /// Handles subscribing to events
        /// </summary>
        private void OnEnable()
        {
            EventManager.OnUpdateGameState += UpdateGameState;
            currState = GameObject.FindWithTag("managers").GetComponent<EventManager>().CurrState;
            PlayerPreferences.UpdateTheme += UpdateTheme;
        }

        /// <summary>
        /// Handles unsubscribing to events
        /// </summary>
        private void OnDisable()
        {
            EventManager.OnUpdateGameState -= UpdateGameState;
            PlayerPreferences.UpdateTheme -= UpdateTheme;
        }

        /// <summary>
        /// Updates once per frame
        /// </summary>
        private void Update()
        {
            if (currState == GameState.play)
            {
                PlayerMovement();
                PlayerFiring();
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
            sprite.material.color = theme.spriteColor_player;
        }

        /// <summary>
        /// Handles player movement
        /// </summary>
        private void PlayerMovement()
        {
            // Handle ship turning
            if (Input.GetKey(playerPrefs.Controls.Ship_CCW))
            {
                transform.Rotate(0, 0, shipRotSpd);
            }

            if (Input.GetKey(playerPrefs.Controls.Ship_CW))
            {
                transform.Rotate(0, 0, -shipRotSpd);
            }

            // Handle ship acceleration
            shipAcc = shipAccRate * transform.right * (Time.fixedDeltaTime * 50f);

            if (Input.GetKey(playerPrefs.Controls.Ship_Forward))
            {
                if (rigidbody.velocity.magnitude < shipMaxSpd)
                {
                    rigidbody.AddForce(shipAcc);
                }
            }
        }

        /// <summary>
        /// Handles player firing
        /// </summary>
        private void PlayerFiring()
        {
            fireTimer += Time.deltaTime;
            if (Input.GetKey(playerPrefs.Controls.Ship_Fire) && fireTimer > fireRate)
            {
                // Instantiate bullet
                GameObject bullet = Instantiate(bulletPrefab, firesource.position, Quaternion.identity);

                // Set up its velocity and color based on current theme (aka the ship's color)
                Projectile bulletBeh = bullet.GetComponentInChildren<Projectile>();
                bulletBeh.Setup(new Vector2(transform.right.x * projectileSpeed, transform.right.y * projectileSpeed));
                bullet.GetComponentInChildren<SpriteRenderer>().material.color = sprite.material.color;

                // Reset fire timer to limit firing, and play the firing sound
                fireTimer = 0;
                bulletSound.Play();
                rigidbody.AddForce(-shipAcc * 5f);
            }
        }

        /// <summary>
        /// Handles resetting player to the center of the screen with max health and zero speed/acceleration
        /// </summary>
        public void ResetPlayer()
        {
            // Reset state and stats
            playerHealth = 10f;
            eventMgr.UpdateHUDElement(HUDElement.health, playerHealth.ToString());

            // Reset transform
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;

            // Reset physics
            shipAcc = Vector2.zero;
            rigidbody.velocity = Vector2.zero;
        }

        /// <summary>
        /// Contributes damage to the player
        /// </summary>
        /// <param name="damageDone">The damage to contribute</param>
        public void TakeDamage(float damageDone)
        {
            playerHealth -= damageDone;
            eventMgr.UpdateHUDElement(HUDElement.health, playerHealth.ToString());
        }
        #endregion
    }
}