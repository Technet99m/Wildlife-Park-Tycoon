using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageFactory
{
    private static GameObject CageRef;
    public static Cage GetNewCage(string biome, int cagesNumb)
    {
        if (CageRef == null)
            CageRef = Resources.Load<GameObject>("Cage");
        Cage cage = GameObject.Instantiate(CageRef, cagesNumb * Vector3.right * 43.65f, Quaternion.identity).GetComponent<Cage>();
        cage.Biome = Resources.Load<Biome>("Biomes/" + biome);
        return cage;
    }
}
