using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    [SerializeField] Vector2 offset;
    MovementController mc;
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (mc == null)
                mc = GetComponent<MovementController>();
            var go = new GameObject().transform;
            go.position = (Vector2)Technet99m.Utils.ScreenToWorldPoint(Input.mousePosition) + offset;
            mc.SetNewTarget(go);
        }
    }
}
