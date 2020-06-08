using System.Collections;
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
    private void Start()
    {
        data = GetComponent<AnimalDataHolder>().data;
        stats = GetComponent<AnimalDataHolder>().stats;
        needs = GetComponent<Animal>().needs;

        if (data.age < 1)
            body.localScale = new Vector3(0.7f, 0.7f, 1);
    }
    private void OnDisable()
    {
        Technet99m.TickingMachine.EveryTick -= OnTick;
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
                if (!data.male) Pregnant();
                break;
        }
    }

    private void Pregnant()
    {
        data.pregnant = true;
    }
    private void OnTick()
    {
        foreach (var food in stats.foods)
        {
            data.foods[(int)food] -= 0.01f;
            if (data.foods[(int)food] < 0 && needs.Find((x) => x.type == NeedType.Food && x.food == food) == null)
                needs.Add(new Need() { type = NeedType.Food, food = food });
        }
        foreach (var spec in stats.specials)
        {
            data.specials[(int)spec] -= 0.01f;
            if (data.specials[(int)spec] < 0 && needs.Find((x) => x.type == NeedType.Special && x.special == spec) == null)
                needs.Add(new Need() { type = NeedType.Special, special = spec });
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
            if (data.age > 1 && !data.pregnant)
                data.sexualActivity += (data.happiness - 0.5f) * 2f / stats.TicksToFullMate;
            else if(!data.pregnant)
                data.age += (data.happiness - 0.5f) * 2f / stats.TicksToFullMate;
            else
                data.pregnancy += (data.happiness - 0.5f) * 2f / stats.TicksToBorn;
        }
        if (data.pregnant && data.pregnancy > 1)
            Born();
        if (data.age > 1 && !data.adult)
            StartCoroutine(Adult());
        if (data.sexualActivity > 1 && needs.Find((x) => x.type == NeedType.Sex) == null)
            needs.Add(new Need() { type = NeedType.Sex });
        mood.sprite = Translator.Happiness(data.happiness);
    }
    private void Born()
    {
        for (int i = 0, tmp = Random.Range(stats.minChildren, stats.maxChildren + 1); i < tmp; i++)
        {
            if (transform.parent.GetComponent<Cage>().animals.Count == 15)
            {
                DataManager.AddMoney(stats.price);
                break;
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
    private IEnumerator Adult()
    {
        data.adult = true;
        while (body.localScale != Vector3.one)
        {
            body.localScale = Vector3.MoveTowards(body.localScale, Vector3.one, Time.deltaTime);
            yield return null;
        }
    }
    
}
