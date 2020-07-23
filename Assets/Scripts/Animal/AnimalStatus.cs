﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalStatus : MonoBehaviour
{
    [SerializeField] SpriteRenderer mood;
    [SerializeField] Transform body;


    private AnimalData data;
    private AnimalStats stats;
    private List<Need> needs;

    private void OnEnable()
    {
        Technet99m.TickingMachine.EveryTick += OnTick;
    }
    
    private void OnDisable()
    {
        Technet99m.TickingMachine.EveryTick -= OnTick;
    }
    public void Initialize()
    {
        data = GetComponent<AnimalDataHolder>().data;
        stats = GetComponent<AnimalDataHolder>().stats;
        needs = GetComponent<Animal>().needs;

        OnTick();
    }
    public void Pregnant()
    {
        data.pregnant = true;
    }
    public void Born()
    {
        Boost special = BoostController.boosts.Find(x => x.type == BoostType.specialFood);
        int tmp = Random.Range(stats.minChildren, stats.maxChildren + (special != null ? 2 + special.power : 1));
        for (int i = 0; i < tmp; i++)
        {
            if (!transform.parent.GetComponent<Cage>().hasSpace)
            {
                DataManager.AddMoney(stats.price);
                continue;
            }
            AnimalDataHolder child = AnimalFactory.NewAnimalOfKind(stats.kind, transform.parent).GetComponent<AnimalDataHolder>();
            child.transform.position = transform.position;
            child.data.male = Random.value > 0.5f;
            child.data.age = 0;
            child.data.adult = false;
            for (int j = 0; j < child.data.foods.Length; j++)
                child.data.foods[j] = 0.5f;
            for (int j = 0; j < child.data.specials.Length; j++)
                child.data.specials[j] = 1f;
        }
        data.pregnant = false;
        data.pregnancy = 0;
    }
    public void Done(Need need)
    {
        needs.Remove(need);
        switch (need.type)
        {
            case NeedType.Food:
                data.foods[(int)need.food] = 1;
                break;
            case NeedType.Special:
                data.specials[(int)need.special] = 1;
                break;
            case NeedType.Sex:
                data.sexualActivity = 0;
                if (!data.male) Pregnant();
                break;
        }
    }

    private void OnTick()
    {
        if (StateMachine.state == State.Game)
            RecalculateStatus();
    }
    private void RecalculateStatus()
    {
        foreach (var food in stats.foods)
        {
            if (needs.Exists(x => x.type == NeedType.Food && x.food == food))
                continue;
            data.foods[(int)food] -= 0.01f;
            if (data.foods[(int)food] < 0)
            {
                needs.Add(new Need() { type = NeedType.Food, food = food });
            }
        }
        foreach (var spec in stats.specials)
        {
            if (needs.Exists(x => x.type == NeedType.Special && x.special == spec))
                continue;
            data.specials[(int)spec] -= 0.01f;
            if (data.specials[(int)spec] < 0)
            {
                needs.Add(new Need() { type = NeedType.Special, special = spec });
            }
        }
        data.happiness = 1;
        foreach (var need in needs)
            switch (need.type)
            {
                case NeedType.Food:
                    data.happiness -= 0.5f / stats.foods.Length;
                    break;
                case NeedType.Special:
                    data.happiness -= 0.4f / stats.specials.Length;
                    break;
                case NeedType.Sex:
                    data.happiness -= 0.1f;
                    break;
            }
        if (data.happiness >= 0.51)
        {
            Boost feromones = BoostController.boosts.Find(x => x.type == BoostType.feromons);
            Boost vitamins = BoostController.boosts.Find(x => x.type == BoostType.vitamins);
            if (data.age > 1 && !data.pregnant && data.sexualActivity > -0.1f)
                data.sexualActivity += (data.happiness - 0.5f) * 2f / stats.TicksToFullMate * (feromones != null ? 2f * feromones.power : 1f);
            else if (!data.pregnant)
                data.age += (data.happiness - 0.5f) * 2f / stats.TicksToFullMate * (vitamins != null ? 2f * vitamins.power : 1f);
            else
                data.pregnancy += (data.happiness - 0.5f) * 2f / stats.TicksToBorn;
        }
        if (data.pregnant && data.pregnancy > 1)
            Born();
        if (data.age > 1 && !data.adult)
        {
            data.adult = true;
            StartCoroutine(Adult());
        }
        if (data.sexualActivity > 1)
        {
            needs.Add(new Need() { type = NeedType.Sex });
            data.sexualActivity = -1;
        }
        mood.sprite = Translator.Happiness(data.happiness);
    }

    private void RecalculateStatusLoading()
    {
        foreach (var food in stats.foods)
        {
            if (data.foods[(int)food] < -1f)
                continue;
            data.foods[(int)food] -= 0.05f;
            if (data.foods[(int)food] < 0)
            {
                needs.Add(new Need() { type = NeedType.Food, food = food });
                data.foods[(int)food] = -1.1f;
            }
        }
        foreach (var spec in stats.specials)
        {
            if (data.specials[(int)spec] < -1f)
                continue;
            data.specials[(int)spec] -= 0.05f;
            if (data.specials[(int)spec] < 0)
            {
                needs.Add(new Need() { type = NeedType.Special, special = spec });
                data.specials[(int)spec] = -1.1f;
            }
        }
        data.happiness = 1;
        foreach (var need in needs)
            switch (need.type)
            {
                case NeedType.Food:
                    data.happiness -= 0.5f / stats.foods.Length;
                    break;
                case NeedType.Special:
                    data.happiness -= 0.4f / stats.specials.Length;
                    break;
                case NeedType.Sex:
                    data.happiness -= 0.1f;
                    break;
            }
        if (data.happiness >= 0.51)
        {
            Boost feromones = BoostController.boosts.Find(x => x.type == BoostType.feromons);
            Boost vitamins = BoostController.boosts.Find(x => x.type == BoostType.vitamins);
            if (data.age > 1 && !data.pregnant && data.sexualActivity > -0.1f)
                data.sexualActivity += ((data.happiness - 0.5f) * 2f / stats.TicksToFullMate) * 5 * (feromones!=null? 2f*feromones.power : 1f);
            else if (!data.pregnant)
                data.age += ((data.happiness - 0.5f) * 2f / stats.TicksToFullMate) * 5 * (vitamins!=null ? 2f * vitamins.power : 1f);
            else
                data.pregnancy += ((data.happiness - 0.5f) * 2f / stats.TicksToBorn) * 5;
        }
        if (data.pregnant && data.pregnancy > 1)
            Born();
        if (data.age > 1 && !data.adult)
        {
            data.adult = true;
            body.localScale=Vector3.one;
        }
        if (data.sexualActivity > 1)
        {
            needs.Add(new Need() { type = NeedType.Sex });
            data.sexualActivity = -1;
        }
    }
    
    private IEnumerator Adult()
    {
        while (body.localScale != Vector3.one)
        {
            body.localScale = Vector3.MoveTowards(body.localScale, Vector3.one, Time.deltaTime);
            yield return null;
        }
    }
    
}
