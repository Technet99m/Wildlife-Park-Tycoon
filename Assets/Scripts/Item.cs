﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : MonoBehaviour
{
    [SerializeField] Transform[] actionPoints;
    [SerializeField] bool[] areBusy;

    public Transform GetFree()
    {
        for (int i = 0; i < actionPoints.Length; i++)
        {
            if (!areBusy[i])
            {
                areBusy[i] = true;
                return actionPoints[i];
            }
        }
        return null;
    }
}
