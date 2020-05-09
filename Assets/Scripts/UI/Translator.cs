using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translator : Technet99m.Singleton<Translator>
{
    public Sprite male, female;
    public Color badC, normalC, goodC;
    public Sprite bad, normal, good;
    public static Sprite Sex(bool isMale)
    {
        return isMale ? Ins.male : Ins.female;
    }
    public static Sprite Happiness(float happiness)
    {
        if (happiness > 0.74f)  return Ins.good;
        if (happiness > 0.49f)   return Ins.normal;
        return Ins.bad;
    }
    public static Color HappinessColor(float happiness)
    {
        if (happiness > 0.74f) return Ins.goodC;
        if (happiness > 0.49f) return Ins.normalC;
        return Ins.badC;
    }
    public static string Needs2Text(List<Need> needs)
    {
        var text = "";
        for(int i = 0;i<needs.Count;i++)
        {
            switch (needs[i].type)
            {
                case NeedType.Food:
                    switch(needs[i].food)
                    {
                        case Food.water:
                            text += "I want to drink\n";
                            break;
                        case Food.veggie:
                            text += "I want some veggies\n";
                            break;
                        case Food.meat:
                            text += "I want fresh meat\n";
                            break;
                        case Food.grass:
                            text += "I want some grass\n";
                            break;
                        case Food.fruit:
                            text += "I want fruits\n";
                            break;
                        case Food.fish:
                            text += "I want some fish\n";
                            break;
                    }
                    break;
                case NeedType.Special:
                    switch (needs[i].special)
                    {
                        case Special.jump:
                            text += "I need to jump somewhere\n";
                            break;
                        case Special.run:
                            text += "Where I can run?\n";
                            break;
                        case Special.hike:
                            text += "Where I can run?\n";
                            break;
                    }
                    break;
                case NeedType.Sex:
                    text += "I want to mate someone\n";
                    break;

            }

        }
        return text.TrimEnd();
    }
}
