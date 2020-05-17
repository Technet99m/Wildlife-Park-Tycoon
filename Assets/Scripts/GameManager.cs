using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Technet99m.Singleton<GameManager>
{
    [SerializeField] private Transform cageIcons;
    [SerializeField] private Vector3 offset;
    [SerializeField] private GameObject CageRef;
    [SerializeField] private Sprite[] CageIconsSprites;
    [SerializeField] private Sprite[] CageFences;
    [SerializeField] private Sprite[] CageBackgrounds;
    [SerializeField] private float speed;

    public Transform cam;
    public List<Cage> cages;
    public int currentCageIndex;
    public float cellSize;

    private Coroutine move;
    public Cage activeCage { get => cages[currentCageIndex]; }
    

    public void BuyNewCage(int biome)
    {
        Cage cage = Instantiate(CageRef, cages[cages.Count - 1].transform.position + Vector3.right * 44f, Quaternion.identity).GetComponent<Cage>();
        cages.Add(cage);
        cageIcons.GetChild(cages.Count - 1).gameObject.SetActive(true);
        cage.background.sprite = CageBackgrounds[biome];
        cage.fence.sprite = CageFences[biome];
        cageIcons.GetChild(cages.Count - 1).GetComponent<Image>().sprite = CageIconsSprites[biome];
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
