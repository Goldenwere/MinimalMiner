using UnityEngine;

namespace MinimalMiner.Util
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
        public Color32 sprite_player;

        /// <summary>
        /// The color used for asteroids
        /// </summary>
        public Color32 sprite_asteroid;

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a basic theme
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

            sprite_player = fore;
            sprite_asteroid = fore;
        }

        #endregion
    }
}