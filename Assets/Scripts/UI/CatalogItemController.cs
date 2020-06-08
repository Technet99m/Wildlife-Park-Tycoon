using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatalogItemController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] foods, specials;
    [SerializeField]
    private Text Name, Price;
    [SerializeField]
    private Text preg, grow, children;
    [SerializeField]
    private Image Icon;
    

    public void SetUp(string kind)
    {
        AnimalStats stats = Resources.Load<AnimalStats>($"Animals/{kind}/Stats");
        foreach (var f in stats.foods)
            foods[(int)f].SetActive(true);
        foreach (var s in stats.specials)
            specials[(int)s].SetActive(true);
        Name.text = stats.kind;
        Icon.sprite = Resources.Load<Sprite>($"Animals/{kind}/Icon");
        Price.text = stats.price.ToString();
        preg.text = Translator.TicksToTime(stats.TicksToBorn);
        grow.text = Translator.TicksToTime(stats.TicksToFullMate);
        children.text = stats.minChildren == stats.maxChildren ? $"{stats.minChildren}" : $"{stats.minChildren}-{stats.maxChildren}";
    }
}
