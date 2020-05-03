using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CageMenuItemController : MonoBehaviour
{
    [SerializeField] private Text animalName;
    [SerializeField] private Text animalHappiness;
    [SerializeField] private Text animalPrice;
    [SerializeField] private Image animalIcon;
    [SerializeField] private Image animalHappinessIcon;
    [SerializeField] private Image animalSex;
    [SerializeField] private Slider animalProgress;
    [SerializeField] private Toggle animalSelected;

    public void SetUp(Animal animal)
    {
        animalName.text = animal.data.name;
        animalHappiness.text = Mathf.RoundToInt(animal.data.happiness * 100).ToString() + "%";
        animalHappiness.color = Translator.HappinessColor(animal.data.happiness);
        animalPrice.text = "";
        animalIcon.sprite = null;
        animalHappinessIcon.sprite = Translator.Happiness(animal.data.happiness);
        animalSex.sprite = Translator.Sex(animal.data.male);
        animalProgress.value = animal.data.sexualActivity;
        animalSelected.isOn = false;
        gameObject.SetActive(true);
    }
}
