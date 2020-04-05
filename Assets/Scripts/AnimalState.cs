﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalState : MonoBehaviour
{
    AnimalData data;
    AnimalStats stats;
    List<Need> needs;
    void Start()
    {
        Technet99m.TickingMachine.EveryTick += OnTick;
        data = GetComponent<AnimalDataHolder>().data;
        stats = GetComponent<AnimalDataHolder>().stats;
        needs = GetComponent<Animal>().needs;
    }
    void OnTick()
    {
        foreach(var food in stats.foods)
        {
            data.foods[(int)food] -= 0.02f;
            if (data.foods[(int)food] < 0.5f && needs.Find((x) => x.type == NeedType.Food && x.food == food) == null)
                needs.Add(new Need() { type = NeedType.Food, food = food });
        }
        foreach (var spec in stats.specials)
        {
            data.specials[(int)spec] -= 0.02f;
            if (data.specials[(int)spec] < 0.5f && needs.Find((x) => x.type == NeedType.Special && x.special == spec) == null)
                needs.Add(new Need() { type = NeedType.Special, special = spec });
        }
        data.happiness = 1;
        foreach(var need in needs)
            switch(need.type)
            {
                case NeedType.Food:
                    data.happiness -= 0.5f/stats.foods.Length;
                    break;
                case NeedType.Special:
                    data.happiness -= 0.4f / stats.specials.Length;
                    break;
                case NeedType.Sex:
                    data.happiness -= 0.1f;
                    break;
            }
        data.sexualActivity += (data.happiness - 0.5f) * 2f / stats.TicksToFullMate;
        if (data.sexualActivity > 0.5 && needs.Find((x) => x.type == NeedType.Sex) == null)
            needs.Add(new Need() { type = NeedType.Sex });
    }
    public void Done(Need need)
    {
        needs.Remove(need);
        switch(need.type)
        {
            case NeedType.Food:
                data.foods[(int)need.food] = 1;
                break;
            case NeedType.Special:
                data.specials[(int)need.special] = 1;
                break;
            case NeedType.Sex:
                data.sexualActivity = 0;
                break;
        }
    }
}