using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Mugnifier
{
    public static CellType[,] Impose(CellType[,] from, int toWidht, int toHeight)
    {
        var result = new CellType[toWidht, toHeight];
        int maxValue = 0;
        for (int x = 0; x < toWidht; x++)
        {
            for (int y = 0; y < toHeight; y++)
            {
                int fromX = (int)(/*((float)from.GetLength(0) / (float) toWidht) */.3f * x);
                fromX = Mathf.Clamp(fromX, 0, from.GetLength(0) - 1);
                int fromY = (int)(/*((float)from.GetLength(1) / (float)toHeight)*/.3f * y);
                fromY = Mathf.Clamp(fromY, 0, from.GetLength(1) - 1);
                result[x, y] = from[fromX, fromY];
                if (fromX > maxValue)
                {
                    maxValue = fromX;
                }
            }
        }
        Debug.Log(maxValue);
        return result;
    }
}

