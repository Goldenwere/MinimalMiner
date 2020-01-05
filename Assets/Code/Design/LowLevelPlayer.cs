using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MinimalMiner.Entity;

public class LowLevelPlayer : MonoBehaviour
{
    public ShipConfiguration Config { get; set; }
    public Sprite Sprite
    {
        get { return renderer.sprite; }
        set { renderer.sprite = value; }
    }

    [SerializeField] private SpriteRenderer renderer;
    private float fireTimer;
}
