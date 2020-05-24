using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cage : MonoBehaviour
{
    [SerializeField] private Sprite EmptyTile, FilledTile;
    [SerializeField] private Image background;
    [SerializeField] private Image fence;

    public List<Item> feeders;
    public List<Animal> animals;
    
    public string KindInCage { get => animals[0].GetComponent<AnimalDataHolder>().stats.kind; }
    public Biome Biome { 
        get => biome; 
        set 
        { 
            biome = value;
            background.sprite = biome.background;
            fence.sprite = biome.fence;
        }
    } 

    public Technet99m.Grid<bool> walkingMap;

    public Technet99m.Grid<bool> placingMap;

    private SpriteRenderer[] gridTiles;
    private Biome biome;

    private void Start()
    {
        GridInit();
    }
    private void GridInit()
    {
        walkingMap = new Technet99m.Grid<bool>(20, 9, GameManager.Ins.cellSize);
        placingMap = new Technet99m.Grid<bool>(20, 9, GameManager.Ins.cellSize);
        InitGridTiles();
    }
    private void InitGridTiles()
    {
        gridTiles = new SpriteRenderer[walkingMap.Height * walkingMap.Width];
        for (int x = 0, i = 0; x < walkingMap.Width; x++)
            for (int y = 0; y < walkingMap.Height; y++, i++)
            {
                placingMap.SetValue(x, y, true);
                walkingMap.SetValue(x, y, true);
                /*var go = new GameObject("gridTile", typeof(SpriteRenderer));
                gridTiles[i] = go.GetComponent<SpriteRenderer>();
                gridTiles[i].sortingOrder = -2;
                go.transform.parent = transform;
                go.SetActive(false);
                go.transform.position = placingMap.GetWorldPos(x, y, transform.position);
                go.transform.localScale = Vector3.one * placingMap.CellSize;*/
            }
    }
    public Feeder GetProperFeeder(Food type)
    {
        return feeders.Find((x) => x is Feeder && (x as Feeder).type == type) as Feeder;
    }
    public SpecialItem GetProperSpecial(Special type)
    {
        return feeders.Find((x) => x is SpecialItem && (x as SpecialItem).type == type) as SpecialItem;
    }
    public Animal GetProperMate()
    {
        return animals.Find((x) => !x.data.male && x.data.sexualActivity > 0.5f && x.Followers == 0);
    }
    public void ShowGrid()
    {
        for (int x = 0, i = 0; x < placingMap.Width; x++)
            for (int y = 0; y < placingMap.Height; y++, i++)
            {
                gridTiles[i].sprite = placingMap.GetUnitAt(x, y) ? EmptyTile : FilledTile;
                gridTiles[i].gameObject.SetActive(true);
            }
    }
    public void HideGrid()
    {
        for (int x = 0, i = 0; x < placingMap.Width; x++)
            for (int y = 0; y < placingMap.Height; y++, i++)
            {
                gridTiles[i].gameObject.SetActive(false);
            }
    }
    public Vector3 RoundToGridCell(Vector3 pos)
    {
        walkingMap.GetXY(pos, transform.position, out int x, out int y);
        return walkingMap.GetWorldPos(x, y, transform.position);
    }
    public bool CanPlace(Vector2Int size,Vector2 pos)
    {
        bool canPlace = true;
        Vector2 offset = (size - Vector2.one) * -0.5f;
         placingMap.GetXY(pos + offset - Vector2.up * 0.1f, transform.position, out int XS, out int YS);
        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
                if (!placingMap.GetUnitAt(XS + x, YS + y))
                    return false;
        return canPlace;
    }
    public void Place(Vector2Int placedSize,Vector2Int walkingSize, Vector2 pos)
    {
        Vector2 offset = (placedSize - Vector2.one) * -0.5f;
        placingMap.GetXY(pos + offset - Vector2.up * 0.1f, transform.position, out int XS, out int YS);
        for (int x = 0; x < placedSize.x; x++)
            for (int y = 0; y < placedSize.y; y++)
                placingMap.SetValue(XS + x, YS + y, false);
        offset = (walkingSize - Vector2.one) * -0.5f;
        walkingMap.GetXY(pos + offset, transform.position, out  XS, out  YS);
        for (int x = 0; x < walkingSize.x; x++)
            for (int y = 0; y < walkingSize.y; y++)
                walkingMap.SetValue(XS + x, YS + y, false);
    }
    public Vector2 GetFreeTileInGrid()
    {
        int x, y;
        do
        {
            x = Random.Range(0, walkingMap.Width);
            y = Random.Range(0, walkingMap.Height);
        }
        while (!walkingMap.GetUnitAt(x,y));
        return walkingMap.GetWorldPos(x, y, transform.position);
        
    }
    public Vector2 GetPlaceToMate()
    {
        int x, y;
        do
        {
            x = Random.Range(0, walkingMap.Width);
            y = Random.Range(0, walkingMap.Height);
        }
        while (!walkingMap.GetUnitAt(x, y) || !walkingMap.GetUnitAt(x-1, y));
        return walkingMap.GetWorldPos(x, y, transform.position);
    }
}
