using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostController : MonoBehaviour
{
    public static List<Boost> boosts;

    private void OnEnable()
    {
        Technet99m.TickingMachine.EveryTick += OnTick;
    }
    private void OnDisable()
    {
        Technet99m.TickingMachine.EveryTick -= OnTick;
    }
    private void OnTick()
    {
        if (boosts == null)
            boosts = new List<Boost>();
        foreach (var boost in boosts)
            boost.ticksRemain--;
    }
}
public class Boost
{
    public BoostType type;
    public int ticksRemain;
}
public enum BoostType
{
    vitamins,
    virginBirth,
    specialFood,
    feromons
}
