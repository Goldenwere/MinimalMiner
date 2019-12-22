namespace MinimalMiner
{
    /// <summary>
    /// Defines the type that an asteroid is
    /// </summary>
    public enum AsteroidType
    {
        general
    }

    /// <summary>
    /// Defines the size that an asteroid is
    /// </summary>
    public enum AsteroidSize
    {
        small,
        medium,
        large
    }

    /// <summary>
    /// Represents the different game states (menus, paused vs gameplay, etc.)
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// The main menu
        /// </summary>
        main,
        /// <summary>
        /// The settings menu
        /// </summary>
        settings,
        /// <summary>
        /// Normal play screen
        /// </summary>
        play,
        /// <summary>
        /// The pause menu
        /// </summary>
        pause,
        /// <summary>
        /// The death menu
        /// </summary>
        death
    }

    /// <summary>
    /// Represents the states of the settings menu
    /// </summary>
    public enum SettingsState
    {
        /// <summary>
        /// The regular settings menu
        /// </summary>
        settings,
        /// <summary>
        /// The controls menu, without a control selected
        /// </summary>
        controls_inactive,
        /// <summary>
        /// The controls menu, with a control selected
        /// </summary>
        controls_active
    }

    /// <summary>
    /// Represents the different components of the HUD that can be updated during the play GameState
    /// </summary>
    public enum HUDElement
    {
        armor,
        shield
    }

    /// <summary>
    /// Represents the file names that theme files must have
    /// </summary>
    public enum ThemeFileName
    {
        asteroid,
        playerShip,
        backgroundNormal
    }

    /// <summary>
    /// Represents the file type imported textures use in the theme
    /// </summary>
    public enum SpriteImportType
    {
        png,
        svg,
        svggradient
    }
}

