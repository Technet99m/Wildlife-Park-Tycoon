using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionsCounterController : MonoBehaviour
{
    [SerializeField]
    private Text[] potions;

    private void OnEnable()
    {
        DataManager.potionsChanged += UpdatePotion;
        potions[0].text = DataManager.ForestPotion.ToString();
        potions[1].text = DataManager.SavannahPotion.ToString();
        potions[2].text = DataManager.ArcticPotion.ToString();
        potions[3].text = DataManager.JunglePotion.ToString();
    }
    private void OnDisable()
    {
        DataManager.potionsChanged -= UpdatePotion;
    }
    private void UpdatePotion(int index,int amount)
    {
        potions[index].text = amount.ToString();
    }
}
