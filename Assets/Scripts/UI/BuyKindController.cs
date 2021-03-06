﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyKindController : MonoBehaviour
{
    public Image preview;
    public Text kind;
    public Text price;

    public void BuyThis()
    {
        transform.parent.parent.parent.parent.parent.parent.GetComponent<CageMenuController>().BuyNewKind(kind.text);
    }
    public void ToCatalog()
    {
        GameManager.Ins.catalog.ToAnimal(kind.text);
    }
    public void SetUp(string kind)
    {
        this.kind.text = kind;
        preview.sprite = Resources.Load<Sprite>($"Animals/{kind}/Picture");
        price.text = Translator.CurrencyToString(Translator.KindToPrice(kind));
        gameObject.SetActive(true);
    }
}
