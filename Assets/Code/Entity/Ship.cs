using System.Collections.Generic;
using UnityEngine;
using MinimalMiner.Util;
using System;

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
        /// Represents the main sprite of the ship
        /// </summary>
        public Sprite BodySprite
        {
            get; private set;
        }

        // Timer used for delaying shield recharge
        private float rechargeDelay;
        // For updating text without a damage event
        private EventManager eventMgr;
        #endregion

        #region Methods
        /// <summary>
        /// Creates a ShipConfiguration with the specified parameters
        /// </summary>
        /// <param name="wpn">Ship's weapons</param>
        /// <param name="def">Ship's defenses</param>
        /// <param name="thr">Ship's thrusters</param>
        /// <param name="mas">Ship's mass</param>
        /// <param name="col">Ship's colliders</param>
        /// <param name="spr">Ship's body sprite</param>
        public ShipConfiguration(ShipWeaponry wpn, ShipDefenses def, ShipThrusters thr, float mas, Vector2[] col, Sprite spr, EventManager e)
        {
            Stats_Weapons = wpn;
            Stats_Defenses = def;
            Current_Defenses = def;
            Stats_Thrusters = thr;
            Mass = mas;
            ColliderForm = col;
            BodySprite = spr;
            eventMgr = e;
        }

        /// <summary>
        /// Applies damage to the ship
        /// </summary>
        /// <param name="damageTaken">The incoming amount of damage to take</param>
        public bool TakeDamage(float damageTaken)
        {
            ShipDefenses updatedDef = Current_Defenses;

            if (updatedDef.ShieldStrength <= 0)
                updatedDef.ArmorStrength -= damageTaken;

            else
            {
                if ((updatedDef.ShieldStrength - damageTaken) <= 0)
                    updatedDef.ShieldStrength = 0;

                else
                    updatedDef.ShieldStrength -= damageTaken;

                rechargeDelay = 0;
            }

            Current_Defenses = updatedDef;

            if (Current_Defenses.ArmorStrength > 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Resets ship properties
        /// </summary>
        public void ResetShip()
        {
            Current_Defenses = Stats_Defenses;
        }

        /// <summary>
        /// Called every time the ship's parent manager is updated
        /// </summary>
        /// <param name="dt">Delta time (between frames)</param>
        public void Update(float dt)
        {
            // If the shield strength is down, start recharge process
            if (Current_Defenses.ShieldStrength < Stats_Defenses.ShieldStrength)
            {
                // Increment timer 
                if (rechargeDelay < Current_Defenses.ShieldDelay)
                    rechargeDelay += dt;

                // Increment shields
                else
                {
                    ShipDefenses updatedDef = Current_Defenses;
                    updatedDef.ShieldStrength += Current_Defenses.ShieldRecharge * dt;
                    Current_Defenses = updatedDef;
                    eventMgr.UpdateHUDElement(HUDElement.shield, Math.Round(updatedDef.ShieldStrength, 2).ToString());
                }
            }

            // Ensure shields don't go over after recharge process
            else if (Current_Defenses.ShieldStrength > Stats_Defenses.ShieldStrength)
            {
                ShipDefenses updatedDef = Current_Defenses;
                updatedDef.ShieldStrength = Stats_Defenses.ShieldStrength;
                Current_Defenses = updatedDef;
                eventMgr.UpdateHUDElement(HUDElement.shield, Math.Round(updatedDef.ShieldStrength, 2).ToString());
            }
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
        /// Defines the rotations of weapon slots on the ship, where the key is the wepaon slot's position and the value is the rotation itself
        /// </summary>
        public Dictionary<Vector3, Vector3> Rotations;

        /// <summary>
        /// Defines the weapons on a ship, where the key is the weapon slot's position on the ship and the value is the weapon itself
        /// </summary>
        public Dictionary<Vector3, ShipWeapon> Weapons;

        /// <summary>
        /// Defines the status of the weapon slots on a ship, where the key is the weapon slot's position on the ship and the value is the status of that slot
        /// </summary>
        public Dictionary<Vector3, WeaponSlotStatus> SlotStatus;

        /// <summary>
        /// Modifies all weapons' rate of fire by this multiplied factor
        /// </summary>
        public float RateModifier;

        /// <summary>
        /// Modifies all weapons' damage by this multiplied factor
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
        /// The rate at which the shields recharge per second
        /// </summary>
        public float ShieldRecharge;

        /// <summary>
        /// The delay until shields begin recharging
        /// </summary>
        public float ShieldDelay;

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
