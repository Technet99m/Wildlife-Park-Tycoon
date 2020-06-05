﻿using Newtonsoft.Json;
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

        public CageSaveData(Cage cage)
        {
            Name = cage.Name;
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
    private void Awake()
    {

    }

    public void LoadGame()
    {
        List<CageSaveData> cages = JsonConvert.DeserializeObject<List<CageSaveData>>(PlayerPrefs.GetString("save"));
        for (int i = 0; i < cages.Count; i++)
        {
            Cage tmp = CageFactory.GetNewCage(cages[i].biome, i, true);
            tmp.Name = cages[i].Name;
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
                a.data.age = animal.data.age;
                a.data.male = animal.data.male;
                a.data.pregnant = animal.data.pregnant;
                a.data.pregnancy = animal.data.pregnancy;
                a.data.sexualActivity = animal.data.sexualActivity;
                a.data.happiness = animal.data.happiness;
                a.data.foods = animal.data.foods;
                a.data.specials = animal.data.specials;
                a.transform.position = tmp.GetFreeTileInGrid();
            }

            if(tmp.animals.Count>0)
            {
                GameManager.Ins.cageIcons.GetChild(i).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Animals/{tmp.animals[0].stats.kind}/CageIcon");
            }
            
        }
    }
    public void SaveGame()
    {
        List<CageSaveData> cages = new List<CageSaveData>();
        foreach (var cage in GameManager.Ins.cages)
            cages.Add(new CageSaveData(cage));
        PlayerPrefs.SetString("save", JsonConvert.SerializeObject(cages));
    }

}