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
}
