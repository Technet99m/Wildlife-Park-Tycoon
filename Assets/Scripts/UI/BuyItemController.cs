using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyItemController : MonoBehaviour
{
    [SerializeField] GameObject[] items;

    public void BuyItem(int index)
    {
        Instantiate(items[index], (Vector2)GameManager.Ins.cam.position, Quaternion.identity);
    }
}
