using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    public List<Item> feeders;
    public List<Animal> animals;
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
