using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyCounter : MonoBehaviour
{
    [SerializeField] bool gems;
    [SerializeField] Text text;
    [SerializeField] Technet99m.ChildContentFitter fitter;

    private void OnEnable()
    {
        if (gems)
            DataManager.gemsChanged += Refresh;
        else
            DataManager.moneyChanged += Refresh;
        Refresh(gems ? DataManager.Gems : DataManager.Money);
    }
    private void OnDisable()
    {
        if (gems)
            DataManager.gemsChanged -= Refresh;
        else
            DataManager.moneyChanged -= Refresh;
    }
    private void Refresh(int value)
    {
        text.text = Translator.CurrencyToString(value);
        fitter.Fit();
    }
}
