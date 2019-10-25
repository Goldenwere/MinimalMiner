using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

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
        /// <param name="themeName"></param>
        /// <param name="spriteName"></param>
        /// <returns></returns>
        public static Sprite GetSprite(string themeName, string spriteName)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath + "/Themes/" + themeName);
            FileInfo[] files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
            Sprite sprite = null;
            foreach (FileInfo file in files)
            {
                if (Path.GetFileNameWithoutExtension(file.Name) == spriteName)
                {
                    // WWW is obsolete, needs replaced eventually
                    WWW www = new WWW(file.FullName);
                    Texture2D tex = www.texture;
                    sprite = Sprite.Create(tex, new Rect(new Vector2(), new Vector2(tex.width, tex.height)), new Vector2(tex.width / 2, tex.height / 2));
                }
            }

            return sprite;
        }

        /// <summary>
        /// Assigns sprites to a theme
        /// </summary>
        /// <param name="theme"></param>
        /// <returns></returns>
        /// <remarks>This should only be called on an active theme to prevent overusage of memory</remarks>
        public static Theme AssignSprites(Theme theme)
        {

            theme.img_backgroundNormal = GetSprite(theme.themeName, ThemeFileName.backgroundNormal.ToString());

            return theme;
        }
    }
}