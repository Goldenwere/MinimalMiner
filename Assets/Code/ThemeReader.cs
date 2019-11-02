#pragma warning disable 0618

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Unity.VectorGraphics;
using UnityEngine;
using TextureScale;

namespace MinimalMiner
{
    /// <summary>
    /// Handles the reading of theme files (written in XML format)
    /// </summary>
    public static class ThemeReader
    {
        /// <summary>
        /// Reads from a specified file and returns a Theme object
        /// </summary>
        /// <param name="file">The .theme file</param>
        /// <returns>The Theme data stored in the file</returns>
        public static Theme GetTheme(FileInfo file)
        {
            // IO stuff
            StreamReader reader = null;
            StringReader sr = null;
            XmlSerializer serializer = null;
            XmlTextReader xmlReader = null;

            // Needed for creating theme to return
            Theme theme;
            Type type = typeof(Theme);
            try
            {
                // Read the file
                reader = new StreamReader(file.OpenRead());
                string data = reader.ReadToEnd();
                sr = new StringReader(data);

                // Deserialize the file
                serializer = new XmlSerializer(type);
                xmlReader = new XmlTextReader(sr);
                theme = (Theme)serializer.Deserialize(xmlReader);

                // Assign sprites and return theme
                //theme = AssignSprites(theme);

                // TO-DO: only assign sprites on a selected theme

                return theme;
            }

            catch (Exception e)
            {
                MonoBehaviour.print(e.Message + "\nFile: " + file.Name + "\n" + e.StackTrace);
            }

            finally
            {
                if (xmlReader != null)
                    xmlReader.Close();
                if (sr != null)
                    sr.Close();
                if (reader != null)
                    reader.Close();
            }

            return new Theme("undefined");
        }

        /// <summary>
        /// Creates a sprite stored at the specified theme directory, to be used for assigning sprites to a theme
        /// </summary>
        /// <param name="themeName">The name of the theme (for path)</param>
        /// <param name="spriteName">The name of the sprite</param>
        /// <param name="spriteType">The sprite type (for import method)</param>
        /// <returns>The sprite found</returns>
        public static Sprite GetSprite(string themeName, string spriteName, SpriteImportType spriteType)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath + "/Themes/" + themeName);
            FileInfo[] files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
            Sprite sprite = null;
            foreach (FileInfo file in files)
            {
                if (Path.GetFileNameWithoutExtension(file.Name) == spriteName && !file.Name.Contains("meta"))
                {
                    int size = DetermineSize(spriteName);
                    switch (spriteType)
                    {
                        case SpriteImportType.svg:
                        case SpriteImportType.svggradient:
                            sprite = ImportVector(file, size, spriteType);
                            break;
                        case SpriteImportType.png:
                        default:
                            sprite = ImportRaster(file, size);
                            break;
                    }
                }
            }

