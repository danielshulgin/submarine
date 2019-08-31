using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Unity;

class MapManager : MonoBehaviour
{
    private MarchingSquaresGenerator marchingSquaresGenerator;

    //TODO
    public GameObject currentGo;
    public Material material;
    //
    public int xLength = 40;
    public int yLength = 40;
    public float sideLength = 6f;
    public int[,] pointsValues;

    public static MapManager instance;

    private void Awake()
    {
        instance = this;
        marchingSquaresGenerator = new MarchingSquaresGenerator(xLength, yLength, sideLength);
        GenerateMapChunk();
    }



    private void GenerateMapChunk()
    {
        if (currentGo != null)
        {
            Destroy(currentGo);
        }
        currentGo = GameObject.CreatePrimitive(PrimitiveType.Quad);
       // currentGo.transform.position = new Vector3(-xLength / 2 * sideLength, -yLength / 2 * sideLength, 0);
        //var collider = currentGo.AddComponent(typeof(EdgeCollider2D)) as PolygonCollider2D;
        float salt = Random.Range(0f, 1f) * 0.1f;
        float perlinNoizeDenominator = 1f / sideLength / 2f + salt;
        pointsValues = RandomGenerator.GenerateValues(xLength, yLength, perlinNoizeDenominator);
        Mesh mesh = marchingSquaresGenerator.Generate((int[,])pointsValues.Clone());
        currentGo.GetComponent<MeshFilter>().mesh = mesh;
        currentGo.GetComponent<MeshRenderer>().material = material;
    }

    public bool PointCollide(Vector3 position, float radius)
    {
        int xMin = Mathf.FloorToInt((position.x - radius) / sideLength);
        int xMax = Mathf.CeilToInt((position.x + radius) / sideLength);
        int yMin = Mathf.FloorToInt((position.y - radius) / sideLength);
        int yMax = Mathf.CeilToInt((position.y + radius) / sideLength);
        //Debug.Log($"{xMin} {xMax} {yMin} {yMax} {pointsValues.Length}");
        for (int x = xMin; x < xMax; x++)
        {
            for (int y = yMin; y < yMax; y++)
            {
                try
                {
                    if (pointsValues[x, y] != 0)
                    {
                        return true;
                    }
                }
                catch (System.IndexOutOfRangeException e)  // CS0168
                {
                    Debug.Log("Out of range point array");
                    return false;
                }
            }
        }

        return false;
    }
    public bool PointDestroy(Vector3 position, float radius)
    {
        int xMin = Mathf.FloorToInt((position.x - radius) / sideLength);
        int xMax = Mathf.CeilToInt((position.x + radius) / sideLength);
        int yMin = Mathf.FloorToInt((position.y - radius) / sideLength);
        int yMax = Mathf.CeilToInt((position.y + radius) / sideLength);
        //Debug.Log($"{xMin * sideLength} {xMax * sideLength}");
        bool result = false;
        for (int x = xMin; x < xMax; x++)
        {
            for (int y = yMin; y < yMax; y++)
            {
                try
                {
                    if (pointsValues[x, y] != 0)
                    {
                        pointsValues[x, y] = 0;
                        result = true;
                    }
                }
                catch (System.IndexOutOfRangeException e)  // CS0168
                {
                    Debug.Log("Out of range point array");
                    return false;
                }
            }
        }
        Mesh mesh = marchingSquaresGenerator.Generate(pointsValues);
        currentGo.GetComponent<MeshFilter>().mesh = mesh;
        return result;
    }
}

