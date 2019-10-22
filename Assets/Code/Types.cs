using UnityEngine;

namespace MinimalMiner
{
    /// <summary>
    /// Represents a set of colors and sprites that are applied to the game's UI, HUD, etc.
    /// </summary>
    public struct Theme
    {
        #region Fields

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
        /// The color used for asteroids
        /// </summary>
        public Color32 spriteColor_asteroid;

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a basic-level theme
        /// </summary>
        /// <param name="fore">UI Foreground color</param>
        /// <param name="back">UI Background color</param>
        /// <param name="bkgd">World background</param>
        public Theme(Color32 fore, Color32 back, Sprite bkgd)
        {
            text_primaryHead = fore;
            text_secondaryHead = fore;
            text_body = fore;

            img_backgroundNormal = bkgd;
            img_backgroundColor = back;

            button_normal = back;
            button_hover = back;
            button_active = back;
            button_focus = back;
            button_disabled = back;

            spriteColor_player = fore;
            spriteColor_asteroid = fore;
        }

        /// <summary>
        /// Creates an intermediate-level theme
        /// </summary>
        /// <param name="textFore">Color used for text foregrounds</param>
        /// <param name="spriteFore">Color used for sprite colors</param>
        /// <param name="bkgdColor">World background color</param>
        /// <param name="buttonBack">Color used for button backgrounds</param>
        /// <param name="buttonHover">Color used for buttons when hovering</param>
        /// <param name="buttonActive">Color used for buttons when active (clicked)</param>
        /// <param name="buttonFocus">Color used for buttons when focused (selected)</param>
        /// <param name="buttonDisabled">Color used for buttons when disabled</param>
        /// <param name="bkgd">World background image</param>
        public Theme(Color32 textFore, Color32 spriteFore, Color32 bkgdColor, Color32 buttonBack, Color32 buttonHover, Color32 buttonActive, Color32 buttonFocus, Color32 buttonDisabled, Sprite bkgd)
        {
            text_primaryHead = textFore;
            text_secondaryHead = textFore;
            text_body = textFore;

            img_backgroundNormal = bkgd;
            img_backgroundColor = bkgdColor;

            button_normal = buttonBack;
            button_hover = buttonHover;
            button_active = buttonActive;
            button_focus = buttonFocus;
            button_disabled = buttonDisabled;

            spriteColor_player = spriteFore;
            spriteColor_asteroid = spriteFore;
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

        public KeyCode Menu_Pause;
    }
}