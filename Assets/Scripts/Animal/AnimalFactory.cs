using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalFactory 
{
    private static GameObject AnimalRef;
    public static Animal NewAnimalOfKind(string kind, Transform parent)
    {
        if (AnimalRef == null)
            AnimalRef = Resources.Load<GameObject>("Animal");
        Animal animal = GameObject.Instantiate(AnimalRef, parent).GetComponent<Animal>();
        AnimalStats tmp = Resources.Load<AnimalStats>($"Animals/{kind}/Stats");
        animal.GetComponent<AnimalDataHolder>().stats = tmp;
        animal.GetComponent<AnimalAnimationController>().sprites = Resources.Load<AnimalSprites>($"Animals/{kind}/Sprites");
        return animal;
    }
}
