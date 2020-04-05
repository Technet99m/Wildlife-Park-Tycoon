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
    public bool male;
    public float sexualActivity;
    public float happiness;
    public float[] foods;
    public float[] specials;
}
