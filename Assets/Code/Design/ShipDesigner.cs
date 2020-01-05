using System;
using System.Collections.Generic;
using UnityEngine;
using MinimalMiner;
using MinimalMiner.Entity;

namespace MinimalMiner.Design
{
    public class ShipDesigner : MonoBehaviour
    {
        [SerializeField] private GameObject shipParent;
        [SerializeField] private LowLevelPlayer shipClass;
        private float value;
        private int weaponIndex;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
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
            switch(comp)
            {
                case ShipComponent.Def_Armor:
                    break;
                case ShipComponent.Def_Resistance:
                    break;
                case ShipComponent.Def_ShieldDelay:
                    break;
                case ShipComponent.Def_ShieldRecharge:
                    break;
                case ShipComponent.Def_ShieldStrength:
                    break;
                case ShipComponent.Thr_Dampener:
                    break;
                case ShipComponent.Thr_Forward:
                    break;
                case ShipComponent.Thr_MaxSpd:
                    break;
                case ShipComponent.Thr_Recoil:
                    break;
                case ShipComponent.Thr_Reverse:
                    break;
                case ShipComponent.Thr_RotSpd:
                    break;
                case ShipComponent.Wpn_Count:
                    ResetWeapons();
                    break;
                case ShipComponent.Wpn_Damage:
                    break;
                case ShipComponent.Wpn_PosX:
                    break;
                case ShipComponent.Wpn_PosY:
                    break;
                case ShipComponent.Wpn_RateOfFire:
                    break;
                case ShipComponent.Wpn_Recoil:
                    break;
                case ShipComponent.Wpn_RotX:
                    break;
                case ShipComponent.Wpn_RotY:
                    break;
                case ShipComponent.Wpn_Selection:
                    break;
                case ShipComponent.Wpn_Speed:
                    break;
                case ShipComponent.Wpn_Type:
                    break;
            }
        }

        /// <summary>
        /// Resets weapons when the weapon count is updated
        /// </summary>
        private void ResetWeapons()
        {
            weaponIndex = 0;
            ShipWeaponry sw = new ShipWeaponry();
            sw.DamageModifier = 1;
            sw.RateModifier = 1;

            sw.Slots = new List<Vector3>();
            for (int i = 0; i < value; i++)
                sw.Slots[i] = new Vector3();

            sw.SlotStatus = new Dictionary<Vector3, WeaponSlotStatus>();
            for (int i = 0; i < value; i++)
                sw.SlotStatus[sw.Slots[i]] = WeaponSlotStatus.enabled;

            sw.Rotations = new Dictionary<Vector3, Vector3>();
            for (int i = 0; i < value; i++)
                sw.Rotations[sw.Slots[i]] = new Vector3();
        }
    }
}