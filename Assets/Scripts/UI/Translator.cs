﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translator : Technet99m.Singleton<Translator>
{
    public Sprite male, female;
    public Color badC, normalC, goodC;
    public Sprite bad, normal, good;
    public Sprite[] Feromones, SpecialFood, Vitamins;

    public static string TicksToTime(int ticks)
    {
        int days = ticks / 86400;
        int hours = (ticks % 86400) / 3600;
        int minutes = (ticks % 86400 % 3600) / 60;
        int seconds = (ticks % 86400 % 3600 % 60);

        if (days > 0)
            return $"{days}d {hours}h";
        else if (hours > 0)
            return $"{hours}h {minutes}m";
        else if (minutes > 0)
            return $"{minutes}m {seconds}s";
        else
            return $"{ticks}s";
    }   
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
    public static string CurrencyToString(int amount)
    {
        if (amount < 1000)
            return amount.ToString();
        if (amount < 1000000)
        {
            var thousands = amount / 1000;
            if (amount % 1000 == 0)
                return $"{thousands}K";
            else
                return $"{thousands},{(amount % 1000).ToString("D3")}";
        }
        if (amount % 1000000 == 0)
            return $"{amount / 1000000}M";
        else
        {
            var thousands = amount%1000000 / 1000;
                return $"{amount/1000000}M {thousands.ToString("D3")}K";
        }
    }
    public static int KindToPrice(string kind)
    {
        return Resources.Load<AnimalStats>("Animals/" + kind + "/Stats").price;
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
                        case Special.swim:
                            text += "Cam you get me a pool?\n";
                            break;
                        case Special.mud:
                            text += "I'm too clean to be happy\n";
                            break;
                        case Special.scratch:
                            text += "My back is itchy\n";
                            break;
                        case Special.sleep:
                            text += "I'm tired of this\n";
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
    public static Sprite Boost(BoostType type, int stage)
    {
        switch (type)
        {
            case BoostType.feromons:
                return Ins.Feromones[stage];
            case BoostType.specialFood:
                return Ins.SpecialFood[stage];
            case BoostType.vitamins:
                return Ins.Vitamins[stage];
            default:
                return null;
        }
    }
}
