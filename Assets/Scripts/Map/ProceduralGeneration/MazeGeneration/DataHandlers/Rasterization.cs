using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Rasterization
{
    public static CellType[,] Rasterize(List<Room> rooms, int height, int width, float noizeRation, float cellSideLength)
    {
        var cells = new CellType[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cells[x, y] = CellType.Live;

            }
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (UnityEngine.Random.Range(0f, 1f) < noizeRation)
                {
                    cells[x, y] = CellType.Dead;
                }
            }
        }
        foreach (var room in rooms)
        {
            int xMin = Mathf.FloorToInt(room.bottomLeftCorner.position.x / cellSideLength);
            int xMax = Mathf.CeilToInt((room.bottomRightCorner.position.x) / cellSideLength);
            int yMin = Mathf.FloorToInt((room.bottomLeftCorner.position.y) / cellSideLength);
            int yMax = Mathf.CeilToInt((room.topLeftCorner.position.y) / cellSideLength);
            //Debug.Log($"{xMin} {xMax} {yMin} {yMax} {pointsValues.Length}");
            for (int x = xMin; x < xMax; x++)
            {
                for (int y = yMin; y < yMax; y++)
                {
                    try
                    {
                        cells[x, y] = CellType.StrictlyDead;
                    }
                    catch (System.IndexOutOfRangeException)  // CS0168
                    {
                    }
                }
            }
        }
        return cells;
    }
}


