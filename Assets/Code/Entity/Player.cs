#pragma warning disable 0649
#pragma warning disable 0108

using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MinimalMiner.Util;
using System;

namespace MinimalMiner.Entity
{
    /// <summary>
    /// Defines the player's instantiated ship
    /// </summary>
    public class PlayerOld : MonoBehaviour
    {
        #region Fields
        // Core variables
        private GameState currState;
        private PreferencesManager playerPrefs;
        private EventManager eventMgr;
        private MaterialManager matMgr;

        // Ship variables
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Rigidbody2D rigidbody;
        [SerializeField] private AudioSource damageSound;
        [SerializeField] private AudioSource deathSound;
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
        /// Handles finding certain managers before events called
        /// </summary>
        private void Awake()
        {
            GameObject managers = GameObject.FindWithTag("managers");
            playerPrefs = managers.GetComponent<PreferencesManager>();
            eventMgr = managers.GetComponent<EventManager>();
            matMgr = managers.GetComponent<MaterialManager>();
        }

        /// <summary>
        /// Handles the start of the object before the first frame
        /// </summary>
        private void Start()
        {
            ResetPlayer();
        }

        /// <summary>
        /// Handles subscribing to events
        /// </summary>
        private void OnEnable()
        {
            EventManager.OnUpdateGameState += UpdateGameState;
            currState = GameObject.FindWithTag("managers").GetComponent<EventManager>().CurrState;
            PreferencesManager.UpdateTheme += UpdateTheme;
        }

        /// <summary>
        /// Handles unsubscribing to events
        /// </summary>
        private void OnDisable()
        {
            EventManager.OnUpdateGameState -= UpdateGameState;
            PreferencesManager.UpdateTheme -= UpdateTheme;
        }

        /// <summary>
        /// Updates once per physics frame
        /// </summary>
        private void FixedUpdate()
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
            if (theme.spriteImage_player != null)
            {
                sprite.sprite = theme.spriteImage_player;

                switch (theme.import_Asteroids)
                {
                    case (int)SpriteImportType.png:
                        sprite.material = matMgr.Mat_Raster;
                        break;
                    case (int)SpriteImportType.svggradient:
                        sprite.material = matMgr.Mat_VectorGradient;
                        break;
                    case (int)SpriteImportType.svg:
                    default:
                        sprite.material = matMgr.Mat_Vector;
                        break;
                }
            }

            else
            {
                sprite.sprite = matMgr.Default_Player;
                sprite.material = matMgr.Mat_Vector;
            }

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
            fireRate = 0.2f;
            projectileSpeed = 100f;
            shipRotSpd = 5f;
            shipMaxSpd = 10f;
            shipAccRate = 5f;

            GameObject managers = GameObject.FindWithTag("managers");
            playerPrefs = managers.GetComponent<PreferencesManager>();
            eventMgr = managers.GetComponent<EventManager>();
            matMgr = managers.GetComponent<MaterialManager>();

            // Reset state and stats
            playerHealth = 10f;
            eventMgr.UpdateHUDElement(HUDElement.armor, playerHealth.ToString());
            UpdateTheme(playerPrefs.CurrentTheme);

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
            // Wrapped under if so that sound doesn't play even after death
            if (playerHealth > 0)
            {
                playerHealth -= damageDone;
                eventMgr.UpdateHUDElement(HUDElement.armor, playerHealth.ToString());
                damageSound.Play();
            }

