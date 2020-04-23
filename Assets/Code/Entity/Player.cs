#pragma warning disable 0649
#pragma warning disable 0108

using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using MinimalMiner.Util;
using MinimalMiner.Inventory;

namespace MinimalMiner.Entity
{
    /// <summary>
    /// Defines the player's instantiated ship
    /// </summary>
    public class Player : MonoBehaviour
    {
        // Ship-related variables
        private ShipConfiguration shipConfig;
        private Vector2 shipAccForce;
        private float fireTimer;
        private float flashTimer;
        private bool isTargeting;

        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Rigidbody2D rigidbody;
        public Rigidbody2D Rigidbody { get { return rigidbody; } }
        public float DamageResistance { get { return shipConfig.Current_Defenses.DamageResistance; } }
        [SerializeField] private PolygonCollider2D collider;
        [SerializeField] private AudioSource damageSound;
        [SerializeField] private AudioSource deathSound;
        [SerializeField] private ColliderListener colliderListener;

        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private AudioSource bulletSound;
        [SerializeField] private AudioSource bulletSound2;
        [SerializeField] private List<GameObject> bulletLoc;

        private PlayerInventory inv;

        // Management variables
        private GameState currState;
        private PreferencesManager playerPrefs;
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

            defenses.ArmorStrength = 50f;
            defenses.ShieldRecharge = 1f;
            defenses.ShieldStrength = 15f;
            defenses.ShieldDelay = 5f;
            defenses.DamageResistance = 15f;

            thrusters.DampenerStrength = 0.5f;                  // Equivalent to rigidbody linear drag set before this temp code (shipDragRate was unused)
            thrusters.ForwardThrusterForce = 10f;               // Equivalent to shipAccRate set in the old player class ResetPlayer()
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
            secondBlaster.OutputSound = bulletSound2;
            secondBlaster.RateOfFire = 0.2f;
            secondBlaster.Recoil = 5f;
            secondBlaster.Speed = 100f;
            secondBlaster.Type = WeaponType.projectile;

            weapons.DamageModifier = 1;
            weapons.RateModifier = 1;
            weapons.WeaponCount = 3;
            weapons.Positions = new List<Vector3>()
            {
                new Vector3(0.35f,0,0),
                new Vector3(0.25f,0.21f,0),
                new Vector3(0.25f,-0.21f,0)
            };
            weapons.Rotations = new List<Vector3>
            {
                { new Vector3(0,0,0) },
                { new Vector3(0,0,10f) },
                { new Vector3(0,0,-10f) }
            };
            weapons.SlotStatus = new List<WeaponSlotStatus>
            {
                { WeaponSlotStatus.enabled },
                { WeaponSlotStatus.enabled },
                { WeaponSlotStatus.enabled }
            };
            weapons.Weapons = new List<ShipWeapon>
            {
                { basicBlaster },
                { secondBlaster },
                { secondBlaster }
            };

            colliders = new Vector2[]                           // This is based off of what was in the PolygonCollider2D in old Player
            {
                new Vector2(0.25f, 0),
                new Vector2(-0.25f, 0.25f),
                new Vector2(-0.125f, 0),
                new Vector2(-0.25f, -0.25f)
            };

            #endregion

            ShipConfiguration tempShipConfig = new ShipConfiguration(weapons, defenses, thrusters, 1, colliders, sprite.sprite);

            // This may seem redundant, accessing what was set in the previous Setup region, but that will eventually disappear once actual ships are defined
            rigidbody.drag = tempShipConfig.Stats_Thrusters.DampenerStrength;
            rigidbody.mass = tempShipConfig.Mass;
            collider.points = colliders;
            sprite.sprite = tempShipConfig.BodySprite;  // This, like the bulletPrefab and bulletSound, will be handled/stored outside player, so the back-and-fourth setting seen here won't be present eventually

            for (int i = 0; i < weapons.WeaponCount; i++)
            {
                ShipWeapon w = weapons.Weapons[i];
                GameObject obj = new GameObject(w.Name);
                obj.transform.parent = gameObject.transform;
                obj.transform.position = weapons.Positions[i];
                obj.transform.rotation = Quaternion.Euler(weapons.Rotations[i]);
                bulletLoc.Add(obj);
            }
        }

