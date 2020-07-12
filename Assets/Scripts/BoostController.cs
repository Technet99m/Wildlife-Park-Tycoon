using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostController : MonoBehaviour
{
    public static List<Boost> boosts { get; private set; }
    public static void LoadBoosts(List<Boost> b)
    {
        boosts = b;
    }
    [SerializeField]
    private UIBoost[] uiBoosts;

    private int defaultTicks = 420;
    private void OnEnable()
    {
        Technet99m.TickingMachine.EveryTick += OnTick;
    }
    private void OnDisable()
    {
        Technet99m.TickingMachine.EveryTick -= OnTick;
    }
    public void AddBoost(Boost boost)
    {
        boosts.Add(boost);
        var tmp = uiBoosts[boosts.Count - 1];
        tmp.gameObject.SetActive(true);
        tmp.icon.sprite = Translator.Boost(boost.type);
        tmp.time.text = Translator.TicksToTime(boost.ticksRemain);
    }

    private void OnTick()
    {
        if (boosts == null)
            boosts = new List<Boost>();
        for (int i = 0; i < boosts.Count; i++)
        {
            boosts[i].ticksRemain--;
            uiBoosts[i].gameObject.SetActive(true);
            uiBoosts[i].icon.sprite = Translator.Boost(boosts[i].type);
            uiBoosts[i].time.text = Translator.TicksToTime(boosts[i].ticksRemain);
            if(boosts[i].ticksRemain==0)
            {
                boosts.RemoveAt(i);
                uiBoosts[i].gameObject.SetActive(false);
                i--;
            }
        }
    }
    public void BuyVitamins(int stage)
    {
        Boost vit = new Boost();
        vit.type = BoostType.vitamins;
        vit.ticksRemain = defaultTicks;
        if (stage == 1)
            vit.ticksRemain *= 2;
        if (stage == 2)
            vit.power = 2;
        else
            vit.power = 1;
        AddBoost(vit);
    }
    public void BuySpecialFood(int stage)
    {
        Boost sp = new Boost();
        sp.type = BoostType.specialFood;
        sp.ticksRemain = defaultTicks;
        if (stage == 1)
            sp.ticksRemain *= 2;
        if (stage == 2)
            sp.power = 2;
        else
            sp.power = 1;
        AddBoost(sp);
    }
    public void BuyFeromones(int stage)
    {
        Boost fer = new Boost();
        fer.type = BoostType.feromons;
        fer.ticksRemain = defaultTicks;
        if (stage == 1)
            fer.ticksRemain *= 2;
        if (stage == 2)
            fer.power = 2;
        else
            fer.power = 1;
        AddBoost(fer);
    }
    public void BuyVirginBirth(int stage)
    {

    }

}
public class Boost
{
    public BoostType type;
    public int ticksRemain;
    public int power;
}
public enum BoostType
{
    vitamins,
    virginBirth,
    specialFood,
    feromons
}
