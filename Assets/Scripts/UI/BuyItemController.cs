using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyItemController : MonoBehaviour
{
    [SerializeField] GameObject[] items;

    public void BuyItem(int index)
    {
        if (DataManager.TryAndBuyForMoney(items[index].GetComponent<Item>().price))
            Instantiate(items[index], (Vector2)GameManager.Ins.cam.position, Quaternion.identity);

    }
}
