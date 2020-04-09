using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : MonoBehaviour
{
    [SerializeField] Transform[] actionPoints;
    [SerializeField] bool[] areBusy;
    void Awake()
    {
        areBusy = new bool[actionPoints.Length];
    }
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
    public void Empty(Vector2 pos)
    {
        for(int i = 0;i<actionPoints.Length;i++)
        {
            if(Vector2.Distance(actionPoints[i].position, pos)<0.25f)
            {
                areBusy[i] = false;
                break;
            }
        }
    }
}
