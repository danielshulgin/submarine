using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MeshGenerator
{
    public int currentIndex = 0;
    public List<int> meshIndexes;
    public List<Point> meshPoints;

    public void StartGeneration()
    {
        meshIndexes = new List<int>();
        meshPoints = new List<Point>();
        currentIndex = 0;
    }

    /// <summary>
    /// Point array length must be multiple of 3  
    /// </summary>
    public void AddDataFromPoints(Point[] points)
    {
        if(points != null)
        {
            for (int i = 2; i < points.Length; i += 3)
            {
                //Check if alredy added vertix
                if (points[i - 2].triangleIndex == -1)
                { 
                    points[i - 2].triangleIndex = meshPoints.Count;
                    meshPoints.Add(points[i - 2]);
                }
                meshIndexes.Add(points[i - 2].triangleIndex);

                if (points[i - 1].triangleIndex == -1)
                { 
                    points[i - 1].triangleIndex = meshPoints.Count;
                    meshPoints.Add(points[i - 1]);
                }
                meshIndexes.Add(points[i - 1].triangleIndex);

                if (points[i].triangleIndex == -1)
                {
                    points[i].triangleIndex = meshPoints.Count;
                    meshPoints.Add(points[i]);
                }
                meshIndexes.Add(points[i].triangleIndex);
            }
        }
    }

    public Mesh EndGeneration()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = meshPoints.Select(point => point.position).ToArray();
        mesh.uv = meshPoints.Select(v3 => 
            new Vector2(v3.position.x / meshPoints[meshPoints.Count -1 ].position.x, 
            v3.position.y / meshPoints[meshPoints.Count - 1].position.y)).ToArray();

        mesh.triangles = meshIndexes.ToArray();

        return mesh;
    }

    public static Mesh GenerateTestMeshSquare()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] indeses = new int[6];

        vertices[0] = new Vector3(0, 1);
        vertices[1] = new Vector3(0, 0);
        vertices[2] = new Vector3(1, 1);
        vertices[3] = new Vector3(1, 0);

        uv[0] = new Vector2(0, 1);
        uv[1] = new Vector2(0, 0);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);

        indeses[0] = 2;
        indeses[1] = 1;
        indeses[2] = 0;
        indeses[3] = 3;
        indeses[4] = 1;
        indeses[5] = 2;
        
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = indeses;
        return mesh;
    }
}
