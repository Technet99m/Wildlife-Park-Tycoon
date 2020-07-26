using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalUpgradesManager : MonoBehaviour
{
    public static Dictionary<string, AnimalUpgrade> upgrades { get; private set; }
    private void Awake()
    {
        upgrades = JsonConvert.DeserializeObject<Dictionary<string, AnimalUpgrade>>(PlayerPrefs.GetString("Upgrades", Resources.Load<TextAsset>("DefaultUpgrades").text));
        foreach(var kind in upgrades.Keys)
        {
            ApplyUpgrades(Resources.Load<AnimalStats>($"Animals/{kind}/Stats"), upgrades[kind].currentStage);
        }
    }

    private void ApplyUpgrades(AnimalStats stats, int stage)
    {

    }
}
