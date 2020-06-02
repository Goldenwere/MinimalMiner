using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MinimalMiner.Entity;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SandboxSaveData : MonoBehaviour
{

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private AudioSource bulletSound;
    [SerializeField] private AudioSource bulletSound2;
    [SerializeField] private Sprite shipSprite;

    public ShipConfiguration ReadSaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/sandbox.sav", FileMode.Open);

        if (file != null)
            return JsonUtility.FromJson<ShipConfiguration>((string)bf.Deserialize(file));
        else
            return DefaultConfig();
    }

    public void WriteSaveData(ShipConfiguration config)
    {
        string json = JsonUtility.ToJson(config);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/sandbox.sav");
        bf.Serialize(file, json);
        file.Close();
    }

    private ShipConfiguration DefaultConfig()
    {
        ShipDefenses defenses = new ShipDefenses();
        ShipThrusters thrusters = new ShipThrusters();
        ShipWeaponry weapons = new ShipWeaponry();
        Vector2[] colliders = null;

        #region Setup

        defenses.ArmorStrength = 50f;
        defenses.ShieldRecharge = 1f;
        defenses.ShieldStrength = 15f;
        defenses.ShieldDelay = 5f;
        defenses.DamageResistance = 15f;

        thrusters.DampenerStrength = 0.5f;                  // Equivalent to rigidbody linear drag set before this temp code (shipDragRate was unused)
        thrusters.ForwardThrusterForce = 10f;               // Equivalent to shipAccRate set in the old player class ResetPlayer()
        thrusters.MaxDirectionalSpeed = 10f;                // Equivalent to shipMaxSpd set in old player class
        thrusters.RecoilCompensation = 0.9f;
        thrusters.ReverseThrusterForce = 3f;
        thrusters.RotationalSpeed = 5f;                     // Equivalent to shipRotSpd set in old player class

        ShipWeapon basicBlaster = new ShipWeapon();
        basicBlaster.Damage = 5f;                           // Original hardcoded value in prototyped Projectile was 5
        basicBlaster.Name = "Basic Blaster";
        basicBlaster.OutputPrefab = bulletPrefab;           // The prefabs and sounds will eventually be handled/stored outside Player
        basicBlaster.OutputSound = bulletSound;
        basicBlaster.RateOfFire = 0.35f;                    // Originally fireRate in old Player
        basicBlaster.Recoil = 15f;                          // Originally -shipAcc * 5f when handling recoil in old Player
        basicBlaster.Speed = 200f;                          // Originally projectileSpeed in old Player
        basicBlaster.Type = WeaponType.projectile;
        ShipWeapon secondBlaster = new ShipWeapon();
        secondBlaster.Damage = 1f;
        secondBlaster.Name = "Secondary Blaster";
        secondBlaster.OutputPrefab = bulletPrefab;
        secondBlaster.OutputSound = bulletSound2;
        secondBlaster.RateOfFire = 0.2f;
        secondBlaster.Recoil = 5f;
        secondBlaster.Speed = 100f;
        secondBlaster.Type = WeaponType.projectile;

        weapons.DamageModifier = 1;
        weapons.RateModifier = 1;
        weapons.WeaponCount = 3;
        weapons.Positions = new List<Vector3>()
            {
                new Vector3(0.35f,0,0),
                new Vector3(0.25f,0.21f,0),
                new Vector3(0.25f,-0.21f,0)
            };
        weapons.Rotations = new List<Vector3>
            {
                { new Vector3(0,0,0) },
                { new Vector3(0,0,10f) },
                { new Vector3(0,0,-10f) }
            };
        weapons.SlotStatus = new List<WeaponSlotStatus>
            {
                { WeaponSlotStatus.enabled },
                { WeaponSlotStatus.enabled },
                { WeaponSlotStatus.enabled }
            };
        weapons.Weapons = new List<ShipWeapon>
            {
                { basicBlaster },
                { secondBlaster },
                { secondBlaster }
            };

        colliders = new Vector2[]                           // This is based off of what was in the PolygonCollider2D in old Player
        {
                new Vector2(0.25f, 0),
                new Vector2(-0.25f, 0.25f),
                new Vector2(-0.125f, 0),
                new Vector2(-0.25f, -0.25f)
        };

        #endregion

        return new ShipConfiguration(weapons, defenses, thrusters, 1, colliders, shipSprite);
    }
}