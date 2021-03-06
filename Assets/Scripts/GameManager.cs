﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Technet99m.Singleton<GameManager>
{
    [SerializeField] private LoadManager loadManager;
    [SerializeField] private GameObject loadingScreen;
    
    [SerializeField] private Vector3 offset;
    [SerializeField] private float speed;

    [SerializeField] private Text cageName;
    [SerializeField] private Text cageCapacity;
    [SerializeField] private Text[] cageCosts;

    [SerializeField] private Animator buyToggle;

    public Transform cageIcons;
    public Transform cam;
    public List<Cage> cages;
    public int currentCageIndex;
    public float cellSize;
    public CatalogController catalog;
    public CageMenuController cageMenu;
    public Cage activeCage { get => cages[currentCageIndex]; }
    public int CagePrice { get => cages.Count * 1000; }

    private Coroutine move;
    protected new void Awake()
    {
        base.Awake();
        if (PlayerPrefs.HasKey("save"))
        {
            StateMachine.state = State.Loading;
            Technet99m.Clock.firstDeltaActualized += loadManager.LoadGame;
        }
        else
        {
            loadingScreen.SetActive(false);
            StateMachine.state = State.Game;
            DataManager.Money = 1000;
            BuyNewCage("Forest");
        }
        RefreshUI();
    }
    private void OnEnable()
    {
        Technet99m.TickingMachine.EveryTick += RefreshUI;
    }
    private void Update()
    {
         if(Input.GetKeyDown(KeyCode.Escape))
        {
            loadManager.SaveGame();
            Application.Quit();
        }
    }
    private void OnDisable()
    {
        Technet99m.TickingMachine.EveryTick -= RefreshUI;
    }
   
    private void OnApplicationPause(bool pause)
    {
        if (pause)
            loadManager.SaveGame();
    }
    public void FinishLoading()
    {
        loadingScreen.SetActive(false);
    }
    public void BuyAnimalToggle()
    {
        if (activeCage.animals.Count > 0)
        {
            buyToggle.SetTrigger("toggle");
            buyToggle.SetBool("hide", false);
            buyToggle.GetComponentInChildren<Text>().text = Translator.CurrencyToString(activeCage.animals[0].stats.price);
        }
        else
            cageMenu.SetUp();
    }
    public void RefreshUI()
    {
        if (StateMachine.state == State.Loading)
            return;
        cageCapacity.text = $"{activeCage.animals.Count}/{activeCage.Capacity}";
        cageName.text = activeCage.Name;
        foreach (var text in cageCosts)
            text.text = Translator.CurrencyToString(CagePrice);
    }
    public void BuyNewCage(string biome)
    {
        if (!DataManager.TryAndBuyForMoney(CagePrice))
            return;
        Cage cage = CageFactory.GetNewCage(biome, cages.Count);
        cages.Add(cage);
        ToCage(cages.Count - 1);
        RefreshUI();
    }
    public void ToCage(int index)
    {
        buyToggle.SetBool("hide", true);
        cageIcons.GetChild(currentCageIndex).localScale = Vector3.one * 0.8f;
        currentCageIndex = index;
        cageIcons.GetChild(currentCageIndex).localScale = Vector3.one;
        if (move != null)
            StopCoroutine(move);
        move = StartCoroutine(MoveCameraTo(activeCage.transform.position + offset));
    }
    public void PlusOneK()
    {
        DataManager.AddMoney(1000);
    }
    private IEnumerator MoveCameraTo(Vector3 pos)
    {
        while (Vector3.Distance(cam.position,pos)>0.01f)
        {
            cam.position = Vector3.Lerp(cam.position, pos, Time.deltaTime * speed);
            yield return null;
        }
    }

}
