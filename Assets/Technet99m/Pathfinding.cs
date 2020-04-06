using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Technet99m
{
    public class Pathfinding
    {
        const int STRAIGHT = 10;
        const int DIAGONAL = 14;
        Grid<PathNode> grid;
        public Grid<PathNode> Grid { get { return grid; } }
        List<PathNode> openList, closedList;
        public Pathfinding(int width,int height)
        {
            grid = new Grid<PathNode>(width, height, (Grid<PathNode> g, int x, int y)=>(new PathNode(g,x,y)));
        }
        public Pathfinding(Grid<bool> walkMap)
        {
            grid = new Grid<PathNode>(walkMap.Width, walkMap.Height, (Grid<PathNode> g, int x, int y) => (new PathNode(g, x, y)), walkMap.CellSize);
            for (int x = 0; x < grid.Width; x++)
                for (int y = 0; y < grid.Height; y++)
                    grid.GetUnitAt(x, y).isWalkable = walkMap.GetUnitAt(x, y);
        }

        public List<PathNode> FindPath(int startX,int startY,int endX,int endY)
        {
            var startNode = grid.GetUnitAt(startX, startY);
            var endNode = grid.GetUnitAt(endX, endY);
            openList = new List<PathNode> { startNode };
            closedList = new List<PathNode>();

            for (int x = 0; x < grid.Width; x++)
                for (int y = 0; y < grid.Height; y++)
                {
                    PathNode node = grid.GetUnitAt(x, y);
                    node.gCost = int.MaxValue;
                    node.prev = null; 
                }
            startNode.gCost = 0;
            startNode.hCost = GetHCost(startNode, endNode);
            while(openList.Count>0)
            {
                var currentNode = openList[0];
                if (currentNode == endNode)
                    return Path(currentNode);

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (var node in GetNeighbours(currentNode))
                {
                    
                    if (closedList.Contains(node)) continue;
                    if (!node.isWalkable)
                    {
                        closedList.Add(node);
                        continue;
                    }
                        int newGCost = currentNode.gCost + GetHCost(currentNode, node);
                    if (newGCost < node.gCost)
                    {
                        node.prev = currentNode;
                        node.gCost = newGCost;
                        node.hCost = GetHCost(node, endNode);
                        if (!openList.Contains(node))
                            openList.Add(node);
                    }
                }

                openList.Sort((x, y) => x.fCost.CompareTo(y.fCost));
            }
            return null;
        }
        List<PathNode> GetNeighbours(PathNode node)
        {
            List<PathNode> neighbours = new List<PathNode>();
            if(node.x > 0)
            {
                neighbours.Add(grid.GetUnitAt(node.x - 1, node.y));
                if (node.y > 0)
                    neighbours.Add(grid.GetUnitAt(node.x - 1, node.y - 1));
                if (node.y < grid.Height - 1)
                    neighbours.Add(grid.GetUnitAt(node.x - 1, node.y + 1));
            }
            if (node.x < grid.Width - 1)
            {
                neighbours.Add(grid.GetUnitAt(node.x + 1, node.y));
                if (node.y > 0)
                    neighbours.Add(grid.GetUnitAt(node.x + 1, node.y - 1));
                if (node.y < grid.Height - 1)
                    neighbours.Add(grid.GetUnitAt(node.x + 1, node.y + 1));
            }
            if(node.y>0)
                neighbours.Add(grid.GetUnitAt(node.x, node.y - 1));
            if (node.y < grid.Height - 1)
                neighbours.Add(grid.GetUnitAt(node.x, node.y + 1));
            return neighbours;
        }
        List<PathNode> Path(PathNode node)
        {
            List<PathNode> tmp = new List<PathNode> { node };
            while (node.prev != null)
            {
                tmp.Add(node.prev);
                node = node.prev;
            }
            tmp.Reverse();
            return tmp;
        }
        int GetHCost(PathNode a, PathNode b)
        {
            int Horizontal = Mathf.Abs(a.x - b.x);
            int Vertical = Mathf.Abs(a.y - b.y);
            int left = Mathf.Abs(Horizontal - Vertical);
            return DIAGONAL * Mathf.Min(Horizontal, Vertical) + STRAIGHT * left;
        }
    }
    public class PathNode
    {
        Grid<PathNode> grid;
        public int x, y;
        public bool isWalkable;

        public int gCost;
        public int hCost;
        public int fCost { get { return gCost + hCost; } }
        public PathNode prev;
        public PathNode(Grid<PathNode> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
            isWalkable = true;
        }
    }
}
