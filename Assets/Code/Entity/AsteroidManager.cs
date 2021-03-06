﻿#pragma warning disable 0649

using System.Collections.Generic;
using UnityEngine;
using MinimalMiner.Util;
using MinimalMiner.Inventory;

namespace MinimalMiner.Entity
{
    /// <summary>
    /// Maintains the asteroids in a scene when in the playing state
    /// </summary>
    public class AsteroidManager : MonoBehaviour
    {
        #region Fields
        private GameState currState;                                // The current GameState
        private Theme currTheme;                                    // The current Theme
        private List<GameObject> asteroids;                         // The asteroids instantiated in the scene
        [SerializeField] private GameObject asteroidPrefab;         // The asteroid prefab/format to spawn asteroids with
        [SerializeField] private GameObject dropPrefab;             // The drop prefab/format to spawn drops from asteroids with
        [SerializeField] private PreferencesManager prefsMgr;
        public bool DamageFlashOn
        {
            get { return prefsMgr.DoDamageFlash; }
        }

        // Sprites that entities can use
        private List<Sprite> asteroidSprites;
        private List<Sprite> elementSprites;
        private List<Sprite> silicateSprites;
        private List<Sprite> generalItemSprites;
        [SerializeField] private MaterialManager matMgr;            // Used for setting appropriate materials for sprites based on theme
        #endregion

        #region Methods
        /// <summary>
        /// Instantiates asteroids list before events are first handled under Start
        /// </summary>
        private void Awake()
        {
            asteroids = new List<GameObject>();
        }

        /// <summary>
        /// Handles the start of the object before the first frame
        /// </summary>
        private void OnEnable()
        {
            EventManager.OnUpdateGameState += UpdateGameState;
            currState = GameObject.FindWithTag("managers").GetComponent<EventManager>().CurrState;
            PreferencesManager.UpdateTheme += UpdateTheme;
        }

        /// <summary>
        /// Handles subscribing to events
        /// </summary>
        private void OnDisable()
        {
            EventManager.OnUpdateGameState -= UpdateGameState;
            PreferencesManager.UpdateTheme -= UpdateTheme;
        }

        /// <summary>
        /// Handles updates to the theme
        /// </summary>
        /// <param name="theme">The new theme</param>
        private void UpdateTheme(Theme theme)
        {
            currTheme = theme;
            if (theme.spriteImage_asteroid.Count > 0)
            {
                asteroidSprites = new List<Sprite>();

                foreach (Sprite s in theme.spriteImage_asteroid)
                    asteroidSprites.Add(s);
            }

            else
            {
                asteroidSprites = matMgr.Default_Asteroids;
            }

            elementSprites = matMgr.Default_Elements;
            silicateSprites = matMgr.Default_Silicates;
            generalItemSprites = matMgr.Default_GeneralItems;
        }

        /// <summary>
        /// Called when the current GameState is updated
        /// </summary>
        /// <param name="newState">The new GameState after updating</param>
        /// <param name="prevState">The previous GameState before updating</param>
        private void UpdateGameState(GameState newState, GameState prevState)
        {
            currState = newState;

            // Spawn asteroids at the start of gameplay
            if (newState == GameState.play && prevState != GameState.pause)
            {
                SpawnAsteroids();
            }

            // Despawn asteroids at the end of gameplay
            else if (newState == GameState.main && prevState == GameState.pause
                || newState == GameState.main && prevState == GameState.death)
            {
                DespawnAsteroids();
            }
        }

        /// <summary>
        /// Spawns asteroids in the scene
        /// </summary>
        private void SpawnAsteroids()
        {
            // Currently hard-coding max-asteroids until some sort of StarSystem info struct is created
            for (int i = 0; i < 250; i++)
            {
                asteroids.Add(SpawnAsteroid());
            }
        }

        /// <summary>
        /// Despawns the asteroids currently in the scene
        /// </summary>
        private void DespawnAsteroids()
        {
            for (int i = asteroids.Count - 1; i >= 0; i--)
            {
                Destroy(asteroids[i]);
                asteroids.RemoveAt(i);
            }
        }

        /// <summary>
        /// Updates the asteroids list after a collision depletes the health of an asteroid
        /// </summary>
        /// <param name="asteroid">The asteroid whose health has been depleted</param>
        /// <param name="newAsteroids">A new list of asteroids if the parent asteroid was large</param>
        public void OnAsteroidDestruction(GameObject asteroid, List<GameObject> newAsteroids)
        {
            // Spawn items if there aren't new asteroids
            if (newAsteroids.Count == 0)
            {
                ItemMaterial[] drops = asteroid.GetComponent<Asteroid>().Drops;
                foreach(ItemMaterial d in drops)
                {
                    GameObject g = Instantiate(dropPrefab);
                    ItemDrop drop = g.GetComponent<ItemDrop>();
                    Vector3 pos = asteroid.transform.position;
                    float scale = asteroid.transform.localScale.x;
                    pos.x += Random.Range(-scale, scale);
                    pos.y += Random.Range(-scale, scale);
                    drop.SpawnDrop(pos, new Item(d), GenerateItemSprite(Definitions.ItemCategories[d]));
                }
            }

            // Remove current asteroid
            asteroids.Remove(asteroid);
            Destroy(asteroid);

            // Add new asteroids if any
            foreach (GameObject g in newAsteroids)
            {
                asteroids.Add(g);
            }
        }

