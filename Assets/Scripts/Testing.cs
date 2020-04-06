using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Technet99m;

public class Testing : MonoBehaviour
{
    Pathfinding pathfinding;
    List<PathNode> path;
    private void Start()
    {
        Grid<bool> walkMap = new Grid<bool>(10, 10, 0.7f);
        pathfinding = new Pathfinding(walkMap);
        pathfinding.Grid.CellSize = 1;
        path = new List<PathNode>();
    }
    private void Update()
    {
        pathfinding.Grid.ShowGrid(transform.position);
        if(Input.GetMouseButtonDown(0))
        {
            pathfinding.Grid.GetXY(Utils.ScreenToWorldPoint(Input.mousePosition), transform.position, out int x, out int y);
            path = pathfinding.FindPath(0, 0, x, y);
        }
        if(Input.GetMouseButtonDown(1))
        {
            pathfinding.Grid.GetXY(Utils.ScreenToWorldPoint(Input.mousePosition), transform.position, out int x, out int y);
            pathfinding.Grid.GetUnitAt(x, y).isWalkable = false;
            var go = new GameObject("block");
            go.transform.position = Utils.ScreenToWorldPoint(Input.mousePosition);
        }
        for(int i = 1;i<path.Count;i++)
        {
            Debug.DrawLine(pathfinding.Grid.GetWorldPos(path[i - 1].x, path[i - 1].y, transform.position), pathfinding.Grid.GetWorldPos(path[i].x, path[i].y, transform.position),Color.green);
        }

    }
}