            return sprite;
        }

        /// <summary>
        /// Creates sprites stored at the specified theme directory, to be used for assigning sprites to a theme
        /// </summary>
        /// <param name="themeName">The name of the theme (for path)</param>
        /// <param name="spriteName">The name of the sprite</param>
        /// <param name="spriteType">The sprite type (for import method)</param>
        /// <returns>The sprites found</returns>
        public static List<Sprite> GetSprites(string themeName, string spriteName, SpriteImportType spriteType)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath + "/Themes/" + themeName);
            FileInfo[] files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
            List<Sprite> sprites = new List<Sprite>();
            foreach (FileInfo file in files)
            {
                if (file.Name.Contains(spriteName) && !file.Name.Contains("meta"))
                {
                    int size = DetermineSize(spriteName);
                    switch(spriteType)
                    {
                        case SpriteImportType.svg:
                        case SpriteImportType.svggradient:
                            sprites.Add(ImportVector(file, size, spriteType));
                            break;
                        case SpriteImportType.png:
                        default:
                            sprites.Add(ImportRaster(file, size));
                            break;
                    }
                }
            }

            return sprites;
        }

        /// <summary>
        /// Imports a raster texture
        /// </summary>
        /// <param name="file">The file to import from</param>
        /// <param name="size">The size of the texture</param>
        /// <returns>The raster texture imported as a Sprite</returns>
        public static Sprite ImportRaster(FileInfo file, int size)
        {
            // WWW is obsolete, needs replaced eventually
            WWW www = new WWW(file.FullName);
            Texture2D tex = www.texture;
            //tex.alphaIsTransparency = true;

            Texture2D newTex = MonoBehaviour.Instantiate(tex);
            TextureScaler.Bilinear(newTex, size, size);

            Sprite sprite = Sprite.Create(newTex, new Rect(Vector2.zero, new Vector2(size, size)), new Vector2(0.5f, 0.5f), size * 2);

            return sprite;
        }

        /// <summary>
        /// Imports a vector texture
        /// </summary>
        /// <param name="file">The file to import from</param>
        /// <param name="size">The size of the texture</param>
        /// <param name="type"></param>
        /// <returns>The vector texture imported as a Sprite</returns>
        public static Sprite ImportVector(FileInfo file, int size, SpriteImportType type)
        {
            StreamReader reader = null;
            StringReader sr = null;
            try
            {
                reader = new StreamReader(file.OpenRead());
                sr = new StringReader(reader.ReadToEnd());
                SVGParser.SceneInfo scene = SVGParser.ImportSVG(sr);

                VectorUtils.TessellationOptions tessOptions = new VectorUtils.TessellationOptions()
                {
                    StepDistance = 100.0f,
                    MaxCordDeviation = 0.5f,
                    MaxTanAngleDeviation = 0.1f,
                    SamplingStepSize = 0.01f
                };

                List<VectorUtils.Geometry> geoms = VectorUtils.TessellateScene(scene.Scene, tessOptions);
                Sprite tempSprite = VectorUtils.BuildSprite(geoms, size, VectorUtils.Alignment.Center, Vector2.zero, 64, false);

                Shader shader = null;
                switch (type)
                {
                    case SpriteImportType.svggradient:
                        shader = Shader.Find("Unlit/VectorGradient");
                        break;
                    case SpriteImportType.svg:
                    default:
                        shader = Shader.Find("Unlit/Vector");
                        break;
                }

                Texture2D tex = VectorUtils.RenderSpriteToTexture2D(tempSprite, size, size, new Material(shader));
                //tex.alphaIsTransparency = true;
                Sprite sprite = Sprite.Create(tex, new Rect(Vector2.zero, new Vector2(tex.width, tex.height)), new Vector2(0.5f, 0.5f), size * 2);

                return sprite;
            }

            catch (Exception e)
            {
                MonoBehaviour.print(e.Message + "\nFile: " + file.Name + "\n" + e.StackTrace);
            }

            finally
            {
                if (reader != null)
                    reader.Close();
                if (sr != null)
                    sr.Close();
            }

            return null;
        }

        /// <summary>
        /// Determines the size of a texture (themes use specific set sizes when importing textures)
        /// </summary>
        /// <param name="spriteName">The sprite being imported</param>
        /// <returns>The size that a sprite is to be</returns>
        public static int DetermineSize(string spriteName)
        {
            // Backgrounds are 2048px in size
            if (spriteName.Contains("background"))
                return 2048;
            // Asteroids and the player ship are 512px in size
            else if (spriteName == "asteroid" || spriteName == "playerShip")
                return 512;

            // Other sprites default to 512
            else
                return 512;
        }

        /// <summary>
        /// Determines the SpriteImportType that a certain sprite is to be loaded with
        /// </summary>
        /// <param name="spriteName">The name of the sprite being imported</param>
        /// <param name="theme">Reference to the current theme being checked</param>
        /// <returns>The type that a sprite is to be</returns>
        public static SpriteImportType DetermineSprite(string spriteName, Theme theme)
        {
            if (spriteName.Contains("background"))
                return (SpriteImportType)theme.import_Backgrounds;
            else if (spriteName == "asteroid")
                return (SpriteImportType)theme.import_Asteroids;
            else if (spriteName == "playerShip")
                return (SpriteImportType)theme.import_Player;

            else
                return SpriteImportType.png;
        }

        /// <summary>
        /// Assigns sprites to a theme
        /// </summary>
        /// <param name="theme">The theme to edit and assign sprites to</param>
        /// <returns>The theme that was edited</returns>
        /// <remarks>This should only be called on an active theme to prevent overusage of memory</remarks>
        public static Theme AssignSprites(Theme theme)
        {
            theme.img_backgroundNormal = GetSprite(theme.themeName, ThemeFileName.backgroundNormal.ToString(), 
                DetermineSprite(ThemeFileName.backgroundNormal.ToString(), theme));

            theme.spriteImage_asteroid = GetSprites(theme.themeName, ThemeFileName.asteroid.ToString(), 
                DetermineSprite(ThemeFileName.asteroid.ToString(), theme));

            theme.spriteImage_player = GetSprite(theme.themeName, ThemeFileName.playerShip.ToString(), 
                DetermineSprite(ThemeFileName.playerShip.ToString(), theme));

            return theme;
        }
    }
}