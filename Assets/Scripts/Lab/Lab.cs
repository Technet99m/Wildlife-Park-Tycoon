using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Technet99m;
using UnityEngine;

public class Lab : Singleton<Lab>
{
    [SerializeField]
    private LabSlot[] slots;

    private void OnEnable()
    {
        TickingMachine.EveryTick += OnTick;

        LoadData();

    }

    private void OnDisable()
    {
        TickingMachine.EveryTick -= OnTick;
        SaveData();
    }

    public bool TryAddAnimal(string kind)
    {
        LoadData();
        for (int i = 0;i<slots.Length; i++)
        {
            if(!slots[i].isBusy && slots[i].isOpened)
            {
                slots[i].Occupy(kind);
                SaveData();
                return true;
            }
        }
        return false;
    }
    private void OnTick()
    {
        for( int i = 0;i<slots.Length;i++)
        {
            if (!slots[i].isBusy)
                continue;
            if (slots[i].secondsRemain > 0)
            {
                slots[i].secondsRemain--;
                slots[i].Refresh();
            }
        }
    }
    private void LoadData()
    {
        long diff = Clock.ActualTime - long.Parse(PlayerPrefs.GetString("LabLastActualized", "0"));
        int secondsPast = Mathf.RoundToInt((float)(new TimeSpan(diff).TotalSeconds));
        for (int i = 0; i < slots.Length; i++)
        {
            string slotSave = PlayerPrefs.GetString($"Slot{i}", "");
            if (!string.IsNullOrEmpty(slotSave))
            {
                var slotData = JsonConvert.DeserializeObject<LabSlotData>(slotSave);
                slots[i].Initialize(slotData, secondsPast);
            }
            slots[i].Refresh();
        }
    }
    private void SaveData()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            PlayerPrefs.SetString($"Slot{i}", JsonConvert.SerializeObject(new LabSlotData(slots[i])));
        }
        PlayerPrefs.SetString("LabLastActualized", Clock.ActualTime.ToString());
    }

    
}
