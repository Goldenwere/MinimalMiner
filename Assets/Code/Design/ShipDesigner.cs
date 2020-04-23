#pragma warning disable CS0649

using System;
using System.Collections.Generic;
using UnityEngine;
using MinimalMiner;
using MinimalMiner.Entity;

namespace MinimalMiner.Design
{
    /// <summary>
    /// Used for designing a player ship
    /// </summary>
    public class ShipDesigner : MonoBehaviour
    {
        [SerializeField] private GameObject shipParent;
        [SerializeField] private LowLevelPlayer shipClass;
        [SerializeField] private PlayerManager playerMgr;
        private float value;
        private ShipConfiguration CurrentConfig { get { return shipClass.Config; } }    // to avoid repeating shipClass.Config

        // Current weapon being manipulated in weapon config
        private int weaponIndex;
        private ShipWeapon CurrentWeapon
        {
            get { return shipClass.Config.Stats_Weapons.Weapons[weaponIndex]; }
        }
        private Vector3 weaponRot;
        private Vector3 weaponPos;

        // Current other stuff
        private ShipThrusters CurrentThrusters
        {
            get { return shipClass.Config.Stats_Thrusters; }
        }
        private ShipDefenses CurrentDefenses
        {
            get { return shipClass.Config.Stats_Defenses; }
        }

        private void Start()
        {
            
        }

        /// <summary>
        /// Sets the current working value (used by designer UI)
        /// </summary>
        /// <param name="val">The value to be set</param>
        public void SetValue(float val)
        {
            value = val;
        }

        /// <summary>
        /// Sets the current working value (used by designer UI)
        /// </summary>
        /// <param name="val">The value to be set</param>
        public void SetValue(int val)
        {
            value = val;
        }

        /// <summary>
        /// Sets the current working value (used by designer UI)
        /// </summary>
        /// <param name="s">The value to be parsed</param>
        public void SetValue(string s)
        {
            if (float.TryParse(s, out float val))
                value = val;
        }

        /// <summary>
        /// Defines what the working value is (used by designer UI)
        /// </summary>
        /// <param name="param">The component being manipulated in SetValue, parsed to ShipComponent</param>
        public void OnValueSet(string param)
        {
            if (Enum.TryParse(param, out ShipComponent comp))
                OnValueSet(comp);
            else
                throw new System.Exception("Incorrect parameter supplied for ship designer value association");
        }

