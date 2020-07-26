using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Technet99m
{
    public class ToggleSwitch : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent OnTrue, OnFalse;

        public void Action(bool isOn)
        {
            if (isOn)
                OnTrue?.Invoke();
            else
                OnFalse?.Invoke();
        }
    }
}