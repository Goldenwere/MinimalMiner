#pragma warning disable 0618

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
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
                theme = AssignSprites(theme);

                // TO-DO: only assign sprites on a selected theme

                return theme;
            }

            catch (Exception ex)
            {
                MonoBehaviour.print(ex);
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
        /// <returns>The sprite found</returns>
        public static Sprite GetSprite(string themeName, string spriteName)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath + "/Themes/" + themeName);
            FileInfo[] files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
            Sprite sprite = null;
            foreach (FileInfo file in files)
            {
                if (Path.GetFileNameWithoutExtension(file.Name) == spriteName && !file.Name.Contains("meta"))
                {
                    // WWW is obsolete, needs replaced eventually
                    WWW www = new WWW(file.FullName);
                    Texture2D tex = www.texture;

                    // Determine size
                    int size = 0;
                    if (spriteName == "asteroid")
                        size = 512;
                    else if (spriteName.Contains("background"))
                        size = 2048;
                    else if (spriteName == "playerShip")
                        size = 512;

                    Texture2D newTex = MonoBehaviour.Instantiate(tex);
                    TextureScaler.Bilinear(newTex, size, size);
                    

                    // Create the sprite
                    sprite = Sprite.Create(newTex, new Rect(new Vector2(), new Vector2(size, size)), new Vector2(0.5f, 0.5f), size * 2);
                }
            }

            return sprite;
        }

        /// <summary>
        /// Creates sprites stored at the specified theme directory, to be used for assigning sprites to a theme
        /// </summary>
        /// <param name="themeName">The name of the theme (for path)</param>
        /// <param name="spriteName">The name of the sprite</param>
        /// <returns>The sprites found</returns>
        public static List<Sprite> GetSprites(string themeName, string spriteName)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath + "/Themes/" + themeName);
            FileInfo[] files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
            List<Sprite> sprites = new List<Sprite>();
            foreach (FileInfo file in files)
            {
                if (file.Name.Contains(spriteName) && !file.Name.Contains("meta"))
                {
                    // WWW is obsolete, needs replaced eventually
                    WWW www = new WWW(file.FullName);
                    Texture2D tex = www.texture;
                    tex.alphaIsTransparency = true;

                    // Determine size
                    int size = 0;
                    if (spriteName == "asteroid")
                        size = 512;
                    else if (spriteName.Contains("background"))
                        size = 2048;
                    else if (spriteName == "playerShip")
                        size = 512;

                    Texture2D newTex = MonoBehaviour.Instantiate(tex);
                    TextureScaler.Bilinear(newTex, size, size);

                    // Create the sprite
                    sprites.Add(Sprite.Create(newTex, new Rect(new Vector2(), new Vector2(size, size)), new Vector2(0.5f, 0.5f), size * 2));
                }
            }

            return sprites;
        }

        /// <summary>
        /// Assigns sprites to a theme
        /// </summary>
        /// <param name="theme">The theme to edit and assign sprites to</param>
        /// <returns>The theme that was edited</returns>
        /// <remarks>This should only be called on an active theme to prevent overusage of memory</remarks>
        public static Theme AssignSprites(Theme theme)
        {
            theme.img_backgroundNormal = GetSprite(theme.themeName, ThemeFileName.backgroundNormal.ToString());
            theme.spriteImage_asteroid = GetSprites(theme.themeName, ThemeFileName.asteroid.ToString());
            theme.spriteImage_player = GetSprite(theme.themeName, ThemeFileName.playerShip.ToString());

            return theme;
        }
    }
}