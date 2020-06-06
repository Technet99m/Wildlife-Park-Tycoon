using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    public static event Action<int> moneyChanged;
    public static event Action<int> gemsChanged;

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

    private static int money;
    private static int gems;

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
}
