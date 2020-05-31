using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SetPanelController : MonoBehaviour
{
    [SerializeField]Button ok, cancel;
    public UnityAction okPressed;
    public UnityAction cancelPressed;
    void OnEnable()
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
