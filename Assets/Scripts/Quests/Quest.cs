using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public PrizeType prizeType;
    public string name;
    public string description;
    public object prizeAmount;

    public string targetVariable;
    public int targetValue;
    public Func<int> condition;

    public  void Check()
    { 
        if (condition?.Invoke() == targetValue)
            Complete();
    }
    public void Complete()
    {
        switch(prizeType)
        {
            case PrizeType.Money:
                DataManager.AddMoney((int)prizeAmount);
                break;
            case PrizeType.Gems:
                DataManager.AddGems((int)prizeAmount);
                break;
            case PrizeType.Achievement:

                break;
        }
    }
}
public enum PrizeType
{
    Achievement,
    Money,
    Gems
}