        /// <summary>
        /// Called once every frame
        /// </summary>
        private void Update()
        {
            // Toggle the ship dampener
            if (Input.GetKeyDown(playerPrefs.Controls.Ship_Dampener))
            {
                if (rigidbody.drag > 0)
                    rigidbody.drag = 0;
                else
                    rigidbody.drag = shipConfig.Stats_Thrusters.DampenerStrength;
            }

            // Resets the fire timer when spacebar is first pressed
            if (Input.GetKeyDown(playerPrefs.Controls.Ship_Fire))
                fireTimer = 30f;

            if (flashTimer > -1f)
            {
                flashTimer += Time.deltaTime;

                if (sprite.material.color == playerPrefs.CurrentTheme.spriteColor_player)
                    sprite.material.color = playerPrefs.CurrentTheme.spriteColor_playerDamage;
                else
                    sprite.material.color = playerPrefs.CurrentTheme.spriteColor_player;

                if (flashTimer >= SceneConstants.DamageFlashTime)
                {
                    flashTimer = -1f;
                    sprite.material.color = playerPrefs.CurrentTheme.spriteColor_player;
                }
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
                PlayerTargeting();
                shipConfig.Update(Time.fixedDeltaTime);
            }
        }

        /// <summary>
        /// Handles subscribing to events
        /// </summary>
        private void OnEnable()
        {
            EventManager.OnUpdateGameState += UpdateGameState;
            PreferencesManager.UpdateTheme += UpdateTheme;
            colliderListener.OnTriggerDetected += OnTriggerDetected;
        }

        /// <summary>
        /// Handles unsubscribing to events
        /// </summary>
        private void OnDisable()
        {
            EventManager.OnUpdateGameState -= UpdateGameState;
            PreferencesManager.UpdateTheme -= UpdateTheme;
            colliderListener.OnTriggerDetected -= OnTriggerDetected;
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
            /*if (theme.spriteImage_player != null)
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
            }*/

            sprite.material.color = theme.spriteColor_player;
        }

        /// <summary>
        /// Handles collision between this asteroid and a trigger in the scene
        /// </summary>
        /// <param name="collider">The collider that triggered this</param>
        private void OnTriggerDetected(Collider2D collider)
        {
            if (collider.gameObject.tag == "item")
            {
                Vector2 slot;
                Item item = collider.gameObject.GetComponent<ItemDrop>().m_Item;
                // First, see if the item already exists
                if ((slot = PlayerInventory.FindItemSlot(inv, item)) != new Vector2(-1, -1))
                {
                    SlotInformation atSlot = inv.Inventory[slot];
                    atSlot.Amount += 1;
                    inv.Inventory[slot] = atSlot;
                    Destroy(collider.gameObject);
                }
                else if ((slot = PlayerInventory.NextEmptySlot(inv)) != new Vector2(-1, -1))
                {
                    inv.Inventory.Add(slot, new SlotInformation(collider.gameObject.GetComponent<ItemDrop>().m_Item, 1));
                    Destroy(collider.gameObject);
                }
            }
        }

        /// <summary>
        /// Handles player movement in the game scene
        /// </summary>
        private void PlayerMovement()
        {
            // Handle ship turning
            if (Input.GetKey(playerPrefs.Controls.Ship_CCW))
                if (isTargeting)
                    transform.Rotate(0, 0, shipConfig.Stats_Thrusters.RotationalSpeed / playerPrefs.TargetLockForce);
                else
                    transform.Rotate(0, 0, shipConfig.Stats_Thrusters.RotationalSpeed);

            if (Input.GetKey(playerPrefs.Controls.Ship_CW))
                if (isTargeting)
                    transform.Rotate(0, 0, -shipConfig.Stats_Thrusters.RotationalSpeed / playerPrefs.TargetLockForce);
                else
                    transform.Rotate(0, 0, -shipConfig.Stats_Thrusters.RotationalSpeed);

            if (Input.GetKey(playerPrefs.Controls.Ship_Forward))
            {   
                // Handle ship acceleration
                shipAccForce = shipConfig.Stats_Thrusters.ForwardThrusterForce * transform.right;
                rigidbody.AddForce(shipAccForce);
                rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, shipConfig.Stats_Thrusters.MaxDirectionalSpeed);
            }

