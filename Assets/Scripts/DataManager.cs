using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    public static event Action<int> moneyChanged;
    public static event Action<int> gemsChanged;
    /// <summary>
    /// <index, amount>
    /// </summary>
    public static event Action<int,int> potionsChanged;

    public static int Money { get => money; set
        {
            money = value;
            moneyChanged?.Invoke(value);
        } }
    public static int Gems
    {
        get => gems; set
        {
            gems = value;
            gemsChanged?.Invoke(value);
        }
    }
    public static int ForestPotion
    {
        get => potions[0]; set
        {
            potions[0] = value;
            potionsChanged?.Invoke(0, value);
        }
    }
    public static int SavannahPotion
    {
        get => potions[1]; set
        {
            potions[1] = value;
            potionsChanged?.Invoke(1, value);
        }
    }
    public static int ArcticPotion
    {
        get => potions[2]; set
        {
            potions[2] = value;
            potionsChanged?.Invoke(3, value);
        }
    }
    public static int JunglePotion
    {
        get => potions[3]; set
        {
            potions[3] = value;
            potionsChanged?.Invoke(3,value);
        }
    }

    private static int money;
    private static int gems;
    private static int[] potions = new int[4];

    public static bool TryAndBuyForMoney(int price)
    {
        if (price > Money)
            return false;
        Money -= price;
        return true;
    }
    public static bool TryAndBuyForGems(int price)
    {
        if (price > Gems)
            return false;
        Gems -= price;
        return true;
    }
    public static void AddMoney(int amount)
    {
        Money += amount;
    }
    public static void AddGems(int amount)
    {
        Gems += amount;
    }
    public static void AddPotions(string biome, int amount)
    {
        switch(biome)
        {
            case "Forest":
                ForestPotion += amount;
                break;
            case "Jungle":
                JunglePotion += amount;
                break;
            case "Savannah":
                SavannahPotion += amount;
                break;
            case "Arctic":
                ArcticPotion += amount;
                break;
        }
    }
}
