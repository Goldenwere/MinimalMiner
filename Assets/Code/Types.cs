﻿#pragma warning disable 0618

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MinimalMiner
{
    /// <summary>
    /// Represents a set of colors and sprites that are applied to the game's UI, HUD, etc.
    /// </summary>
    public struct Theme
    {
        #region Fields

        #region Theme Properties
        public string themeName;
        public int import_Backgrounds;
        public int import_Asteroids;
        public int import_Player;
        #endregion

        #region UI text
        /// <summary>
        /// UI - Text - Primary heading
        /// </summary>
        public Color32 text_primaryHead;

        /// <summary>
        /// UI - Text - Secondary heading
        /// </summary>
        public Color32 text_secondaryHead;

        /// <summary>
        /// UI - Text - Body text
        /// </summary>
        public Color32 text_body;

        #endregion

        #region Button colors

        /// <summary>
        /// UI - Button - Normal button color
        /// </summary>
        public Color32 button_normal;

        /// <summary>
        /// UI - Button - Color on mouse hover
        /// </summary>
        public Color32 button_hover;

        /// <summary>
        /// UI - Button - Color on mouse click
        /// </summary>
        public Color32 button_active;

        /// <summary>
        /// UI - Button - Color on button selected
        /// </summary>
        public Color32 button_focus;

        /// <summary>
        /// UI - Button - Disabled button color
        /// </summary>
        public Color32 button_disabled;

        #endregion

        #region UI Elements
        /// <summary>
        /// The value color of health bar sliders
        /// </summary>
        public Color32 elem_objHealthValue;

        /// <summary>
        /// The background color of health bar sliders
        /// </summary>
        public Color32 elem_objHealthBkgd;
        #endregion

        #region Images

        /// <summary>
        /// Image - Background image used during normal game-play
        /// </summary>
        public Sprite img_backgroundNormal;

        /// <summary>
        /// Image - Color used when no background image present (applies to camera clear color)
        /// </summary>
        public Color32 img_backgroundColor;

        #endregion

        #region Sprites

        /// <summary>
        /// The color used for player (bullets and ship)
        /// </summary>
        public Color32 spriteColor_player;

        /// <summary>
        /// The color used for the player's flashing damage
        /// </summary>
        public Color32 spriteColor_playerDamage;

        /// <summary>
        /// The sprite used for the player
        /// </summary>
        public Sprite spriteImage_player;

        /// <summary>
        /// The color used for asteroids
        /// </summary>
        public Color32 spriteColor_asteroid;

        /// <summary>
        /// The color used for the asteroid's flashing damage
        /// </summary>
        public Color32 spriteColor_asteroidDamage;

        /// <summary>
        /// The sprites used for the asteroids
        /// </summary>
        public List<Sprite> spriteImage_asteroid;

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a default theme
        /// </summary>
        public Theme(string name)
        {
            themeName = name;

            Color32 text = new Color32(20, 20, 20, 255);
            Color32 sprite = new Color32(30, 30, 30, 255);
            Color32 bkgd = new Color32(235, 235, 235, 255);
            Color32 bBack = new Color32(200, 200, 200, 255);
            Color32 bHover = new Color32(170, 170, 170, 255);
            Color32 bActive = new Color32(140, 140, 140, 255);
            Color32 bFocus = new Color32(255, 255, 255, 255);
            Color32 bDisabled = new Color32(100, 100, 100, 255);

            text_primaryHead = text;
            text_secondaryHead = text;
            text_body = text;

            img_backgroundColor = bkgd;

            button_normal = bBack;
            button_hover = bHover;
            button_active = bActive;
            button_focus = bFocus;
            button_disabled = bDisabled;

            spriteColor_asteroid = sprite;
            spriteColor_player = sprite;
            spriteColor_playerDamage = new Color32(255, 0, 0, 255);
            spriteColor_asteroidDamage = new Color32(255, 0, 0, 255);

            img_backgroundNormal = null;
            spriteImage_asteroid = new List<Sprite>();
            spriteImage_player = null;

            import_Asteroids = 0;
            import_Backgrounds = 0;
            import_Player = 0;

            elem_objHealthValue = sprite;
            elem_objHealthBkgd = bkgd;
        }

        /// <summary>
        /// Creates a basic-level theme
        /// </summary>
        /// <param name="name">The name of the theme</param>
        /// <param name="fore">UI Foreground color</param>
        /// <param name="back">UI Background color</param>
        public Theme(string name, Color32 fore, Color32 back)
        {
            themeName = name;
            text_primaryHead = fore;
            text_secondaryHead = fore;
            text_body = fore;

            img_backgroundColor = back;

            button_normal = back;
            button_hover = back;
            button_active = back;
            button_focus = back;
            button_disabled = back;

            spriteColor_player = fore;
            spriteColor_asteroid = fore;
            spriteColor_playerDamage = new Color32(255, 0, 0, 255);
            spriteColor_asteroidDamage = new Color32(255, 0, 0, 255);

            img_backgroundNormal = null;
            spriteImage_asteroid = new List<Sprite>();
            spriteImage_player = null;

            import_Asteroids = 0;
            import_Backgrounds = 0;
            import_Player = 0;

            elem_objHealthValue = fore;
            elem_objHealthBkgd = back;
        }

        /// <summary>
        /// Creates an intermediate-level theme
        /// </summary>
        /// <param name="name">The name of the theme</param>
        /// <param name="textFore">Color used for text foregrounds</param>
        /// <param name="spriteFore">Color used for sprite colors</param>
        /// <param name="bkgdColor">World background color</param>
        /// <param name="buttonBack">Color used for button backgrounds</param>
        /// <param name="buttonHover">Color used for buttons when hovering</param>
        /// <param name="buttonActive">Color used for buttons when active (clicked)</param>
        /// <param name="buttonFocus">Color used for buttons when focused (selected)</param>
        /// <param name="buttonDisabled">Color used for buttons when disabled</param>
        public Theme(string name, Color32 textFore, Color32 spriteFore, Color32 bkgdColor, Color32 buttonBack, Color32 buttonHover, Color32 buttonActive, Color32 buttonFocus, Color32 buttonDisabled)
        {
            string path = Application.streamingAssetsPath + "/Themes/" + name;
            themeName = name;
            text_primaryHead = textFore;
            text_secondaryHead = textFore;
            text_body = textFore;

            img_backgroundColor = bkgdColor;

            button_normal = buttonBack;
            button_hover = buttonHover;
            button_active = buttonActive;
            button_focus = buttonFocus;
            button_disabled = buttonDisabled;

            spriteColor_player = spriteFore;
            spriteColor_asteroid = spriteFore;
            spriteColor_playerDamage = new Color32(255, 0, 0, 255);
            spriteColor_asteroidDamage = new Color32(255, 0, 0, 255);

            img_backgroundNormal = null;
            spriteImage_asteroid = new List<Sprite>();
            spriteImage_player = null;

            import_Asteroids = 0;
            import_Backgrounds = 0;
            import_Player = 0;

            elem_objHealthValue = textFore;
            elem_objHealthBkgd = bkgdColor;
        }
        #endregion
    }

    /// <summary>
    /// Defines customizable controls
    /// </summary>
    public struct InputDefinitions
    {
        public KeyCode Ship_Forward;
        public KeyCode Ship_Reverse;
        public KeyCode Ship_CW;
        public KeyCode Ship_CCW;
        public KeyCode Ship_Fire;
        public KeyCode Ship_Dampener;

        public KeyCode Menu_Pause;
    }

    public struct GraphicSettings
    {
        public int PostProcessingPreset;
        public int AntiAliasingMode;
        public int TargetFramerate;
        public bool UseVsync;
    }

    /// <summary>
    /// Defines player preferences
    /// </summary>
    public struct PlayerPreferences
    {
        /// <summary>
        /// The player's control preferences
        /// </summary>
        public InputDefinitions Controls;

        /// <summary>
        /// The player's graphical preferences
        /// </summary>
        public GraphicSettings Graphics;

        /// <summary>
        /// The player's selected theme preference
        /// </summary>
        /// <remarks>(reset to 0 if exceeds installed themes e.g. in the situation that themes were removed)</remarks>
        public int CurrentTheme;

        /// <summary>
        /// The force of the soft targeting mechanism (must be a number greater than one, or else destroys rotation)
        /// </summary>
        public float TargetLockForce;

        /// <summary>
        /// The player's preference for sprite flashing on damage taken
        /// </summary>
        public bool DoDamageFlashing;
    }
}