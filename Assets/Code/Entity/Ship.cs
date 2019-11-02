using System.Collections.Generic;
using UnityEngine;

namespace MinimalMiner.Entity
{
    /// <summary>
    /// Represents a ship configuration in terms of weapons, defensive capabilities, and thrusters
    /// </summary>
    public class ShipConfiguration
    {
        #region Properties
        /// <summary>
        /// Represents the stats of the ship's weapons
        /// </summary>
        public ShipWeaponry Stats_Weapons
        {
            get; private set;
        }

        /// <summary>
        /// Represents the stats of the ship's defenses (the maximum/starting state)
        /// </summary>
        public ShipDefenses Stats_Defenses
        {
            get; private set;
        }

        /// <summary>
        /// Represents the current (live state) defenses of the ship
        /// </summary>
        public ShipDefenses Current_Defenses
        {
            get; private set;
        }

        /// <summary>
        /// Represents the stats of the ship's thrusters
        /// </summary>
        public ShipThrusters Stats_Thrusters
        {
            get; private set;
        }

        /// <summary>
        /// Used in setting up the mass of the ship's Rigidbody
        /// </summary>
        public float Mass
        {
            get; private set;
        }
        
        /// <summary>
        /// Used in setting up the ship's Polygon Collider
        /// </summary>
        public Vector2[] ColliderForm
        {
            get; private set;
        }

        /// <summary>
        /// Applies damage to the ship
        /// </summary>
        /// <param name="damageTaken">The incoming amount of damage to take</param>
        public void TakeDamage(float damageTaken)
        {
            ShipDefenses updatedDef = Current_Defenses;
            updatedDef.ArmorStrength -= damageTaken;
            Current_Defenses = updatedDef;
        }
        #endregion
    }

    /// <summary>
    /// Represents the statistics of a ship's weaponry
    /// </summary>
    public struct ShipWeaponry
    {
        #region Properties
        /// <summary>
        /// Defines the positions of weapon slots on the ship (used as keys for the Weapons and SlotStatus collections)
        /// </summary>
        public List<Vector3> Slots;

        /// <summary>
        /// Defines the weapons on a ship, where the key is the weapon slot's position on the ship and the value is the weapon itself
        /// </summary>
        public Dictionary<Vector3, ShipWeapon> Weapons;

        /// <summary>
        /// Defines the status of the weapon slots on a ship, where the key is the weapon slot's position on the ship and the value is the status of that slot
        /// </summary>
        public Dictionary<Vector3, WeaponSlotStatus> SlotStatus;

        /// <summary>
        /// Modifies all weapons' rate of fire
        /// </summary>
        public float RateModifier;

        /// <summary>
        /// Modifies all weapons' damage
        /// </summary>
        public float DamageModifier;
        #endregion
    }

    /// <summary>
    /// Defines the state of a weapon slot
    /// </summary>
    public enum WeaponSlotStatus
    {
        /// <summary>
        /// Represents a slot that is disabled (some action must be taken to enable it)
        /// </summary>
        disabled,

        /// <summary>
        /// Represents a slot that is damaged (firing won't happen from a damaged weapon)
        /// </summary>
        damaged,

        /// <summary>
        /// Represents a slot that is fully functional
        /// </summary>
        enabled
    }

    /// <summary>
    /// Defines a weapon
    /// </summary>
    public struct ShipWeapon
    {
        #region Properties
        /// <summary>
        /// The name of the weapon
        /// </summary>
        public string Name;

        /// <summary>
        /// The weapon type
        /// </summary>
        public WeaponType Type;

        /// <summary>
        /// The rate at which the weapon fires
        /// </summary>
        public float RateOfFire;

        /// <summary>
        /// The amount of damage the weapon contributes
        /// </summary>
        public float Damage;

        /// <summary>
        /// The speed of the projectile (if applicable)
        /// </summary>
        public float Speed;

        /// <summary>
        /// The force of recoil that is applied to the ship when the weapon fires
        /// </summary>
        public float Recoil;

        /// <summary>
        /// Represents the prefab used for firing the weapon in-game. This can be a Projectile-based prefab or a Beam-based prefab
        /// </summary>
        public GameObject OutputPrefab;

        /// <summary>
        /// Represents the sound used for firing the weapon in-game
        /// </summary>
        public AudioSource OutputSound;
        #endregion
    }

    /// <summary>
    /// Defines what kind of weapon a certain weapon is
    /// </summary>
    public enum WeaponType
    {
        /// <summary>
        /// Projectile-based weapon (spawns a sprite that collides with objects)
        /// </summary>
        projectile,

        /// <summary>
        /// Beam-based weapon (uses RayCasting and LineRenderer)
        /// </summary>
        beam
    }

    /// <summary>
    /// Represents the statistics of a ship's defenses
    /// </summary>
    public struct ShipDefenses
    {
        #region Properties
        /// <summary>
        /// The strength of the ship's shields
        /// </summary>
        public float ShieldStrength;

        /// <summary>
        /// The rate at which the shields recharge
        /// </summary>
        public float ShieldRecharge;

        /// <summary>
        /// The strength of the ship's armor
        /// </summary>
        public float ArmorStrength;
        #endregion
    }

    /// <summary>
    /// Represents the statistics of a ship's thrusters
    /// </summary>
    public struct ShipThrusters
    {
        #region Properties
        /// <summary>
        /// The rate at which the ship rotates
        /// </summary>
        public float RotationalSpeed;

        /// <summary>
        /// The maximum speed forward/reverse
        /// </summary>
        public float MaxDirectionalSpeed;

        /// <summary>
        /// The acceleration of the forward thrusters
        /// </summary>
        public float ForwardThrusterForce;

        /// <summary>
        /// The acceleration of the reverse thrusters
        /// </summary>
        public float ReverseThrusterForce;

        /// <summary>
        /// Compensates for recoil on the ship (factor ranging from 1 [no compensation, full recoil] to 0 [full compensation, no recoil])
        /// </summary>
        public float RecoilCompensation;

        /// <summary>
        /// Strength of the ship's dampeners, applied to rigidbody's drag
        /// </summary>
        public float DampenerStrength;
        #endregion
    }
}
