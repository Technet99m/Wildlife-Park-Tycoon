﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Animal Stats", menuName ="AnimalStats", order = 52)]
public class AnimalStats : ScriptableObject
{
    public string kind;
    public int price;
    public int TicksToFullMate;
    public int TicksToBorn;
    public int minChildren;
    public int maxChildren;
    public bool isOpen = true;
    public int TicksInLab = 100;
    public int PotionsPerResearch = 100;
    public Food[] foods;
    public Special[] specials;
}
