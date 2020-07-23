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

    public int capacity = -1;

    private int maxCapacity = 200;

    protected new void Start()
    {
        if(capacity == -1)
            capacity = maxCapacity;
        base.Start();
    }

    public override void Empty(Vector2 pos)
    {
        capacity--;
        CheckSprite();
        base.Empty(pos);
    }
    public override void Place()
    {
        CheckSprite();
        base.Place();
    }
    public void Refill()
    {
        if (!DataManager.TryAndBuyForMoney(price))
            return;
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
    public void CheckSprite()
    {
        if (capacity > maxCapacity / 2)
            sp.sprite = full;
        else if (capacity > 0)
            sp.sprite = half;
        else
            sp.sprite = empty;
    }

}
