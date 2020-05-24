using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Technet99m.Singleton<GameManager>
{
    [SerializeField] private Transform cageIcons;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float speed;

    [SerializeField] private Text cageName;
    [SerializeField] private Text cageCapacity;


    public Transform cam;
    public List<Cage> cages;
    public int currentCageIndex;
    public float cellSize;

    private Coroutine move;
    public Cage activeCage { get => cages[currentCageIndex]; }
    
    protected new void Awake()
    {
        BuyNewCage("Forest");
        base.Awake();
    }
    private void OnEnable()
    {
        Technet99m.TickingMachine.EveryTick += RefreshUI;
    }
    private void OnDisable()
    {
        Technet99m.TickingMachine.EveryTick -= RefreshUI;
    }
    public void RefreshUI()
    {
        cageCapacity.text = $"{activeCage.animals.Count}/{15}";
    }
    public void BuyNewCage(string biome)
    {
        Cage cage = CageFactory.GetNewCage(biome, cages.Count);
        cages.Add(cage);
        cageIcons.GetChild(cages.Count - 1).gameObject.SetActive(true);
        cageIcons.GetChild(cages.Count - 1).GetComponent<Image>().sprite = cage.Biome.icon;
        ToCage(cages.Count - 1);
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
