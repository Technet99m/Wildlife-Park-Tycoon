using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Technet99m
{
    public class ChildContentFitter : MonoBehaviour
    {
        [SerializeField] RectTransform target;
        
        public FitMode Horizontal;
        [HideInInspector]public  float left, right;
        [HideInInspector]public  float minWidth;
        public FitMode Vertical;
        [HideInInspector] public float top, bottom;
        [HideInInspector] public float minHeight;

        RectTransform my;
        public void Fit()
        {
            if (my == null)
                my = GetComponent<RectTransform>();
            if (Horizontal == FitMode.PrefferedSize)
                my.sizeDelta = new Vector2(target.sizeDelta.x, my.sizeDelta.y);
            else if (Horizontal == FitMode.WithOffsets)
            {
                my.sizeDelta = new Vector2(target.sizeDelta.x + left + right, my.sizeDelta.y);
                if (my.sizeDelta.x < minWidth)
                    my.sizeDelta = new Vector2(minWidth, my.sizeDelta.y);
            }
            if (Vertical == FitMode.PrefferedSize)
                my.sizeDelta = new Vector2(my.sizeDelta.x, target.sizeDelta.y);
            else if (Vertical == FitMode.WithOffsets)
            {
                my.sizeDelta = new Vector2(my.sizeDelta.x, target.sizeDelta.y + top + bottom);
                if (my.sizeDelta.y < minHeight)
                    my.sizeDelta = new Vector2(my.sizeDelta.x, minHeight);
            }
        }
        public void LateUpdate()
        {
            Fit();
        }
    }

    public enum FitMode
    {
        Unconstrained = 0,
        PrefferedSize,
        WithOffsets
    }
}
