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
        private GameState currState;
        private PlayerPreferences playerPrefs;
        private EventManager eventMgr;
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

        private void Start()
        {
            fireRate = 0.2f;
            projectileSpeed = 100f;
            shipRotSpd = 5f;
            shipMaxSpd = 5f;
            shipAccRate = 5f;
            shipAcc = new Vector2(0, 0);
            transform.position = new Vector3(0, 0);

            GameObject managers = GameObject.FindWithTag("managers");
            playerPrefs = managers.GetComponent<PlayerPreferences>();
            eventMgr = managers.GetComponent<EventManager>();

            playerHealth = 10f;
            eventMgr.UpdateHUDElement(HUDElement.health, playerHealth.ToString());
        }

        private void OnEnable()
        {
            EventManager.OnUpdateGameState += UpdateGameState;
            currState = GameObject.FindWithTag("managers").GetComponent<EventManager>().CurrState;
            PlayerPreferences.UpdateTheme += UpdateTheme;
        }

        private void OnDisable()
        {
            EventManager.OnUpdateGameState -= UpdateGameState;
            PlayerPreferences.UpdateTheme -= UpdateTheme;
        }

        private void FixedUpdate()
        {
            if (currState == GameState.play)
            {
                PlayerMovement();
                PlayerFiring();
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

            shipAcc = shipAccRate * transform.right * (Time.fixedDeltaTime * 50f);

            if (Input.GetKey(playerPrefs.Controls.Ship_Forward))
            {
                if (rigidbody.velocity.magnitude < shipMaxSpd)
                {
                    rigidbody.AddForce(shipAcc);
                }
            }
        }

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
        /// Contributes damage to the player
        /// </summary>
        /// <param name="damageDone">The damage to contribute</param>
        public void TakeDamage(float damageDone)
        {
            playerHealth -= damageDone;
            eventMgr.UpdateHUDElement(HUDElement.health, playerHealth.ToString());
        }
    }
}