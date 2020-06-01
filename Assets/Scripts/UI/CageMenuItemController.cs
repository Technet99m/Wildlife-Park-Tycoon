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
    [SerializeField] private Image progressFill;
    [SerializeField] private Image progressIcon;

    [SerializeField] private Sprite sexFill, ageFill, pregnancyFill;
    [SerializeField] private Sprite sexIcon, sexIconActive, ageIcon, pregnancyIcon;
    public bool Selected { get => animalSelected.isOn; set { animalSelected.isOn = value; } }

    public void SetUp(Animal animal)
    {
        animalName.text = animal.data.name;
        animalHappiness.text = Mathf.RoundToInt(animal.data.happiness * 100).ToString() + "%";
        animalHappiness.color = Translator.HappinessColor(animal.data.happiness);
        animalPrice.text = animal.stats.price.ToString();
        animalIcon.sprite = Resources.Load<Sprite>($"Animals/{animal.stats.kind}/Icon");
        animalHappinessIcon.sprite = Translator.Happiness(animal.data.happiness);
        animalSex.sprite = Translator.Sex(animal.data.male);
        if (animal.data.age > 1 && !animal.data.pregnant)
        {
            animalProgress.value = animal.data.sexualActivity;
            progressFill.sprite = sexFill;
            progressIcon.sprite = animal.data.sexualActivity > 1 ? sexIconActive : sexIcon;
        }
        else if (animal.data.age < 1)
        {
            animalProgress.value = animal.data.age;
            progressFill.sprite = ageFill;
            progressIcon.sprite = ageIcon;
        }
        else
        {
            animalProgress.value = animal.data.pregnancy;
            progressFill.sprite = pregnancyFill;
            progressIcon.sprite = pregnancyIcon;
        }
        animalSelected.isOn = false;
        gameObject.SetActive(true);
    }
    public void Refresh(Animal animal)
    {
        animalName.text = animal.data.name;
        animalHappiness.text = Mathf.RoundToInt(animal.data.happiness * 100).ToString() + "%";
        animalHappiness.color = Translator.HappinessColor(animal.data.happiness);
        animalPrice.text = Mathf.CeilToInt(animal.stats.price * animal.data.happiness).ToString();
        animalIcon.sprite = Resources.Load<Sprite>($"Animals/{animal.stats.kind}/Icon");
        animalHappinessIcon.sprite = Translator.Happiness(animal.data.happiness);
        animalSex.sprite = Translator.Sex(animal.data.male);
        if (animal.data.age >= 1 && !animal.data.pregnant)
        {
            animalProgress.value = animal.data.sexualActivity;
            progressFill.sprite = sexFill;
            progressIcon.sprite = animal.data.sexualActivity > 1 ? sexIconActive : sexIcon;
        }
        else if (animal.data.age < 1)
        {
            animalProgress.value = animal.data.age;
            progressFill.sprite = ageFill;
            progressIcon.sprite = ageIcon;
        }
        else
        {
            animalProgress.value = animal.data.pregnancy;
            progressFill.sprite = pregnancyFill;
            progressIcon.sprite = pregnancyIcon;
        }
        gameObject.SetActive(true);
    }
}
