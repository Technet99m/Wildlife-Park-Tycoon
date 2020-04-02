using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Technet99m
{
    public class CounterFitter : MonoBehaviour
    {
        Text counter;
        [SerializeField] RectTransform bg;
        [SerializeField] int minDigits;
        [SerializeField] float minSize, deltaSize;

        private void Awake()
        {
            counter = GetComponentInChildren<Text>();
        }
        public void SetCounterTo(int to)
        {
            counter.text = to.ToString();
            int digits = 1;
            for (; to >= 10; digits++)
                to /= 10;
            if (digits <= minDigits)
            {
                bg.sizeDelta = new Vector2(minSize, bg.sizeDelta.y);
            }
            else
            {
                bg.sizeDelta = new Vector2(minSize + (digits - minDigits) * deltaSize, bg.sizeDelta.y);
            }
        }
        public void SetCounterTo(float to, int digitsAfterDot)
        {
            if (counter == null)
                counter = GetComponentInChildren<Text>();
            counter.text = to.ToString($"N{digitsAfterDot}");
            int digits = 1;
            for (; to >= 10; digits++)
                to /= 10;
            digits += digitsAfterDot + 1;
            if (digits <= minDigits)
            {
                bg.sizeDelta = new Vector2(minSize, bg.sizeDelta.y);
            }
            else
            {
                bg.sizeDelta = new Vector2(minSize + (digits - minDigits) * deltaSize, bg.sizeDelta.y);
            }
        }
        public void SetText()
        {
            SetCounterTo(int.Parse(counter.text));
        }
    }
}