        /// <summary>
        /// Defines what the working value is and edits accordingly
        /// </summary>
        /// <param name="comp">The component being manipulated in SetValue</param>
        public void OnValueSet(ShipComponent comp)
        {
            ShipWeapon w;
            ShipThrusters thr;
            ShipDefenses def;
            switch(comp)
            {
                case ShipComponent.Def_Armor:
                    def = CurrentDefenses;
                    def.ArmorStrength = value;
                    shipClass.Config.UpdateDefenseConfig(def);
                    break;
                case ShipComponent.Def_Resistance:
                    def = CurrentDefenses;
                    def.DamageResistance = value;
                    shipClass.Config.UpdateDefenseConfig(def);
                    break;
                case ShipComponent.Def_ShieldDelay:
                    def = CurrentDefenses;
                    def.ShieldDelay = value;
                    shipClass.Config.UpdateDefenseConfig(def);
                    break;
                case ShipComponent.Def_ShieldRecharge:
                    def = CurrentDefenses;
                    def.ShieldRecharge = (float)Math.Round(value, 2);
                    shipClass.Config.UpdateDefenseConfig(def);
                    break;
                case ShipComponent.Def_ShieldStrength:
                    def = CurrentDefenses;
                    def.ShieldStrength = value;
                    shipClass.Config.UpdateDefenseConfig(def);
                    break;
                case ShipComponent.Thr_Dampener:
                    thr = CurrentThrusters;
                    thr.DampenerStrength = (float)Math.Round(value, 2);
                    shipClass.Config.UpdateThrusterConfig(thr);
                    break;
                case ShipComponent.Thr_Forward:
                    thr = CurrentThrusters;
                    thr.ForwardThrusterForce = value;
                    shipClass.Config.UpdateThrusterConfig(thr);
                    break;
                case ShipComponent.Thr_MaxSpd:
                    thr = CurrentThrusters;
                    thr.MaxDirectionalSpeed = value;
                    shipClass.Config.UpdateThrusterConfig(thr);
                    break;
                case ShipComponent.Thr_Recoil:
                    thr = CurrentThrusters;
                    thr.RecoilCompensation = (float)Math.Round(value, 2);
                    shipClass.Config.UpdateThrusterConfig(thr);
                    break;
                case ShipComponent.Thr_Reverse:
                    thr = CurrentThrusters;
                    thr.ReverseThrusterForce = value;
                    shipClass.Config.UpdateThrusterConfig(thr);
                    break;
                case ShipComponent.Thr_RotSpd:
                    thr = CurrentThrusters;
                    thr.RotationalSpeed = value;
                    shipClass.Config.UpdateThrusterConfig(thr);
                    break;
                case ShipComponent.Wpn_Count:
                    ResetWeapons();
                    break;
                case ShipComponent.Wpn_Damage:
                    w = CurrentWeapon;
                    w.Damage = value;
                    shipClass.Config.UpdateWeapon(w, weaponIndex);
                    break;
                case ShipComponent.Wpn_PosX:
                    weaponPos = new Vector3(value, weaponPos.y);
                    shipClass.Config.UpdateWeaponTransform(weaponIndex, weaponPos, TransformChanged.position);
                    break;
                case ShipComponent.Wpn_PosY:
                    weaponPos = new Vector3(weaponPos.x, value);
                    shipClass.Config.UpdateWeaponTransform(weaponIndex, weaponPos, TransformChanged.position);
                    break;
                case ShipComponent.Wpn_RateOfFire:
                    w = CurrentWeapon;
                    w.RateOfFire = (float)Math.Round(value, 2);
                    shipClass.Config.UpdateWeapon(w, weaponIndex);
                    break;
                case ShipComponent.Wpn_Recoil:
                    w = CurrentWeapon;
                    w.Recoil = value;
                    shipClass.Config.UpdateWeapon(w, weaponIndex);
                    break;
                case ShipComponent.Wpn_RotX:
                    weaponRot = new Vector3(value, weaponRot.y);
                    shipClass.Config.UpdateWeaponTransform(weaponIndex, weaponPos, TransformChanged.rotation);
                    break;
                case ShipComponent.Wpn_RotY:
                    weaponRot = new Vector3(weaponRot.x, value);
                    shipClass.Config.UpdateWeaponTransform(weaponIndex, weaponPos, TransformChanged.rotation);
                    break;
                case ShipComponent.Wpn_Selection:
                    weaponIndex = (int)value;
                    weaponRot = shipClass.Config.Stats_Weapons.Rotations[weaponIndex];
                    weaponPos = shipClass.Config.Stats_Weapons.Positions[weaponIndex];
                    break;
                case ShipComponent.Wpn_Speed:
                    w = CurrentWeapon;
                    w.Speed = value;
                    shipClass.Config.UpdateWeapon(w, weaponIndex);
                    break;
                case ShipComponent.Wpn_Type:
                    w = CurrentWeapon;
                    w.Type = (WeaponType)(int)value;
                    shipClass.Config.UpdateWeapon(w, weaponIndex);
                    break;
            }
        }

        /// <summary>
        /// Resets weapons when the weapon count is updated
        /// </summary>
        private void ResetWeapons()
        {
            weaponIndex = 0;
            weaponPos = new Vector3();
            weaponRot = new Vector3();
            ShipWeaponry sw = new ShipWeaponry();
            sw.DamageModifier = 1;
            sw.RateModifier = 1;
            sw.WeaponCount = (int)value;

            sw.Positions = new List<Vector3>();
            for (int i = 0; i < value; i++)
                sw.Positions.Add(new Vector3());

            sw.Rotations = new List<Vector3>();
            for (int i = 0; i < value; i++)
                sw.Rotations.Add(new Vector3());

            sw.SlotStatus = new List<WeaponSlotStatus>();
            for (int i = 0; i < value; i++)
                sw.SlotStatus.Add(WeaponSlotStatus.enabled);

            sw.Weapons = new List<ShipWeapon>();
            for (int i = 0; i < value; i++)
                sw.Weapons.Add(new ShipWeapon());

            shipClass.Config.UpdateWeaponConfig(sw);
        }

        public void InstantiateShip()
        {
            ShipConfiguration config = new ShipConfiguration(CurrentConfig.Stats_Weapons, CurrentConfig.Stats_Defenses,
                CurrentConfig.Stats_Thrusters, CurrentConfig.Mass, CurrentConfig.ColliderForm, CurrentConfig.BodySprite);
            playerMgr.SetupPlayer(config);
        }
    }
}