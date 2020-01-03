namespace MinimalMiner
{
    /// <summary>
    /// Defines the items in the game
    /// </summary>
    public enum ItemMaterial
    {
        #region Alpha 1.3.0
        #region Raw Asteroid Materials
        #region Generic
        rock,
        ice,
        diamond,
        #endregion

        #region Elements
        carbon,             // abundant
        nickel,             // common - alloys, electroplating
        lithium,            // common - electronics, batteries, lubricant

        phosphorus,         // common - fertiliser, 
        antimony,           // uncommon - alloy, microelectronics
        zinc,               // common - galvanizing/alloys/batteries (anti-corrosion/rust)

        tin,                // uncommon - soldering, alloys
        lead,               // common - barriers, batteries
        indium,             // common - semiconductors

        silver,             // uncommon - conductors
        gold,               // uncommon - electrical-connectors, medicine
        copper,             // common - conductor, alloy

        platinum,           // uncommon (space) - jewelry, catalyst
        cobalt,             // common - batteries, alloys
        iron,               // very common

        osmium,             // rare - alloy, electron microscopy
        uranium,            // very rare - power
        thorium,            // very rare - power
        #endregion

        #region Silicates
        olivine,            // common - good for CO2 reduction, uses requiring repeated heating and cooling
        garnet,             // common - good abrasive, gemstone
        zircon,             // common - good as opacifier, zirconium dioxide (high-heat resistance)
                            
        topaz,              // common - gemstone
        feldspar,           // common - glassmaking, ceramics, filler/extender paint/plastics/rubber
        titanite,           // common - titanium dioxide (pigments)
                            
        hemimorphite,       // rare - Zn4Si2O7(OH)2·H2O
        osumilite,          // very rare - (K,Na)(Fe,Mg)2(Al,Fe)3(Si,Al)12O30
        rhodonite,          // common - ornamental stone
                            
        mica,               // common - many uses (as mica is a group)
        chlorite,           // common - carving (v soft)
        quartz,             // common - various
        #endregion
        #endregion
        #endregion
    }

    /// <summary>
    /// Defines categories for items
    /// </summary>
    public enum ItemCategory
    {
        RawElement,
        RawSilicate,
        RawGeneric,
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
        /// The main playing gamestate, further defined by PlayMode
        /// </summary>
        play,
        /// <summary>
        /// The pause menu
        /// </summary>
        pause,
        /// <summary>
        /// The death menu
        /// </summary>
        death,
        /// <summary>
        /// The mode select menu
        /// </summary>
        modeselect,
        /// <summary>
        /// The ship designer menu (used in sandbox PlayMode)
        /// </summary>
        shipdesigner
    }

    /// <summary>
    /// Represents the different modes for the play GameState
    /// </summary>
    public enum PlayMode
    {
        /// <summary>
        /// Sandbox playmode
        /// </summary>
        sandbox,
        /// <summary>
        /// Career playmode
        /// </summary>
        career,
        /// <summary>
        /// Story playmode
        /// </summary>
        story
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
        controls_active,
        /// <summary>
        /// The graphics menu
        /// </summary>
        graphics
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

