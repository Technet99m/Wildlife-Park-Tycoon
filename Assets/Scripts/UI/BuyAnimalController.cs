using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyAnimalController : MonoBehaviour
{
    public void BuyNew(bool male)
    {
        Animal animal = BuyAnimal();
        if (animal != null)
        {
            animal.data.male = male;
            animal.transform.position = GameManager.Ins.activeCage.GetFreeTileInGrid();
        }
    }
    private Animal BuyAnimal()
    {
        if (DataManager.TryAndBuyForMoney(Translator.KindToPrice(GameManager.Ins.activeCage.KindInCage)))
            return AnimalFactory.NewAnimalOfKind(GameManager.Ins.activeCage.KindInCage, GameManager.Ins.activeCage.transform);
        else
            return null;
    }
}
