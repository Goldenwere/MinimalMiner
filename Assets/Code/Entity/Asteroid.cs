#pragma warning disable 0649
#pragma warning disable 0108

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MinimalMiner.Util;

namespace MinimalMiner.Entity
{
    /// <summary>
    /// Defines an asteroid gameobject in the scene
    /// </summary>
    public class Asteroid : MonoBehaviour
    {
        #region Fields
        // Core variable
        private AsteroidManager asteroidMgr;

        // Asteroid variables
        [SerializeField] private ColliderListener colliderListener;
        [SerializeField] private Rigidbody2D rigidbody;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private AudioSource audio;
        [SerializeField] private Slider healthBar;
        private Transform healthBarParent;
        private Color32 damageColor;
        private Color32 normalColor;
        private float currHealth = 10f;
        private float flashTimer;
        private float damageTimer;

        // Asteroid characteristics
        private ItemMaterial[] types;
        private AsteroidSize size;
        public float Mass
        {
            get { return ((int)size + 1) * types.Length; }
        }
        public float DamageResistance
        {
            get { return types.Length + 1; }
        }
        public ItemMaterial[] Drops 
        { 
            get { return types; } 
        }
        #endregion

        #region Methods
        /// <summary>
        /// Handles the start of the object before the first frame
        /// </summary>
        private void OnEnable()
        {
            colliderListener.OnCollisionDetected += OnCollisionDetected;
        }

