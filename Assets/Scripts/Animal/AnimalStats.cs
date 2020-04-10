using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Animal Stats", menuName ="AnimalStats", order = 52)]
public class AnimalStats : ScriptableObject
{
    public int TicksToFullMate;
    public int TicksToBorn;
    public int minChildren;
    public int maxChildren;
    public Food[] foods;
    public Special[] specials;
}
