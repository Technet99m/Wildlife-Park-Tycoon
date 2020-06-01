using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimalDataHolder : MonoBehaviour
{
    public AnimalData data;
    public AnimalStats stats;
}
[System.Serializable]
public class AnimalData
{
    public string name;
    public bool male;
    public bool pregnant;
    public float pregnancy;
    public float sexualActivity;
    public float age;
    public float happiness;
    public float[] foods;
    public float[] specials;
}
