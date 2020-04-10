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
        int closest = 0;
        for(int i = 1;i<actionPoints.Length;i++)
        {
            if(Vector2.Distance(actionPoints[i].position, pos)< Vector2.Distance(actionPoints[closest].position, pos))
            {
                closest = i;
            }
        }
        areBusy[closest] = false;
    }
}
