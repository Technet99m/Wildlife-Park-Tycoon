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

    public Transform cageIcons;
    public Transform cam;
    public List<Cage> cages;
    public int currentCageIndex;
    public float cellSize;
    public CatalogController catalog;
    public Cage activeCage { get => cages[currentCageIndex]; }


    private Coroutine move;
    protected new void Awake()
    {
        base.Awake();
        //PlayerPrefs.DeleteAll();
        if (PlayerPrefs.HasKey("save"))
        {
            loadManager.LoadGame();
            ToCage(0);
        }
        else
        {
            BuyNewCage("Forest");
        }
        RefreshUI();
    }
    private void OnEnable()
    {
        Technet99m.TickingMachine.EveryTick += RefreshUI;
    }
    private void OnDisable()
    {
        Technet99m.TickingMachine.EveryTick -= RefreshUI;
    }
    private void OnApplicationQuit()
    {
        loadManager.SaveGame();
    }
    public void RefreshUI()
    {
        cageCapacity.text = $"{activeCage.animals.Count}/{15}";
        cageName.text = activeCage.Name;
    }
    public void BuyNewCage(string biome)
    {
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
    private IEnumerator MoveCameraTo(Vector3 pos)
    {
        while (Vector3.Distance(cam.position,pos)>0.01f)
        {
            cam.position = Vector3.Lerp(cam.position, pos, Time.deltaTime * speed);
            yield return null;
        }
    }

}
