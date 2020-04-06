using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    public List<Item> feeders;
    public List<Animal> animals;

    public Technet99m.Grid<bool> walkingMap;

    void Start()
    {
        walkingMap = new Technet99m.Grid<bool>(20, 9, 1.37f);
        for (int x = 0; x < walkingMap.Width; x++)
            for (int y = 0; y < walkingMap.Height; y++)
                walkingMap.SetValue(x, y, true);
    }
    public Feeder GetProperFeeder(Food type)
    {
        return feeders.Find((x) => x is Feeder && (x as Feeder).type == type) as Feeder;
    }
    public SpecialItem GetProperSpecial(Special type)
    {
        return feeders.Find((x) => x is SpecialItem && (x as SpecialItem).type == type) as SpecialItem;
    }
    public Animal GetProperMate()
    {
        return animals.Find((x) => !x.data.male && x.data.sexualActivity > 0.5f && x.Followers == 0);
    }

}
