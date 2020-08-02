using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RaysController : MonoBehaviour
{
    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private float length;

    [SerializeField]
    private float minWidth, maxWidth;

    [SerializeField]
    private float minSpeed,maxSpeed;

    [SerializeField]
    private float minAlpha, maxAlpha;

    [SerializeField]
    private int count;

    private Image[] rays;
    private float[] speeds;

    private void Update()
    {
        if(rays!=null)
        {
            for(int i =0;i<count;i++)
            {
                rays[i].transform.rotation = Quaternion.Euler(0, 0, rays[i].transform.eulerAngles.z + speeds[i] * Time.deltaTime);
            }
        }
    }

    public void Show()
    {
        rays = new Image[count];
        speeds = new float[count];
        for (int i = 0; i < count; i++)
        {
            var go = new GameObject("Ray", typeof(Image));
            go.transform.parent = transform;
            rays[i] = go.GetComponent<Image>();
            rays[i].sprite = sprite;
            rays[i].color = new Color(1, 1, 1, 0);
            rays[i].DOFade(Random.Range(minAlpha, maxAlpha), 0.3f);
            rays[i].rectTransform.pivot = new Vector2(0.5f, 1);
            rays[i].rectTransform.localScale = Vector3.one;
            rays[i].rectTransform.anchoredPosition = Vector2.zero;
            rays[i].rectTransform.sizeDelta = new Vector2(Random.Range(minWidth, maxWidth), length);
            rays[i].rectTransform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            speeds[i] = Random.Range(minSpeed, maxSpeed) * (Random.value > 0.5 ? 1 : -1);
        }
    }

    public void Hide()
    {
        for(int i = 0;i<count;i++)
        {
            Destroy(rays[i].gameObject);
        }
        rays = null;
    }
}
