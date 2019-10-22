This is a temporary documentation file whose data will be transferred to a wiki once the repo is made public.

------

# MinimalMiner

## About

The MinimalMiner namespace contains certain types and enumerations that are to be used by multiple child namespaces.

## Types

- Theme
- InputDefinitions

## Enumerations

- AsteroidSize
- AsteroidType
- GameState

------

# MinimalMiner.Entity

## About

The Entity namespace contains the behaviours of entities that are spawned in the playing state, such as the player, npcs, and inanimate objects.

## Classes

- Asteroid
- AsteroidManager
- Projectile
- Player

------

# MinimalMiner.UI

## About

The UI (user interface) namespace contains the behaviours of various UI elements such as menus and HUDs

## Classes

- MenuManager

------

# MinimalMiner.Util

## About

The Util namespace contains various utility classes that other namespaces depend on.

## Classes

- CameraTracking
- EventManager
- PlayerPreferences

------
------

# Roadmap

## 1.0.0

Very similar functionality to the project that this was based off of

## TBA 1

Saving and loading theme files, game sprites (asteroid array, player, etc.) are part of themes

## TBA 2

Saving and loading controls

## TBA 3

Splitting up ship aspects such as weaponry, defenses, etc; has different sprites (bullets, ship, etc), properties (speed, etc.)

## TBA 4

Some sort of restricted zoning, asteroids despawn outside zone (like screen-wrapping except not)

## TBA 5

Physics: use original player movement physics as 'dampening,' have a non-dampening mode (e.g. make turning independent of velocity in this mode, achieves drifting effect)
