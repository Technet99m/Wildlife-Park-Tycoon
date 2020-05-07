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
    public void Refresh()
    {
        activeCage = GameManager.Ins.activeCage;
        cageCapacity.text = $"{activeCage.animals.Count}/{15}";
        HideAll();
        for(int i = 0;i<activeCage.animals.Count;i++)
        {
            items[i].SetUp(activeCage.animals[i]);
        }
        gameObject.SetActive(true);
    }

}
