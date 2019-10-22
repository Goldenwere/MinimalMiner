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
        main,
        settings,
        play,
        pause,
        death
    }

    /// <summary>
    /// Represents the different components of the HUD that can be updated during the play GameState
    /// </summary>
    public enum HUDElement
    {
        health
    }
}

