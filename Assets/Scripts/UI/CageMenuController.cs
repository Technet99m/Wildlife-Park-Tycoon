using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CageMenuController : MonoBehaviour
{
    [SerializeField] private Text cageName;
    [SerializeField] private Text cageCapacity;
    [SerializeField] private CageMenuItemController[] items;
    [SerializeField] private GameObject itemRef;
    private Cage activeCage;
    private void OnEnable()
    {
        Technet99m.TickingMachine.EveryTick += Refresh;
    }
    private void OnDisable()
    {
        Technet99m.TickingMachine.EveryTick -= Refresh;
    }
    private void HideAll()
    {
        foreach (var item in items)
            item.gameObject.SetActive(false);
    }
    public void SetUp()
    {
        activeCage = GameManager.Ins.activeCage;
        cageCapacity.text = $"{activeCage.animals.Count}/{15}";
        HideAll();
        for (int i = 0; i < activeCage.animals.Count; i++)
        {
            items[i].SetUp(activeCage.animals[i]);
        }
        gameObject.SetActive(true);
    }
    public void Refresh()
    {
        cageCapacity.text = $"{activeCage.animals.Count}/{15}";
        HideAll();
        for(int i = 0;i< activeCage.animals.Count; i++)
        {
            items[i].Refresh(activeCage.animals[i]);
        }
    }
    public void Sell()
    {
        List<Animal> listToSell = new List<Animal>();
        for (int i = 0; i < activeCage.animals.Count; i++)
            if (items[i].Selected)
                listToSell.Add(activeCage.animals[i]);
        foreach (var animal in listToSell)
        {
            animal.Sell();
        }
        Refresh();
        foreach (var item in items)
            item.Selected = false;
    }
    public void SelectAll()
    {
        foreach (var item in items)
            if (item.gameObject.activeSelf)
                item.Selected = true;

    }
    public void SelectChildren()
    {
        for (int i = 0; i < activeCage.animals.Count; i++)
            ;
    }
}
