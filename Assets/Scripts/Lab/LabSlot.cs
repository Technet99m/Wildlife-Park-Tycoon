using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabSlot : MonoBehaviour
{
    [SerializeField]
    private Text time;
    [SerializeField]
    private Image animal;

    public bool isBusy;
    public bool isOpened;

    public int secondsRemain;

    [HideInInspector]
    public string animalKind;

    public void Initialize( LabSlotData slot, int secondsPast)
    {
        isOpened = slot.isOpened;

        if (!isOpened)
        {
            gameObject.SetActive(false);
            return;
        }

        isBusy = slot.isBusy;
        animal.gameObject.SetActive(isBusy);
        if(isBusy)
        {
            animal.sprite = null;
        }

        secondsRemain = slot.secondsRemain - secondsPast;
        animalKind = slot.animalKind;

        if (secondsRemain < 0)
            secondsRemain = 0;
    }
    public void Occupy(string kind)
    {
        isBusy = true;
        animalKind = kind;
        secondsRemain = Resources.Load<AnimalStats>($"Animals/{animalKind}/Stats").TicksInLab;
        animal.gameObject.SetActive(true);
        animal.sprite = null;
        Refresh();
    }
    public void Refresh()
    {
        animal.gameObject.SetActive(isBusy);
        if (secondsRemain==0)
        {
            time.text = isBusy ? "Finished" : "Ready";
            return;
        }
        time.text = Translator.TicksToTime(secondsRemain);
    }
    public void OnSlotClicked()
    {
        if (isBusy && secondsRemain == 0)
        {
            foreach(var biome in Resources.LoadAll<Biome>("Biomes"))
            {
                foreach(var kind in biome.kinds)
                {
                    if(kind == animalKind)
                    {
                        DataManager.AddPotions(biome.Name, Resources.Load<AnimalStats>($"Animals/{animalKind}/Stats").PotionsPerResearch);
                        isBusy = false;
                        Refresh();
                        return;
                    }
                }
            }
        }
    }
}
public class LabSlotData
{
    public bool isBusy;
    public bool isOpened;
    public int secondsRemain;
    public string animalKind;
    public LabSlotData()
    {

    }

    public LabSlotData(LabSlot slot)
    {
        isBusy = slot.isBusy;
        isOpened = slot.isOpened;
        secondsRemain = slot.secondsRemain;
        animalKind = slot.animalKind;
    }

}
