﻿using System.Collections;
using System.Collections.Generic;
using Technet99m;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField, Range(1f,5f)] private float speed;
    private AnimalAnimationController anim;
    private Pathfinding pathfinding;
    private List<PathNode> path;
    private Cage cage;
    private Vector2 target;
    private Transform self;

    public event System.Action TargetReached;
    public bool isWalking;
    private void Start()
    {
        path = new List<PathNode>();
        cage = GetComponent<Animal>().cage;
        anim = GetComponent<AnimalAnimationController>();
        self = transform;
    }
    private void Update()
    {
        DrawPath();
        if (path.Count>0)
        {
            self.position = Vector2.MoveTowards(self.position, target, speed * Time.deltaTime);
            self.position = new Vector3(self.position.x, self.position.y, self.position.y * 10f);
            if ((Vector2)self.position == target)
            {
                path.RemoveAt(0);
                if (path.Count == 0)
                {
                    Stop();
                    TargetReached?.Invoke();
                }
                else
                {
                    target = pathfinding.Grid.GetWorldPos(path[0].x, path[0].y, cage.transform.position)+Vector2.up* (Random.value>0.5? Random.Range(0.02f,0.04f): -Random.Range(0.02f, 0.04f));
                    anim.Walk(target - (Vector2)self.position);
                }
            }
        }
    }
    private void DrawPath()
    {
        for (int i = 1; i < path.Count; i++)
        {
            Debug.DrawLine(pathfinding.Grid.GetWorldPos(path[i - 1].x, path[i - 1].y, cage.transform.position), pathfinding.Grid.GetWorldPos(path[i].x, path[i].y, cage.transform.position), Color.green);
        }
    }
    public bool SetNewTarget(Vector2 target)
    {
        isWalking = true;
        pathfinding = new Pathfinding(cage.walkingMap);
        pathfinding.Grid.GetXY(self.position, cage.transform.position, out int sX, out int sY);
        pathfinding.Grid.GetXY(target, cage.transform.position, out int eX, out int eY);
        path = pathfinding.FindPath(sX, sY, eX, eY);
        if (path == null)
            return false;
        if (path.Count > 1)
            path.RemoveAt(0);
        this.target = pathfinding.Grid.GetWorldPos(path[0].x, path[0].y, cage.transform.position);
        anim.Walk(this.target - (Vector2)self.position);
        return true;
    }
    public bool RecalculatePath()
    {
        if (isWalking)
            return SetNewTarget(pathfinding.Grid.GetWorldPos(path[path.Count - 1].x, path[path.Count - 1].y, cage.transform.position));
        return true;
    }
    public void Stop()
    {
        path = new List<PathNode>();
        isWalking = false;
        anim.Idle();
    }
}
