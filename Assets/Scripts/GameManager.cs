using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Technet99m.Singleton<GameManager>
{
    [SerializeField] private LoadManager loadManager;
    
    [SerializeField] private Vector3 offset;
    [SerializeField] private float speed;

    [SerializeField] private Text cageName;
    [SerializeField] private Text cageCapacity;
    [SerializeField] private Text[] cageCosts;

    public Transform cageIcons;
    public Transform cam;
    public List<Cage> cages;
    public int currentCageIndex;
    public float cellSize;
    public CatalogController catalog;
    public Cage activeCage { get => cages[currentCageIndex]; }
    public int CagePrice { get => cages.Count * 1000; }
    private Coroutine move;
    protected new void Awake()
    {
        base.Awake();
        if (PlayerPrefs.HasKey("save"))
        {
            loadManager.LoadGame();
            ToCage(0);
        }
        else
        {
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
    public void RefreshUI()
    {
        cageCapacity.text = $"{activeCage.animals.Count}/{15}";
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
