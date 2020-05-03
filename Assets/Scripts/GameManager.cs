using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Technet99m.Singleton<GameManager>
{
    public List<Cage> cages;
    public Cage activeCage { get => cages[currentCageIndex]; }
    public int currentCageIndex;
    public float cellSize;
}
