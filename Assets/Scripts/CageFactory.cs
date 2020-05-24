using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageFactory
{
    private static GameObject CageRef;
    private static string nameKey;
    public static Cage GetNewCage(string biome, int cagesNumb)
    {
        if (CageRef == null)
            CageRef = Resources.Load<GameObject>("Cage");
        Cage cage = GameObject.Instantiate(CageRef, cagesNumb * Vector3.right * 43.65f, Quaternion.identity).GetComponent<Cage>();
        cage.Biome = Resources.Load<Biome>("Biomes/" + biome);
        PlayerPrefs.SetInt(nameKey + biome, PlayerPrefs.GetInt(nameKey + biome, 0) + 1);
        cage.Name = biome + " #" + PlayerPrefs.GetInt(nameKey + biome);
        return cage;
    }
}
