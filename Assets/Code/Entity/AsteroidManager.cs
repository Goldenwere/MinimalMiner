using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    private List<GameObject> asteroids;

    public void OnAsteroidDestruction(GameObject asteroid, List<GameObject> newAsteroids)
    {
        asteroids.Remove(asteroid);
        Destroy(asteroid);
        foreach (GameObject g in newAsteroids)
        {
            asteroids.Add(g);
        }
    }
}
