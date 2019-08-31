using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

static class RandomGenerator
{
    public static int[,] GenerateValues(int xLength, int yLength, float perlinNoizeDenominator)
    {
        int[,] pointsValues = new int[xLength + 1, yLength + 1];
        for (int y = 0; y < yLength + 1; y++)
        {
            for (int x = 0; x < xLength + 1; x++)
            {
                pointsValues[x, y] = (int)Mathf.Round(Mathf.PerlinNoise(
                   x * perlinNoizeDenominator,
                   y * perlinNoizeDenominator));
            }
        }
        return pointsValues;
    }
}

