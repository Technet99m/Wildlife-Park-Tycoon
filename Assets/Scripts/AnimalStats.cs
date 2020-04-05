using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Animal Stats", menuName ="AnimalStats", order = 52)]
public class AnimalStats : ScriptableObject
{
    public float TicksToFullMate;
    public float TicksToBorn;
    public Food[] foods;
    public Special[] specials;
}