            // Not using else if because if decrementing above makes health 0, death should occur, separate if because of sound, second condition to prevent repeating event/sound
            if (playerHealth <= 0 && currState != GameState.death)
            {
                eventMgr.UpdateGameState(GameState.death);
                deathSound.Play();
            }
        }
        #endregion
    }

    /// <summary>
    /// Defines the player's instantiated ship
    /// </summary>
    public class Player : MonoBehaviour
    {
        // Ship-related variables
        private ShipConfiguration shipConfig;
        private Vector2 shipAccForce;
        private float fireTimer = 30f;

        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Rigidbody2D rigidbody;
        [SerializeField] private PolygonCollider2D collider;
        [SerializeField] private AudioSource damageSound;
        [SerializeField] private AudioSource deathSound;

        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private AudioSource bulletSound;
        [SerializeField] private List<GameObject> bulletLoc;

        // Management variables
        private GameState currState;
        private PreferencesManager playerPrefs;
        private EventManager eventMgr;
        private MaterialManager matMgr;

        /// <summary>
        /// Called before the first frame update
        /// </summary>
        private void Start()
        {
            ShipDefenses defenses = new ShipDefenses();
            ShipThrusters thrusters = new ShipThrusters();
            ShipWeaponry weapons = new ShipWeaponry();
            Vector2[] colliders = null;

            #region Setup

            defenses.ArmorStrength = 10f;
            defenses.ShieldRecharge = 1f;
            defenses.ShieldStrength = 5f;
            defenses.ShieldDelay = 2f;

            thrusters.DampenerStrength = 2.5f;                  // Equivalent to rigidbody linear drag set before this temp code (shipDragRate was unused)
            thrusters.ForwardThrusterForce = 5f;                // Equivalent to shipAccRate set in the old player class ResetPlayer()
            thrusters.MaxDirectionalSpeed = 10f;                // Equivalent to shipMaxSpd set in old player class
            thrusters.RecoilCompensation = 0.9f;
            thrusters.ReverseThrusterForce = 3f;
            thrusters.RotationalSpeed = 5f;                     // Equivalent to shipRotSpd set in old player class

            ShipWeapon basicBlaster = new ShipWeapon();
            basicBlaster.Damage = 5f;                           // Original hardcoded value in prototyped Projectile was 5
            basicBlaster.Name = "Basic Blaster";
            basicBlaster.OutputPrefab = bulletPrefab;           // The prefabs and sounds will eventually be handled/stored outside Player
            basicBlaster.OutputSound = bulletSound;
            basicBlaster.RateOfFire = 0.35f;                    // Originally fireRate in old Player
            basicBlaster.Recoil = 15f;                          // Originally -shipAcc * 5f when handling recoil in old Player
            basicBlaster.Speed = 200f;                          // Originally projectileSpeed in old Player
            basicBlaster.Type = WeaponType.projectile;
            ShipWeapon secondBlaster = new ShipWeapon();
            secondBlaster.Damage = 1f;
            secondBlaster.Name = "Secondary Blaster";
            secondBlaster.OutputPrefab = bulletPrefab;
            secondBlaster.OutputSound = bulletSound;
            secondBlaster.RateOfFire = 0.2f;
            secondBlaster.Recoil = 5f;
            secondBlaster.Speed = 100f;
            secondBlaster.Type = WeaponType.projectile;

            weapons.DamageModifier = 1;
            weapons.RateModifier = 1;
            weapons.Slots = new List<Vector3>()
            {
                new Vector3(0.35f,0,0),
                new Vector3(0.25f,0.21f,0),
                new Vector3(0.258f,-0.21f,0)
            };
            weapons.SlotStatus = new Dictionary<Vector3, WeaponSlotStatus>()
            {
                { weapons.Slots[0], WeaponSlotStatus.enabled },
                { weapons.Slots[1], WeaponSlotStatus.enabled },
                { weapons.Slots[2], WeaponSlotStatus.enabled }
            };
            weapons.Weapons = new Dictionary<Vector3, ShipWeapon>()
            {
                { weapons.Slots[0], basicBlaster },
                { weapons.Slots[1], secondBlaster },
                { weapons.Slots[2], secondBlaster }
            };

            colliders = new Vector2[]                           // This is based off of what was in the PolygonCollider2D in old Player
            {
                new Vector2(0.25f, 0),
                new Vector2(-0.25f, 0.25f),
                new Vector2(-0.125f, 0),
                new Vector2(-0.25f, -0.25f)
            };

            #endregion

            shipConfig = new ShipConfiguration(weapons, defenses, thrusters, 0.25f, colliders, sprite.sprite, eventMgr);

            // This may seem redundant, accessing what was set in the previous Setup region, but that will eventually disappear once actual ships are defined
            rigidbody.drag = shipConfig.Stats_Thrusters.DampenerStrength;
            rigidbody.mass = shipConfig.Mass;
            collider.points = colliders;
            sprite.sprite = shipConfig.BodySprite;  // This, like the bulletPrefab and bulletSound, will be handled/stored outside player, so the back-and-fourth setting seen here won't be present eventually

            foreach(Vector3 w in weapons.Slots)
            {
                GameObject obj = new GameObject();
                obj.transform.parent = gameObject.transform;
                obj.transform.position = w;
                bulletLoc.Add(obj);
            }
        }

        /// <summary>
        /// Called once every physics update
        /// </summary>
        private void FixedUpdate()
        {
            if (currState == GameState.play)
            {
                PlayerMovement();
                PlayerFiring();
                shipConfig.Update(Time.fixedDeltaTime);
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
        /// Called when the current Theme is updated (note: this is based off of the old Player and will need changed to account for different player sprites)
        /// </summary>
        /// <param name="theme">The new GameTheme properties</param>
        private void UpdateTheme(Theme theme)
        {
            if (theme.spriteImage_player != null)
            {
                sprite.sprite = theme.spriteImage_player;

                switch (theme.import_Asteroids)
                {
                    case (int)SpriteImportType.png:
                        sprite.material = matMgr.Mat_Raster;
                        break;
                    case (int)SpriteImportType.svggradient:
                        sprite.material = matMgr.Mat_VectorGradient;
                        break;
                    case (int)SpriteImportType.svg:
                    default:
                        sprite.material = matMgr.Mat_Vector;
                        break;
                }
            }

            else
            {
                sprite.sprite = matMgr.Default_Player;
                sprite.material = matMgr.Mat_Vector;
            }

            sprite.material.color = theme.spriteColor_player;
        }

        /// <summary>
        /// Handles player movement in the game scene
        /// </summary>
        private void PlayerMovement()
        {
            // Handle ship turning
            if (Input.GetKey(playerPrefs.Controls.Ship_CCW))
            {
                transform.Rotate(0, 0, shipConfig.Stats_Thrusters.RotationalSpeed);
            }

            if (Input.GetKey(playerPrefs.Controls.Ship_CW))
            {
                transform.Rotate(0, 0, -shipConfig.Stats_Thrusters.RotationalSpeed);
            }

            if (Input.GetKey(playerPrefs.Controls.Ship_Forward))
            {   
                // Handle ship acceleration
                shipAccForce = shipConfig.Stats_Thrusters.ForwardThrusterForce * transform.right * (Time.fixedDeltaTime * 50f);
                if (rigidbody.velocity.magnitude < shipConfig.Stats_Thrusters.MaxDirectionalSpeed)
                {
                    rigidbody.AddForce(shipAccForce);
                }
            }
        }

        /// <summary>
        /// Handles player firing
        /// </summary>
        private void PlayerFiring()
        {
            fireTimer += 0.02f;

            if (Input.GetKey(playerPrefs.Controls.Ship_Fire))
            {
                Vector3[] weapons = new Vector3[shipConfig.Stats_Weapons.Weapons.Count];
                shipConfig.Stats_Weapons.Weapons.Keys.CopyTo(weapons, 0);

                for (int i = 0; i < weapons.Length; i++)
                {
                    ShipWeapon w = shipConfig.Stats_Weapons.Weapons[weapons[i]];
                    if (System.Math.Round(fireTimer % w.RateOfFire, 3) <= 0.02f)
                    {
                        if (w.Type == WeaponType.projectile)
                        {
                            GameObject proj = Instantiate(w.OutputPrefab, bulletLoc[i].transform.position, Quaternion.identity);

                            // Set up its velocity and color based on current theme (aka the ship's color)
                            Projectile behaviour = proj.GetComponentInChildren<Projectile>();
                            behaviour.Setup(new Vector2(transform.right.x * w.Speed, transform.right.y * w.Speed));
                            proj.GetComponentInChildren<SpriteRenderer>().material.color = sprite.material.color;

                            // Play fire sound and add recoil force
                            bulletSound.Play();
                            rigidbody.AddForce(-transform.right * w.Recoil);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles damage taken from other entities
        /// </summary>
        /// <param name="damageDone">The damage that was taken</param>
        public void TakeDamage(float damageDone)
        {
            bool isDead = shipConfig.TakeDamage(damageDone);
            eventMgr.UpdateHUDElement(HUDElement.armor, Math.Round(shipConfig.Current_Defenses.ArmorStrength, 2).ToString());
            eventMgr.UpdateHUDElement(HUDElement.shield, Math.Round(shipConfig.Current_Defenses.ShieldStrength, 2).ToString());

            if (isDead)
            {
                deathSound.Play();
                eventMgr.UpdateGameState(GameState.death);
            }

            else
                damageSound.Play();
        }

        /// <summary>
        /// Resets player properties
        /// </summary>
        public void ResetPlayer()
        {
            GameObject managers = GameObject.FindWithTag("managers");
            playerPrefs = managers.GetComponent<PreferencesManager>();
            eventMgr = managers.GetComponent<EventManager>();
            matMgr = managers.GetComponent<MaterialManager>();

            // Reset state and stats
            Start();
            shipConfig.ResetShip();
            eventMgr.UpdateHUDElement(HUDElement.armor, shipConfig.Current_Defenses.ArmorStrength.ToString());
            eventMgr.UpdateHUDElement(HUDElement.shield, shipConfig.Current_Defenses.ShieldStrength.ToString());
            UpdateGameState(eventMgr.CurrState, eventMgr.CurrState);
            UpdateTheme(playerPrefs.CurrentTheme);

            // Reset transform
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;

            // Reset physics
            shipAccForce = Vector2.zero;
            rigidbody.velocity = Vector2.zero;
        }
    }
}