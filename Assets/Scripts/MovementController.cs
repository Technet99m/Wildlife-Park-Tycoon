using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    Vector2 target;
    [SerializeField, Range(1f,5f)] float speed;
    AnimalAnimationController anim;
    public event System.Action TargetReached;
    void Start()
    {
        target = transform.position;
        anim = GetComponent<AnimalAnimationController>();
    }
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y * 10f);
        if ((Vector2)transform.position == target)
        {
            TargetReached?.Invoke();
        }
    }
    public void SetNewTarget(Vector2 target)
    {
        this.target = target;
        anim.Walk(target-(Vector2)transform.position);
    }
}
