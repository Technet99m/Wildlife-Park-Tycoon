using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Animal", menuName = "Animal Sprites Pack", order = 51)]
public class AnimalSprites : ScriptableObject
{
    public AnimalPack male;
    public AnimalPack female;
}
[System.Serializable]
public class AnimalPack
{
    public DirectionPack down;
    public DirectionPack up;
    public DirectionPack side;
}
[System.Serializable]
public class DirectionPack
{
    public Sprite body;
    public Sprite face;
    public Sprite eyes;
    public Sprite right_leg;
    public Sprite left_leg;
    public Sprite right_hand;
    public Sprite left_hand;
    public Sprite tail;
}
