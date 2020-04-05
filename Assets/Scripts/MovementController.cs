using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    Transform target;
    [SerializeField, Range(1f,5f)] float speed;
    AnimalAnimationController anim;
    public event System.Action TargetReached;
    void Start()
    {
        target = null;
        anim = GetComponent<AnimalAnimationController>();
    }
    void Update()
    {
        if (target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y * 10f);
            if ((Vector2)transform.position == (Vector2)target.position)
            {
                target = null;
                TargetReached?.Invoke();
            }
        }
    }
    public void SetNewTarget(Transform target)
    {
        this.target = target;
        anim.Walk(target.position-transform.position);
    }
}