        /// <summary>
        /// Spawns an asteroid with random characteristics
        /// </summary>
        /// <returns>The asteroid that was spawned</returns>
        public GameObject SpawnAsteroid()
        {
            int spriteIndex = Random.Range(0, asteroidSprites.Count);
            int size = Random.Range(0, 3);
            Vector2 velocity = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
            Vector3 position;
            do
            {
                position = new Vector3(
                    Random.Range(-(SceneConstants.BoundarySize.x - SceneConstants.BodySpawnPadding), SceneConstants.BoundarySize.x - SceneConstants.BodySpawnPadding), 
                    Random.Range(-(SceneConstants.BoundarySize.y - SceneConstants.BodySpawnPadding), SceneConstants.BoundarySize.y - SceneConstants.BodySpawnPadding), 0);
            }
            while (position.x > -SceneConstants.PlayerSafeZone.x && position.x < SceneConstants.PlayerSafeZone.x 
                || position.y > -SceneConstants.PlayerSafeZone.y && position.y < SceneConstants.PlayerSafeZone.y);

            ItemMaterial[] contents = new ItemMaterial[Random.Range(1, 11)];
            for (int i = 0; i < contents.Length; i++)
                contents[i] = GenerateItemMaterial();

            Material mat = null;

            switch (currTheme.import_Asteroids)
            {
                case (int)SpriteImportType.png:
                    mat = matMgr.Mat_Raster;
                    break;
                case (int)SpriteImportType.svggradient:
                    mat = matMgr.Mat_VectorGradient;
                    break;
                case (int)SpriteImportType.svg:
                default:
                    mat = matMgr.Mat_Vector;
                    break;
            }


            GameObject asteroid = Instantiate(asteroidPrefab);

            Asteroid behaviour = asteroid.GetComponent<Asteroid>();

            Color32[] uiColors = new Color32[]
            {
                currTheme.elem_objHealthValue,
                currTheme.elem_objHealthBkgd
            };
            behaviour.Setup(contents, (AsteroidSize)size, asteroidSprites[spriteIndex], mat, currTheme.spriteColor_asteroid, velocity, position, this, uiColors, currTheme.spriteColor_asteroidDamage);

            return asteroid;
        }

        /// <summary>
        /// Spawns an asteroid with characteristics based off of a parent or predetermined asteroid
        /// </summary>
        /// <param name="type">The type of asteroid to spawn</param>
        /// <param name="maxSize">The maximum size of the asteroid (exclusive)</param>
        /// <param name="originalVelocity">The original velocity to base new velocity off of</param>
        /// <param name="originalPosition">The original position to base new position off of</param>
        /// <returns>The asteroid that was spawned</returns>
        public GameObject SpawnAsteroid(ItemMaterial[] types, int maxSize, Vector2 originalVelocity, Vector3 originalPosition)
        {
            int spriteIndex = Random.Range(0, asteroidSprites.Count);
            int size = Random.Range(0, maxSize);
            Vector2 velocity = new Vector2(originalVelocity.x + Random.Range(-5f, 5f), originalVelocity.y + Random.Range(-5f, 5f));

            Material mat = null;

            switch (currTheme.import_Asteroids)
            {
                case (int)SpriteImportType.png:
                    mat = matMgr.Mat_Raster;
                    break;
                case (int)SpriteImportType.svggradient:
                    mat = matMgr.Mat_VectorGradient;
                    break;
                case (int)SpriteImportType.svg:
                default:
                    mat = matMgr.Mat_Vector;
                    break;
            }

            GameObject asteroid = Instantiate(asteroidPrefab);

            Asteroid behaviour = asteroid.GetComponentInChildren<Asteroid>();

            Color32[] uiColors = new Color32[]
{
                currTheme.elem_objHealthValue,
                currTheme.elem_objHealthBkgd
};
            behaviour.Setup(types, (AsteroidSize)size, asteroidSprites[spriteIndex], mat, currTheme.spriteColor_asteroid, velocity, originalPosition, this, uiColors, currTheme.spriteColor_asteroidDamage);

            return asteroid;
        }

        /// <summary>
        /// Randomly generates an ItemMaterial
        /// </summary>
        /// <returns>A random ItemMaterial, based on an external loot table spreadsheet</returns>
        public ItemMaterial GenerateItemMaterial()
        {
            // ItemMaterial can be expanded upon in later versions. This method of iterating over using min-max helps account for that
            // DropWeightTotal is all of the DropWeights added together. This is precalculated using a spreadsheet document that defines items before being written to code
            float chance = Random.Range(0f, (float)Definitions.ItemDropWeightTotal);
            float min;
            float max = 0;
            ItemMaterial[] vals = (ItemMaterial[])System.Enum.GetValues(typeof(ItemMaterial));

            for (int i = 0; i < vals.Length; i++)
            {
                // Define the range
                min = max;
                max = min + Definitions.ItemDropWeights[vals[i]];

                // Return the material if it fits the range
                if (chance >= min && chance <= max)
                    return vals[i];
            }

            return ItemMaterial.rock;
        }

        /// <summary>
        /// Randomly generates an ItemDrop's sprite based on the incoming ItemMaterial
        /// </summary>
        /// <param name="cat">The category of the item drop</param>
        /// <returns>A sprite that matches the item drop's category</returns>
        public Sprite GenerateItemSprite(ItemCategory cat)
        {
            switch(cat)
            {
                case ItemCategory.RawElement:
                    return elementSprites[Random.Range(0, elementSprites.Count)];
                case ItemCategory.RawSilicate:
                    return silicateSprites[Random.Range(0, silicateSprites.Count)];
                case ItemCategory.RawGeneric:
                default:
                    return generalItemSprites[Random.Range(0, generalItemSprites.Count)];
            }
        }
        #endregion
    }
}