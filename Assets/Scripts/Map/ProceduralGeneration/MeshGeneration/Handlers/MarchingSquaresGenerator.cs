using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingSquaresGenerator
{
    private Dictionary<int, int[]> FromIntToTriangleIndexes = new Dictionary<int, int[]>();

    public int xLength = 100;
    public int yLength = 100;

    public float sideLength = 1f;

    public MarchingSquaresGenerator()
    {
        InitializeHelperDictionary();
    }

    public MarchingSquaresGenerator(int xLength, int yLength, float sideLength)
    {
        this.xLength = xLength;
        this.yLength = yLength;
        this.sideLength = sideLength;
        InitializeHelperDictionary();
    }

    public void InitializeHelperDictionary()
    {
        FromIntToTriangleIndexes.Add(0000, new int[] { });
        FromIntToTriangleIndexes.Add(1000, new int[] { 7, 5, 6 });
        FromIntToTriangleIndexes.Add(0001, new int[] { 3, 4, 5 });
        FromIntToTriangleIndexes.Add(1001, new int[] { 6, 7, 4, 4, 7, 3 });
        FromIntToTriangleIndexes.Add(0010, new int[] { 1, 2, 3 });
        FromIntToTriangleIndexes.Add(1010, new int[] { 6, 7, 1, 1, 2, 6, 6, 2, 3, 3, 5, 6 });
        FromIntToTriangleIndexes.Add(0011, new int[] { 5, 1, 4, 4, 1, 2 });
        FromIntToTriangleIndexes.Add(1011, new int[] { 7, 1, 6, 6, 1, 2, 2, 4, 6 });
        FromIntToTriangleIndexes.Add(0100, new int[] { 1, 7, 0 });
        FromIntToTriangleIndexes.Add(1100, new int[] { 6, 0, 1, 1, 5, 6 });
        FromIntToTriangleIndexes.Add(0101, new int[] { 7, 0, 5, 5, 0, 4, 4, 0, 3, 3, 0, 1 });
        FromIntToTriangleIndexes.Add(1101, new int[] { 6, 0, 4, 4, 0, 3, 3, 0, 1 });
        FromIntToTriangleIndexes.Add(0110, new int[] { 3, 7, 0, 0, 2, 3 });
        FromIntToTriangleIndexes.Add(1110, new int[] { 0, 2, 6, 6, 2, 5, 5, 2, 3 });
        FromIntToTriangleIndexes.Add(0111, new int[] { 7, 0, 5, 5, 0, 4, 4, 0, 2 });
        FromIntToTriangleIndexes.Add(1111, new int[] { 4, 6, 0, 0, 2, 4 });
    }

    public Mesh Generate(int[,] pointsValues)
    {
        MeshGenerator meshGenerator = new MeshGenerator(); 
        meshGenerator.StartGeneration();

        Square[,] squares = new Square[xLength, yLength];
        Point[,] points = new Point[xLength * 2 + 1, yLength * 2 + 1];

        AddPoints(points);
        AddSquares(pointsValues, points, squares);
        AddDataToMesh(squares, meshGenerator);

        return meshGenerator.EndGeneration();
    }

    private void AddDataToMesh(Square[,] squares, MeshGenerator meshGenerator)
    {
        for (int y = 0; y < yLength; y++)
        {
            for (int x = 0; x < xLength; x++)
            {
                int[] indexes = FromIntToTriangleIndexes[squares[x, y].Configuration];
                List<Point> trianglesPoints = new List<Point>();
                for (int i = 0; i < indexes.Length; i++)
                {
                    trianglesPoints.Add(squares[x, y].points[indexes[i]]);
                }
                meshGenerator.AddDataFromPoints(trianglesPoints.ToArray());
            }
        }
    }

    private void AddPoints(Point[,] points)
    {
        for (int y = 0; y < yLength; y++)
        {
            for (int x = 0; x < xLength; x++)
            {
                points[x * 2, y * 2] = new Point(new Vector3(x * sideLength, y * sideLength, 0f));
                points[x * 2 + 1, y * 2] = new Point(new Vector3(x * sideLength + sideLength / 2, y * sideLength, 0f));
                points[x * 2, y * 2 + 1] = new Point(new Vector3(x * sideLength, y * sideLength + sideLength / 2, 0f));
            }
        }
        if (xLength >= 1 && yLength >= 1)
        {
            for (int x = 0; x < xLength; x++)
            {
                points[x * 2, yLength * 2] = new Point(new Vector3(x * sideLength, yLength * sideLength, 0f));
                points[x * 2 + 1, yLength * 2] = new Point(new Vector3(x * sideLength + sideLength / 2, yLength * sideLength, 0f));
            }
            for (int y = 0; y < yLength; y++)
            {
                points[xLength * 2, y * 2] = new Point(new Vector3(xLength * sideLength, y * sideLength, 0f));
                points[xLength * 2, y * 2 + 1] = new Point(new Vector3(xLength * sideLength, y * sideLength + sideLength / 2, 0f));
            }
            points[xLength * 2, yLength * 2] = new Point(new Vector3(xLength * sideLength, yLength * sideLength, 0f));
        }
    }

    private void AddSquares(int[,] pointsValues, Point[,] points, Square[,] squares)
    {
        for (int y = 0; y < yLength; y++)
        {
            for (int x = 0; x < xLength; x++)
            {
                squares[x, y] = new Square();
                squares[x, y].points = new Point[] {
                    points[x * 2, y * 2 + 2],
                    points[x * 2 + 1, y * 2 + 2],
                    points[x * 2 + 2, y * 2 + 2],
                    points[x * 2 + 2, y * 2 + 1],
                    points[x * 2 + 2, y * 2],
                    points[x * 2 + 1, y * 2],
                    points[x * 2, y * 2],
                    points[x * 2, y * 2 + 1],
                };
                squares[x, y].values = new int[] {
                    pointsValues[x, y],
                    pointsValues[x, y + 1],
                    pointsValues[x + 1, y + 1],
                    pointsValues[x + 1, y]
                };
            }
        }
    }
}
