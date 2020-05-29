using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFlip : MonoBehaviour
{
   [SerializeField] Transform target;
   
   public void FlipX()
	{
		target.localScale = new Vector3(target.localScale.x*-1f,target.localScale.y,target.localScale.z);
	}
	
	public void FlipY()
	{
		target.localScale = new Vector3(target.localScale.x,target.localScale.y*-1f,target.localScale.z);
	}
}
