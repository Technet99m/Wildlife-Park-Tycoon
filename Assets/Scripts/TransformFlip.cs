using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFlip : MonoBehaviour
{
   [SerializeField] Transform target;

    Coroutine flip;
   public void FlipX(int frames)
	{
        if (flip != null)
            StopCoroutine(flip);
        flip = StartCoroutine(FlipTo(new Vector3(target.localScale.x * -1f, target.localScale.y, target.localScale.z), frames));
	}
	
	public void FlipY(int frames)
	{
        if (flip != null)
            StopCoroutine(flip);
        flip = StartCoroutine(FlipTo(new Vector3(target.localScale.x, target.localScale.y * -1f, target.localScale.z), frames));
    }

    private IEnumerator FlipTo(Vector3 destination, int frameLength)
    {
        Vector3 start = target.localScale;
        while(target.localScale!=destination)
        {
            target.localScale = Vector3.MoveTowards(target.localScale, destination, Vector3.Distance(start, destination)/frameLength);
            yield return null;
        }
    }
}
