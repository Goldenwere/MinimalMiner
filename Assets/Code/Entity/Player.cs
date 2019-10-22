#pragma warning disable 0649

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
        [SerializeField] private SpriteRenderer sprite;

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

        // Variables for ship firing
        [SerializeField] GameObject bulletPrefab;
        [SerializeField] AudioSource bulletSound;
        float fireTimer;
        float fireRate;

        private void Start()
        {
            shipRotSpd = 5f;
            shipAccRate = 0.05f;
            shipDragRate = 0.975f;
            shipMaxSpd = 0.1f;
            fireRate = 0.2f;

            shipDir = new Vector3(1, 0);
            shipVel = new Vector3(0, 0);
            shipAcc = new Vector3(0, 0);
            shipPos = new Vector3(0, 0);
            transform.position = new Vector3(0, 0);

            playerPrefs = GameObject.FindWithTag("managers").GetComponent<PlayerPreferences>();
        }

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
                shipDir = Quaternion.Euler(0, 0, shipRotSpd) * shipDir;
                shipVel = shipVel.magnitude * shipDir;
            }

            if (Input.GetKey(playerPrefs.Controls.Ship_CW))
            {
                shipDir = Quaternion.Euler(0, 0, -shipRotSpd) * shipDir;
                shipVel = shipVel.magnitude * shipDir;
            }

            // Handle ship acceleration
            shipAcc = shipAccRate * shipDir * Time.deltaTime;

            if (Input.GetKey(playerPrefs.Controls.Ship_Forward))
            {
                shipVel += shipAcc;
            }

            else
            {
                shipVel = shipVel * shipDragRate;
            }

            shipVel = Vector3.ClampMagnitude(shipVel, shipMaxSpd);
            shipPos += shipVel;

            // Set transform
            transform.position = shipPos;

            float zAngle = Mathf.Atan2(shipDir.y, shipDir.x);
            zAngle *= Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, zAngle);
        }

        private void PlayerFiring()
        {
            if (Input.GetKey(KeyCode.Space) && fireTimer > fireRate)
            {
                // Instantiate bullet
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                // Set up its velocity and color based on current theme (aka the ship's color{)
                Bullet bulletBeh = bullet.GetComponentInChildren<Bullet>();
                bulletBeh.Setup(new Vector3(shipDir.x * 0.25f, shipDir.y * 0.25f));
                bullet.GetComponentInChildren<SpriteRenderer>().material.color = sprite.material.color;

                // Reset fire timer to limit firing, and play the firing sound
                fireTimer = 0;
                bulletSound.Play();
            }
        }
    }
}