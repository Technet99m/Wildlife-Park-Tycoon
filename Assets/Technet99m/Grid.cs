using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Technet99m
{
    public class Grid<GridUnit>
    {
        int width;
        int height;
        float cellSize;
        public float CellSize { get { return cellSize; } set { if (value > 0) cellSize = value; } }
        GridUnit[,] units;
        public int Width {get {return width;} }
        public int Height {get {return height; } }
        public Grid(int width,int height, System.Func<Grid<GridUnit>, int, int, GridUnit> func, float cellSize = 1)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            units = new GridUnit[width, height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    units[x, y] = func.Invoke(this,x,y);
        }
        public Grid(int width,int height, float cellSize)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            units = new GridUnit[width, height];
        }
        public GridUnit GetUnitAt(int x,int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
                return default;
            return units[x, y];
        }
        public void SetValue(int x,int y, GridUnit val)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
                return;
            units[x, y] = val;
        }

        /// <summary>
        /// Get X and Y of cell of world position
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="startPos"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void GetXY(Vector2 pos, Vector2 startPos,out int x, out int y)
        {
            x = Mathf.FloorToInt((pos - startPos).x / cellSize);
            y = Mathf.FloorToInt((pos - startPos).y / cellSize);
        }
        public void ShowGrid(Vector2 startPos)
        {
            for (int i = 0; i <= width; i++)
                Debug.DrawLine(new Vector3(startPos.x + i * cellSize, startPos.y), new Vector3(startPos.x + i * cellSize, height * cellSize + startPos.y));
            for (int i = 0; i <= height; i++)
                Debug.DrawLine(new Vector3(startPos.x, i * cellSize + startPos.y), new Vector3(startPos.x + width * cellSize, i * cellSize + startPos.y));
        }

        /// <summary>
        /// Get World Position of cell with X and Y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="startPos"></param>
        /// <returns></returns>
        public Vector2 GetWorldPos(int x,int y, Vector2 startPos)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
                return default;
            return new Vector2(startPos.x + x * cellSize + cellSize / 2, startPos.y + y * cellSize + cellSize / 2);
        }

        public static string Grid2String(Technet99m.Grid<GridUnit> g)
        {
            string s = "";
            GridUnit[] arr = new GridUnit[g.Width * g.Height];
            for (int x = 0; x < g.Width; x++)
                for (int y = 0; y < g.Height; y++)
                    arr[x + y * g.Width] = g.GetUnitAt(x, y);
            return JsonUtility.ToJson(new GridSave<GridUnit>() { array = arr, gridWidth = g.Width , cellSize = g.cellSize});
        }
        public static Grid<GridUnit> String2Grid(string s)
        {
            var tmp = JsonUtility.FromJson<GridSave<GridUnit>>(s);
            Grid<GridUnit> grid = new Grid<GridUnit>(tmp.gridWidth, tmp.array.Length / tmp.gridWidth, tmp.cellSize);
            for (int i = 0; i < tmp.array.Length; i++)
                grid.SetValue(i % tmp.gridWidth, i / tmp.gridWidth, tmp.array[i]);
            return grid;
        }
        class GridSave<GridUnit>
        {
            public GridUnit[] array;
            public int gridWidth;
            public float cellSize;
        }
    }
}
