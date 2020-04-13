using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SetPanelController : MonoBehaviour
{
    [SerializeField]Button ok, cancel;
    public UnityAction okPressed;
    void OnEnable()
    {
        ok.onClick.AddListener(okPressed);
    }
}
