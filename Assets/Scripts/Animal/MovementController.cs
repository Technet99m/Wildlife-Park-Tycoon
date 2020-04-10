using System.Collections;
using System.Collections.Generic;
using Technet99m;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField, Range(1f,5f)] float speed;
    AnimalAnimationController anim;
    Pathfinding pathfinding;
    List<PathNode> path;
    Cage cage;
    Vector2 target;
    public event System.Action TargetReached;
    void Start()
    {
        path = new List<PathNode>();
        cage = GetComponent<Animal>().cage;
        anim = GetComponent<AnimalAnimationController>();
    }
    void Update()
    {
        DrawPath();
        cage.walkingMap.ShowGrid(cage.transform.position);
        if (path.Count>0)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y * 10f);
            if ((Vector2)transform.position == target)
            {
                path.RemoveAt(0);
                if (path.Count == 0)
                {
                    anim.Idle();
                    TargetReached?.Invoke();
                }
                else
                {
                    target = pathfinding.Grid.GetWorldPos(path[0].x, path[0].y, cage.transform.position)+Vector2.up* (Random.value>0.5? Random.Range(0.02f,0.06f): -Random.Range(0.02f, 0.06f));
                    anim.Walk(target - (Vector2)transform.position);
                }
            }
        }
    }
    void DrawPath()
    {
        for (int i = 1; i < path.Count; i++)
        {
            Debug.DrawLine(pathfinding.Grid.GetWorldPos(path[i - 1].x, path[i - 1].y, cage.transform.position), pathfinding.Grid.GetWorldPos(path[i].x, path[i].y, cage.transform.position), Color.green);
        }
    }
    public void SetNewTarget(Vector2 target)
    {
        pathfinding = new Pathfinding(cage.walkingMap);
        pathfinding.Grid.GetXY(transform.position, cage.transform.position, out int sX, out int sY);
        pathfinding.Grid.GetXY(target, cage.transform.position, out int eX, out int eY);
        path = pathfinding.FindPath(sX, sY, eX, eY);
        if (path.Count > 1)
            path.RemoveAt(0);
        this.target = pathfinding.Grid.GetWorldPos(path[0].x, path[0].y, cage.transform.position);
        anim.Walk(target - (Vector2)transform.position);
    }
}
