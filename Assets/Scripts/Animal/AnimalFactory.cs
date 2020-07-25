using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalFactory 
{
    private static GameObject AnimalRef;
    private static string nameKey;
    public static Animal NewAnimalOfKind(string kind, Transform parent, bool IgnoreName = false)
    {
        if (AnimalRef == null)
            AnimalRef = Resources.Load<GameObject>("Animal");
        Animal animal = Object.Instantiate(AnimalRef, parent).GetComponent<Animal>();
        AnimalStats tmp = Resources.Load<AnimalStats>($"Animals/{kind}/Stats");
        animal.Initialize(tmp);
        animal.GetComponent<AnimalAnimationController>().sprites = Resources.Load<AnimalSprites>($"Animals/{kind}/Sprites");
        if (IgnoreName)
            return animal;
        PlayerPrefs.SetInt(nameKey + kind, PlayerPrefs.GetInt(nameKey + kind, 0) + 1);
        animal.data.name = kind+" #"+PlayerPrefs.GetInt(nameKey + kind);
        return animal;
    }
}
