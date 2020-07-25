﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{
    [System.Serializable]
    private class CageSaveData
    {
        public List<AnimalSaveData> animals;
        public string Name;
        public List<ItemSaveData> items;
        public string biome;
        public int stage;

        public CageSaveData(Cage cage)
        {
            Name = cage.Name;
            stage = cage.stage;
            biome = cage.Biome.Name;
            animals = new List<AnimalSaveData>();
            foreach (var animal in cage.animals)
                animals.Add(new AnimalSaveData(animal));
            items = new List<ItemSaveData>();
            foreach(var item in cage.items)
            {
                items.Add(new ItemSaveData(item));   
            }
        }
        public CageSaveData()
        {

        }
    }
    [System.Serializable]
    private class AnimalSaveData
    {
        public AnimalData data;
        public string kind;

        public AnimalSaveData(Animal animal)
        {
            data = animal.data;
            kind = animal.stats.kind;
        }
        public AnimalSaveData()
        {

        }
    }
    [System.Serializable]
    private class ItemSaveData
    {
        public NeedType type;
        public Special special;
        public Food food;
        public Vector3 localPos;
        public int capacity;

        public ItemSaveData(Item item)
        {
            localPos = item.transform.localPosition;
            if (item as SpecialItem)
            {
                var tmp = item as SpecialItem;
                type = NeedType.Special;
                special = tmp.type;
            }
            else
            {
                var tmp = item as Feeder;
                type = NeedType.Food;
                food = tmp.type;
                capacity = tmp.capacity;
            }
        }
        public ItemSaveData()
        {

        }
    }

    [SerializeField]
    private Feeder[] feeders;
    [SerializeField]
    private SpecialItem[] specials;
    public void LoadGame()
    {
        List<CageSaveData> cages = JsonConvert.DeserializeObject<List<CageSaveData>>(PlayerPrefs.GetString("save"));
        BoostController.LoadBoosts(JsonConvert.DeserializeObject<List<Boost>>(PlayerPrefs.GetString("boosts")));
        DataManager.Money = (PlayerPrefs.GetInt("money", 0));
        for (int i = 0; i < cages.Count; i++)
        {
            Cage tmp = CageFactory.GetNewCage(cages[i].biome, i, true);
            tmp.Name = cages[i].Name;
            tmp.stage = cages[i].stage;
            GameManager.Ins.cages.Add(tmp);
            GameManager.Ins.ToCage(i);
            foreach (var item in cages[i].items)
            {
                if (item.type == NeedType.Food)
                {
                    Feeder f = Instantiate(feeders[(int)item.food], tmp.transform).GetComponent<Feeder>();
                    f.transform.localPosition = item.localPos;
                    f.capacity = item.capacity;
                    f.Place();
                }
                else
                {
                    SpecialItem s = Instantiate(specials[(int)item.special], tmp.transform).GetComponent<SpecialItem>();
                    s.transform.localPosition = item.localPos;
                    s.Place();
                }
            }
            foreach(var animal in cages[i].animals)
            {
                Animal a = AnimalFactory.NewAnimalOfKind(animal.kind, tmp.transform, true);
                a.data.name = animal.data.name;
                a.data.adult = animal.data.adult;
                a.data.age = animal.data.age;
                a.data.male = animal.data.male;
                a.data.pregnant = animal.data.pregnant;
                a.data.pregnancy = animal.data.pregnancy;
                a.data.sexualActivity = animal.data.sexualActivity;
                a.data.happiness = animal.data.happiness;
                a.data.foods = animal.data.foods;
                a.data.specials = animal.data.specials;
                a.transform.position = tmp.GetFreeTileInGrid();
                a.status.RecalculateHappiness();
            }

            if(tmp.animals.Count>0)
            {
                GameManager.Ins.cageIcons.GetChild(i).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Animals/{tmp.animals[0].stats.kind}/CageIcon");
            }
            
        }
        Technet99m.Clock.deltaActualized += SaveGame;
        GameManager.Ins.ToCage(0);
        StartCoroutine(CalculateTimeChanges());
    }
    public void SaveGame()
    {
        PlayerPrefs.SetString("time", (Technet99m.Clock.delta + DateTime.Now.Ticks).ToString());
        List<CageSaveData> cages = new List<CageSaveData>();
        foreach (var cage in GameManager.Ins.cages)
            cages.Add(new CageSaveData(cage));
        PlayerPrefs.SetString("save", JsonConvert.SerializeObject(cages));
        PlayerPrefs.SetString("boosts", JsonConvert.SerializeObject(BoostController.boosts));
        PlayerPrefs.SetInt("money", DataManager.Money);
    }
    
    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private IEnumerator CalculateTimeChanges()
    {
        long old = long.Parse(PlayerPrefs.GetString("time","0"));
        if (old < 1000)
        yield break;
        long now = DateTime.Now.Ticks + Technet99m.Clock.delta;
        int secondsElapsed = (int)((now - old) / 10000000);
        Debug.Log($"Elapsed: {Translator.TicksToTime(secondsElapsed)}");
        foreach(var cage in GameManager.Ins.cages)
        {
            CalculateTimeForCage(cage, secondsElapsed);
            yield return null;
        }

        StateMachine.state = State.Game;
        GameManager.Ins.FinishLoading();
    }
    private void CalculateTimeForCage(Cage cage,int secondsElapsed)
    {
        int delta = 0;
        while (secondsElapsed > 0)
        {
            float generalHappiness = CalculateHappinessForCage(cage);
            int elapsed = CalculateLinearTime(cage, generalHappiness);
            if (elapsed < 0)
                return;
            delta += elapsed;
            UpdateCage(cage, elapsed, generalHappiness);
            if (delta >= 100)
            {
                UpdateFeeders(cage, delta);
                delta = delta % 100;
            }
            secondsElapsed -= elapsed;
        }
    }
    private float CalculateHappinessForCage(Cage cage)
    {
        var stats = cage.animals[0].stats;
        float happiness = 1f;
        foreach (var food in stats.foods)
        {
            if (cage.GetProperFeeder(food) == null)
            {
                happiness -= 0.5f / stats.foods.Length;
                foreach (var animal in cage.animals)
                    animal.data.foods[(int)food] = 0;
            }
        }
        foreach (var spec in stats.specials)
        {
            if(cage.GetProperSpecial(spec)== null)
            {
                happiness -= 0.4f / stats.specials.Length;
                foreach (var animal in cage.animals)
                    animal.data.specials[(int)spec] = 0;
            }
        }
        return happiness;
    }
    private int CalculateLinearTime(Cage cage, float happiness)
    {
        int minTime = -1;
        foreach (var feeder in cage.items.FindAll(i => i is Feeder))
        {
            int time = (feeder as Feeder).capacity / cage.animals.Count * 100;
            if ((time < minTime || minTime < 0) && time > 0)
                minTime = time;
        }
        if (happiness < 0.51)
            return minTime;
        foreach (var animal in cage.animals)
        {
            int toAction = 0;
            if (animal.data.adult)
            {
                if(animal.data.pregnant)
                {
                    toAction = Mathf.FloorToInt((animal.stats.TicksToBorn / ((happiness - 0.5f) * 2f)) * (1 - animal.data.pregnancy));
                }
                else
                {
                    toAction = Mathf.FloorToInt((animal.stats.TicksToFullMate / ((happiness - 0.5f) * 2f)) * (1 - animal.data.sexualActivity));
                }
            }
            else
            {
                toAction = Mathf.FloorToInt((animal.stats.TicksToFullMate / ((happiness - 0.5f) * 2f)) * (1 - animal.data.age));
            }
            if ((toAction < minTime || minTime < 0) && toAction > 0)
                minTime = toAction;
        }
        
        return minTime;
    }
    private void UpdateCage(Cage cage,int elapsed,float happiness)
    {
        if (happiness < 0.5f)
            return;
        foreach (var animal in cage.animals)
        {
            if (animal.data.adult)
            {
                if (animal.data.pregnant)
                {
                    animal.data.pregnancy += (happiness - 0.5f) * 2f / animal.stats.TicksToBorn * elapsed;
                }
                else
                {
                    animal.data.sexualActivity += (happiness - 0.5f) * 2f / animal.stats.TicksToFullMate * elapsed;
                }
            }
            else
            {
                animal.data.age += (happiness - 0.5f) * 2f / animal.stats.TicksToFullMate * elapsed;
            }

        }
        var toBorn = new List<Animal>();
        foreach (var animal in cage.animals)
        {
            if (animal.data.sexualActivity >= 0.999f)
                animal.data.sexualActivity = -1;
        }
        foreach (var animal in cage.animals)
        {
            if (animal.data.sexualActivity < -0.1f && animal.data.male)
            {
                var mate = cage.GetProperMate();
                if (mate == null)
                    continue;
                mate.status.Pregnant();
                mate.data.sexualActivity = 0;
                animal.data.sexualActivity = 0;
            }
            else if (animal.data.pregnancy >= 0.999f)
            {
                toBorn.Add(animal);
            }

        }
        foreach (var animal in toBorn)
            animal.status.Born();
    }
    private void UpdateFeeders(Cage cage,int elapsed)
    {
        foreach (var feeder in cage.items.FindAll(i => i is Feeder))
        {
            (feeder as Feeder).capacity -= cage.animals.Count * (elapsed / 100);
            if ((feeder as Feeder).capacity < 0)
                (feeder as Feeder).capacity = 0;
            (feeder as Feeder).CheckSprite();
        }
    }
}