            if (Input.GetKey(playerPrefs.Controls.Ship_Reverse))
            {
                shipAccForce = shipConfig.Stats_Thrusters.ReverseThrusterForce * -transform.right;
                rigidbody.AddForce(shipAccForce);
                rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, shipConfig.Stats_Thrusters.MaxDirectionalSpeed);
            }

            // Handle boundaries
            if (transform.position.x > SceneConstants.BoundarySize.x)
                rigidbody.AddForce(-transform.position * (transform.position.x - SceneConstants.BoundarySize.x) * 0.1f);
            else if (transform.position.x < -SceneConstants.BoundarySize.x)
                rigidbody.AddForce(transform.position * (SceneConstants.BoundarySize.x + transform.position.x) * 0.1f);

            if (transform.position.y > SceneConstants.BoundarySize.y)
                rigidbody.AddForce(-transform.position * (transform.position.y - SceneConstants.BoundarySize.y) * 0.1f);
            else if (transform.position.y < -SceneConstants.BoundarySize.y)
                rigidbody.AddForce(transform.position * (SceneConstants.BoundarySize.y + transform.position.y) * 0.1f);
        }

        /// <summary>
        /// Handles player firing
        /// </summary>
        private void PlayerFiring()
        {
            fireTimer += 0.02f;

            if (Input.GetKey(playerPrefs.Controls.Ship_Fire))
            {
                for (int i = 0; i < shipConfig.Stats_Weapons.WeaponCount; i++)
                {
                    ShipWeapon w = shipConfig.Stats_Weapons.Weapons[i];
                    if (Math.Round(fireTimer % w.RateOfFire, 3) <= 0.02f)
                    {
                        if (w.Type == WeaponType.projectile)
                        {
                            GameObject proj = Instantiate(w.OutputPrefab, bulletLoc[i].transform.position, Quaternion.identity);

                            // Set up its velocity and color based on current theme (aka the ship's color)
                            Projectile behaviour = proj.GetComponentInChildren<Projectile>();
                            behaviour.Setup(Vector2.ClampMagnitude(new Vector2(bulletLoc[i].transform.right.x * w.Speed, bulletLoc[i].transform.right.y * w.Speed), w.Speed),
                                w.Damage * shipConfig.Stats_Weapons.DamageModifier);
                            proj.GetComponentInChildren<SpriteRenderer>().material.color = sprite.material.color;

                            // Play fire sound and add recoil force
                            w.OutputSound.Play();
                            rigidbody.AddForce(-transform.right * w.Recoil);
                            rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, shipConfig.Stats_Thrusters.MaxDirectionalSpeed);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles player targetting (soft-locking onto asteroids)
        /// </summary>
        private void PlayerTargeting()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + (transform.right * 0.5f), transform.right, 5f);
            if (hit.collider != null && hit.collider.tag == "asteroid")
            {
                if (!isTargeting)
                    isTargeting = true;

                // Rotate toward target
                Vector3 toTarget = hit.collider.transform.position - transform.position;
                float angle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;
                Quaternion newRot = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, newRot, shipConfig.Stats_Thrusters.RotationalSpeed * Time.fixedDeltaTime);

                Rigidbody2D rigidbody = hit.collider.gameObject.GetComponent<Rigidbody2D>();

                EventManager.Instance.UpdateTargetElement(true, hit.transform, transform, rigidbody);
            }

            else if (isTargeting)
            {
                isTargeting = false;
                EventManager.Instance.UpdateTargetElement(false, null, null, null);
            }
        }

        /// <summary>
        /// Handles damage taken from other entities
        /// </summary>
        /// <param name="damageDone">The damage that was taken</param>
        public void TakeDamage(float damageDone)
        {
            bool isDead = shipConfig.TakeDamage(damageDone);
            EventManager.Instance.UpdateHUDElement(HUDElement.armor, Math.Round(shipConfig.Current_Defenses.ArmorStrength, 2).ToString());
            EventManager.Instance.UpdateHUDElement(HUDElement.shield, Math.Round(shipConfig.Current_Defenses.ShieldStrength, 2).ToString());

            if (isDead)
            {
                deathSound.Play();
                EventManager.Instance.UpdateGameState(GameState.death);
            }

            else
            {
                if (playerPrefs.DoDamageFlash)
                    flashTimer = 0;
                damageSound.Play();
            }
        }

        /// <summary>
        /// Resets player properties
        /// </summary>
        public void ResetPlayer()
        {
            GameObject managers = GameObject.FindWithTag("managers");
            playerPrefs = managers.GetComponent<PreferencesManager>();
            matMgr = managers.GetComponent<MaterialManager>();
            inv = new PlayerInventory(10000f, 10, 10);

            // Reset state and stats
            Start();
            shipConfig.ResetShip();
            EventManager.Instance.UpdateHUDElement(HUDElement.armor, shipConfig.Current_Defenses.ArmorStrength.ToString());
            EventManager.Instance.UpdateHUDElement(HUDElement.shield, shipConfig.Current_Defenses.ShieldStrength.ToString());
            EventManager.Instance.UpdateTargetElement(false, null, null, null);
            UpdateGameState(EventManager.Instance.CurrState, EventManager.Instance.CurrState);
            UpdateTheme(playerPrefs.CurrentTheme);
            flashTimer = -1f;
            fireTimer = 30f;

            // Reset transform
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;

            // Reset physics
            shipAccForce = Vector2.zero;
            rigidbody.velocity = Vector2.zero;
        }

        public void SetupPlayer(ShipConfiguration config)
        {
            shipConfig = config;
            ResetPlayer();
        }
    }
}