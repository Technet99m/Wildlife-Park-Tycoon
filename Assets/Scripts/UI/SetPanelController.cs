using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SetPanelController : MonoBehaviour
{
    public Button ok, cancel;
    public UnityAction okPressed;
    public UnityAction cancelPressed;

    private void OnEnable()
    {
        ok.onClick.AddListener(okPressed);
        cancel.onClick.AddListener(cancelPressed);
    }
    private void OnDisable()
    {
        ok.onClick.RemoveListener(okPressed);
        cancel.onClick.RemoveListener(cancelPressed);
    }
}
