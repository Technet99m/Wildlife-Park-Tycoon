﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CageMenuController : MonoBehaviour
{
    [SerializeField] private InputField cageName;
    [SerializeField] private Text cageCapacity;
    [SerializeField] private CageMenuItemController[] items;
    [SerializeField] private GameObject itemRef;
    [SerializeField] private Transform cageIcons;

    [SerializeField] private GameObject buySegment;
    [SerializeField] private GameObject sellSegment;
    [SerializeField] private Transform sellContent;
    private Cage activeCage;
    private void OnEnable()
    {
        Technet99m.TickingMachine.EveryTick += Refresh;
    }
    private void OnDisable()
    {
        Technet99m.TickingMachine.EveryTick -= Refresh;
    }
    public void SetUp()
    {
        activeCage = GameManager.Ins.activeCage;
        cageName.text = activeCage.Name;
        cageCapacity.text = $"{activeCage.animals.Count}/{15}";
        gameObject.SetActive(true);
        HideAll();
        if (activeCage.animals.Count == 0)
        {
            sellSegment.SetActive(false);
            buySegment.SetActive(true);
            SetUpBuy();
            return;
        }
        sellSegment.SetActive(true);
        buySegment.SetActive(false);
        for (int i = 0; i < activeCage.animals.Count; i++)
        {
            items[i].SetUp(activeCage.animals[i]);
        }
    }
    public void Refresh()
    {
        if (activeCage.animals.Count == 0)
        {
            sellSegment.SetActive(false);
            buySegment.SetActive(true);
            SetUpBuy();
            return;
        }
        sellSegment.SetActive(true);
        buySegment.SetActive(false);
        cageCapacity.text = $"{activeCage.animals.Count}/{15}";
        HideAll();
        for(int i = 0;i< activeCage.animals.Count; i++)
        {
            items[i].Refresh(activeCage.animals[i]);
        }
    }
    public void Sell()
    {
        List<Animal> listToSell = new List<Animal>();
        for (int i = 0; i < activeCage.animals.Count; i++)
            if (items[i].Selected)
                listToSell.Add(activeCage.animals[i]);
        foreach (var animal in listToSell)
        {
            animal.Sell();
        }
        Refresh();
        foreach (var item in items)
            item.Selected = false;
    }
    public void SelectAll()
    {
        foreach (var item in items)
            if (item.gameObject.activeSelf)
                item.Selected = true;

    }
    public void SelectChildren()
    {
        for (int i = 0; i < activeCage.animals.Count; i++)
            ;
    }
    public void BuyNewKind(string kind)
    {
        Animal animal = AnimalFactory.NewAnimalOfKind(kind, activeCage.transform);
        animal.data.male = false;
        animal.transform.position = activeCage.GetFreeTileInGrid();
        cageIcons.GetChild(GameManager.Ins.currentCageIndex).GetComponent<Image>().sprite = Resources.Load<Sprite>($"Animals/{kind}/CageIcon");
        Refresh();
    }
    public void SetNewName(string newName)
    {
        activeCage.Name = newName;
    }
    private void HideAll()
    {
        foreach (var item in items)
            item.gameObject.SetActive(false);
    }
    private void SetUpBuy()
    {
        for(int i = 0;i< sellContent.childCount;i++)
        {
            sellContent.GetChild(i).gameObject.SetActive(false);
        }
        for(int i = 0;i<GameManager.Ins.activeCage.Biome.kinds.Length;i++)
        {
            sellContent.GetChild(i).GetComponent<BuyKindController>().SetUp(GameManager.Ins.activeCage.Biome.kinds[i]);
        }
    }
}
