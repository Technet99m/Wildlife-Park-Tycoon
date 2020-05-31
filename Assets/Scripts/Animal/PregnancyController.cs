using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PregnancyController : MonoBehaviour
{
    [SerializeField] AnimalDataHolder data;

    public int ticksToBorn;
    void Start()
    {
        Technet99m.TickingMachine.EveryTick += OnTick;
    }
    void OnTick()
    {
        ticksToBorn--;
        if (ticksToBorn == 0)
            Born();
    }
    void Born()
    {
        for (int i = 0, tmp = Random.Range(data.stats.minChildren, data.stats.maxChildren + 1); i < tmp; i++)
        {
            if(transform.parent.GetComponent<Cage>().animals.Count==15)
            {
                DataManager.AddMoney(data.stats.price);
                break;
            }
            AnimalDataHolder child = AnimalFactory.NewAnimalOfKind(data.stats.kind, transform.parent).GetComponent<AnimalDataHolder>();
            child.transform.position = transform.position;
            child.GetComponent<AnimalAnimationController>().sprites = GetComponent<AnimalAnimationController>().sprites;
            child.stats = data.stats;
            child.data.male = Random.value > 0.5f;
            child.data.age = 0;
            for (int j = 0; j < child.data.foods.Length; j++)
                child.data.foods[j] = 0.5f;
            for (int j = 0; j < child.data.specials.Length; j++)
                child.data.specials[j] = 1f;
        }
        GetComponent<AnimalStatus>().pregnant = false;
    }
}
