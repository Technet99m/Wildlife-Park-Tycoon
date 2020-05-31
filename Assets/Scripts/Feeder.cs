using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Feeder : Item
{
    [SerializeField]
    private Sprite full, half, empty;
    [SerializeField]
    private SpriteRenderer sp;


    public Food type;

    public int capacity;

    private int maxCapacity = 6;

    public override void Empty(Vector2 pos)
    {
        capacity--;
        if (capacity > maxCapacity / 2)
            sp.sprite = full;
        else if (capacity > 0)
            sp.sprite = half;
        else
            sp.sprite = empty;
        base.Empty(pos);
    }
    public override void Place()
    {
        Refill();
        base.Place();
    }
    public void Refill()
    {
        capacity = maxCapacity;
        sp.sprite = full;
    }
    public bool HasEnough()
    {
        int busy = 0;
        foreach(var b in areBusy)
        {
            if(b)
                busy++;
        }
        return (capacity - busy) > 0;
    }
}
