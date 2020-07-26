using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalStatus
{

    private SpriteRenderer mood;
    private Transform body;
    private AnimalData data;
    private AnimalStats stats;
    private List<Need> needs;
    private Transform self;
    
    public AnimalStatus(AnimalData data, AnimalStats stats, List<Need> needs, Transform self, SpriteRenderer mood, Transform body)
    {
        Technet99m.TickingMachine.EveryTick += OnTick;
        this.data = data;
        this.stats = stats;
        this.needs = needs;
        this.self = self;
        this.mood = mood;
        this.body = body;
        OnTick();
    }

    public void Unsubscribe()
    {
        Technet99m.TickingMachine.EveryTick -= OnTick;
    }
    public void RecalculateHappiness()
    {
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
        mood.sprite = Translator.Happiness(data.happiness);
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
            if (!self.parent.GetComponent<Cage>().hasSpace)
            {
                DataManager.AddMoney(stats.price);
                continue;
            }
            Animal child = AnimalFactory.NewAnimalOfKind(stats.kind, self.parent);
            child.transform.position = self.position;
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

        RecalculateHappiness();

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
            self.GetComponent<Animal>().StartCoroutine(Adult());
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
