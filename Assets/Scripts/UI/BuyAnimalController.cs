using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyAnimalController : MonoBehaviour
{
    [SerializeField] Animator anim;

    public void Show()
    {

    }
    public void Hide()
    {

    }
    public void BuyNew(bool male)
    {
        Animal animal = BuyAnimal();
        if (animal != null)
        {
            animal.data.male = male;
            animal.transform.position = GameManager.Ins.activeCage.RandomFreePos;
        }
    }
    private Animal BuyAnimal()
    {
        return AnimalFactory.NewAnimalOfKind(GameManager.Ins.activeCage.KindInCage, GameManager.Ins.activeCage.transform);
    }
}