        /// <summary>
        /// Handles subscribing to events
        /// </summary>
        private void OnDisable()
        {
            colliderListener.OnCollisionDetected -= OnCollisionDetected;
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        private void Update()
        {
            // Handle boundaries
            if (transform.position.x < -SceneConstants.BoundarySize.x || transform.position.x > SceneConstants.BoundarySize.x ||
                transform.position.y < -SceneConstants.BoundarySize.y || transform.position.y > SceneConstants.BoundarySize.y)
            {
                float newX;
                float newY;
                if (transform.position.x < -SceneConstants.BoundarySize.x)
                    newX = SceneConstants.BoundarySize.x;
                else if (transform.position.x > SceneConstants.BoundarySize.x)
                    newX = -SceneConstants.BoundarySize.x;
                else
                    newX = transform.position.x;

                if (transform.position.y < -SceneConstants.BoundarySize.y)
                    newY = SceneConstants.BoundarySize.y;
                else if (transform.position.y > SceneConstants.BoundarySize.y)
                    newY = -SceneConstants.BoundarySize.y;
                else
                    newY = transform.position.y;

                transform.position = new Vector2(newX, newY);
            }

            // Handle healthbar
            if (healthBar.gameObject.activeInHierarchy)
            {
                healthBar.value = Mathf.Lerp(healthBar.value, currHealth, SceneConstants.SmoothTime * Time.deltaTime);
                healthBarParent.position = transform.position;
                healthBarParent.rotation = Quaternion.identity;
            }

            // Handle damage flashing
            if (flashTimer > -1f)
            {
                flashTimer += Time.deltaTime;

                if (sprite.material.color == normalColor)
                    sprite.material.color = damageColor;
                else
                    sprite.material.color = normalColor;

                if (flashTimer >= SceneConstants.DamageFlashTime)
                {
                    flashTimer = -1f;
                    sprite.material.color = normalColor;
                }
            }

            // Handle healthbar keep-alive
            if (damageTimer > -1f)
            {
                damageTimer += Time.deltaTime;

                if (damageTimer >= SceneConstants.HealthbarKeepAliveTime)
                {
                    damageTimer = -1f;
                    healthBar.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Handles collision between this asteroid and another object in the scene
        /// </summary>
        /// <param name="collision">Holds the collision information</param>
        private void OnCollisionDetected(Collision2D collision)
        {
            if (collision.gameObject.tag == "asteroid")
            {
                Asteroid collided = collision.gameObject.GetComponentInParent<Asteroid>();
                Vector2 net = rigidbody.velocity - collided.rigidbody.velocity;
                float KE1 = Mass * net.sqrMagnitude / 2;
                float KE2 = collided.Mass * net.sqrMagnitude / 2;
                float dmg1 = KE2 * collided.rigidbody.sharedMaterial.bounciness;
                float dmg2 = KE1 * rigidbody.sharedMaterial.bounciness;

                // Damage is half due to OnCollisionDetected being called on both asteroids
                if (dmg1 > DamageResistance)
                    TakeDamage((dmg1 - DamageResistance) / 2);
                if (dmg2 > collided.DamageResistance)
                    collided.TakeDamage((dmg2 - collided.DamageResistance) / 2);
            }

            else if (collision.gameObject.tag == "player")
            {
                Player collided = collision.gameObject.GetComponent<Player>();
                Vector2 net = rigidbody.velocity - collided.Rigidbody.velocity;
                float KE1 = Mass * net.sqrMagnitude / 2;
                float KE2 = collided.Rigidbody.mass * net.sqrMagnitude / 2;
                float dmg1 = KE2 * collided.Rigidbody.sharedMaterial.bounciness;
                float dmg2 = KE1 * rigidbody.sharedMaterial.bounciness;

                if (dmg1 > DamageResistance)
                    TakeDamage(dmg1 - DamageResistance);
                if (dmg2 > collided.DamageResistance)
                    collided.TakeDamage(dmg2 - collided.DamageResistance);
            }
        }

        /// <summary>
        /// Called when the current Theme is updated
        /// </summary>
        /// <param name="theme">The new GameTheme properties</param>
        private void UpdateTheme(Theme theme)
        {
            sprite.material.color = theme.spriteColor_asteroid;
        }

        /// <summary>
        /// Handles splitting an asteroid if an asteroid is large
        /// </summary>
        /// <returns>A list of newly split asteroids (returns null if a small asteroid)</returns>
        private List<GameObject> Split()
        {
            List<GameObject> newAsteroids = new List<GameObject>();

            if (!(size == AsteroidSize.small))
            {
                int amountToSpawn = Random.Range(2, 5);

                for (int i = 0; i < amountToSpawn; i++)
                {
                    newAsteroids.Add(asteroidMgr.SpawnAsteroid(types, (int)size, rigidbody.velocity, transform.position));
                }
            }

            return newAsteroids;
        }

        /// <summary>
        /// Sets up the asteroid upon instantiation
        /// </summary>
        /// <param name="type">The type that the asteroid should be</param>
        /// <param name="size">The size of the asteroid</param>
        /// <param name="sprite">The sprite to use for the asteroid</param>
        /// <param name="mat">The material to use for the asteroid</param>
        /// <param name="color">The color to use for the sprite</param>
        /// <param name="velocity">The initial velocity of the asteroid</param>
        /// <param name="position">The initial position of the asteroid</param>
        /// <param name="manager">The asteroid manager that spawned this asteroid</param>
        /// <param name="uiColors">The colors for the healthbar</param>
        /// <param name="damageColor">The damage color to use for the asteroid</param>
        public void Setup(ItemMaterial[] types, AsteroidSize size, Sprite sprite, Material mat, Color32 color, Vector2 velocity, Vector3 position, AsteroidManager manager, Color32[] uiColors, Color32 damageColor)
        {
            this.types = types;
            this.size = size;

            switch (size)
            {
                case AsteroidSize.large:
                    transform.localScale = new Vector3(4f, 4f, 4f);
                    break;
                case AsteroidSize.medium:
                    transform.localScale = new Vector3(2f, 2f, 2f);
                    break;
                case AsteroidSize.small:
                default:
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    break;
            }

            GetComponent<Rigidbody2D>().AddForce(velocity * 10f);
            transform.position = position;
            asteroidMgr = manager;
            this.sprite.sprite = sprite;
            this.sprite.material = mat;
            this.sprite.material.color = color;
            healthBar.minValue = 0;
            healthBar.maxValue = currHealth;
            healthBar.value = currHealth;
            healthBar.gameObject.SetActive(false);
            healthBarParent = healthBar.transform.parent;
            normalColor = color;
            flashTimer = -1f;

            ColorBlock block = healthBar.colors;
            block.disabledColor = uiColors[0];
            healthBar.colors = block;
            healthBar.gameObject.GetComponentInChildren<Image>().color = uiColors[1];
            this.damageColor = damageColor;
            damageTimer = -1;
        }

        /// <summary>
        /// Contributes damage to the asteroid
        /// </summary>
        /// <param name="damageDone">The damage to contribute</param>
        public void TakeDamage(float damageDone)
        {
            currHealth -= damageDone;
            damageTimer = 0;

            if (!healthBar.gameObject.activeInHierarchy)
            {
                healthBar.gameObject.SetActive(true);
                healthBarParent.parent.SetParent(null, false); // prevent rotating with asteroid
            }

            if (currHealth <= 0)
            {
                Destroy(healthBarParent.gameObject);
                asteroidMgr.OnAsteroidDestruction(gameObject, Split());
            }

            else
            {
                if(asteroidMgr.DamageFlashOn)
                    flashTimer = 0;
                audio.Play();
            }
        }
        #endregion
    }
}