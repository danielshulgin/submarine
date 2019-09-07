using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Unity;
using System;

class MapManager : MonoBehaviour
{
    private MarchingSquaresGenerator marchingSquaresGenerator;

    //TODO
    public Material material;
    //
    #region Metrics
    public int xChunkLength = 40;
    public int yChunkLength = 40;

    public int xMapLength = 40;
    public int yMapLength = 40;

    public float realChunkLenght;

    public float realMapXLenght;
    public float realMapYLenght;

    public float sideLength = 6f;
    #endregion

    public MapChunk[,] mapChunks;
    public Dictionary<Coordinates, MapChunk> activeChunks = new Dictionary<Coordinates, MapChunk>();

    public MapChunk currentChunk;
    //TODO
    public GameObject submarine;

    public static MapManager instance;

    private void Awake()
    {
        realChunkLenght = xChunkLength * sideLength;
        realMapYLenght = realChunkLenght * xMapLength;
        realMapXLenght = realChunkLenght * yMapLength;
        instance = this;
        marchingSquaresGenerator = new MarchingSquaresGenerator(xChunkLength, yChunkLength, sideLength);
        FirstGeneration();
    }

    private void FirstGeneration()
    {
        mapChunks = new MapChunk[xMapLength, yMapLength];
        for (int x = 0; x < xMapLength; x++)
        {
            for (int y = 0; y < yMapLength; y++)
            {
                
                //TODO
                float salt = UnityEngine.Random.Range(0f, 1f) * 0.1f;
                float perlinNoizeDenominator = 1f / sideLength / 2f + salt;
                int[,] pointsValues = 
                    RandomGenerator.GenerateValues(xChunkLength, yChunkLength, perlinNoizeDenominator);
                Coordinates chunkCoordinates = new Coordinates(x, y);
                GameObject chunkGameObject = GenerateMapChunk(pointsValues);
                chunkGameObject.SetActive(false);
                chunkGameObject.transform.position = GetPositionFromChunkCoordinates(chunkCoordinates);
        
                mapChunks[x, y] = 
                    new MapChunk(pointsValues, chunkCoordinates, chunkGameObject);
                //
            }
        }
    }

    private void Update()
    {
        MapChunk nextMapChunk = GetChunkFromPosition(submarine.transform.position);
        if (currentChunk != nextMapChunk)
        {
            //TODO
            currentChunk = nextMapChunk;
            RegenerateMap(GetChunksInRadius(nextMapChunk.coordinates.x, nextMapChunk.coordinates.y, 7));
        }
        
    }

    private MapChunk GetChunkFromPosition(Vector3 position)
    {
        int x = Mathf.CeilToInt( position.x / (realChunkLenght));
        int y = Mathf.CeilToInt( position.y / (realChunkLenght));
        return GetChunkFromCoordinates(x, y);
    }

    public Vector3 GetPositionFromChunkCoordinates(Coordinates coordinates)
    {
        return new Vector3(coordinates.x * realChunkLenght, coordinates.y * realChunkLenght, 0f);
    }

    private MapChunk GetChunkFromCoordinates(int x, int y)
    {
        if (x < 0 || y < 0 || x >= xMapLength || y >= yMapLength)
        {
            return mapChunks[0, 0];
        }
        return mapChunks[x, y];
    }

    private void RegenerateMap(List<Coordinates> nextValues)
    {
        List<Coordinates> coordinatseToRemove = new List<Coordinates>();
        foreach (var coordinate in activeChunks.Keys)
        {
            if (!nextValues.Contains(coordinate))
            {  
                coordinatseToRemove.Add(coordinate);
            }
        }

        foreach (var coordinate in coordinatseToRemove)
        {
            activeChunks[coordinate].chunkGameObject.SetActive(false);
            activeChunks.Remove(coordinate);
        }

        foreach (var coordinate in nextValues)
        {
            if (!activeChunks.ContainsKey(coordinate))
            {
                activeChunks[coordinate] = GetChunkFromCoordinates(coordinate.x, coordinate.y);
                activeChunks[coordinate].chunkGameObject.SetActive(true);
            }
        }
    }

    private List<Coordinates> GetChunksInRadius(int xChunkCoordinate, int yChunkCoordinate, int radius)
    {
        var result = new List<Coordinates>();
        int xMin = xChunkCoordinate - radius;
        int xMax = xChunkCoordinate + radius;
        int yMin = yChunkCoordinate - radius;
        int yMax = yChunkCoordinate + radius;
        for (int x = xMin; x < xMax; x++)
        {
            for (int y = yMin; y < yMax; y++)
            {
                result.Add(new Coordinates(x, y));
            }
        }
        return result;
    }

    private GameObject GenerateMapChunk(int[,] pointValues)
    {
        GameObject currentGo = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Mesh mesh = marchingSquaresGenerator.Generate(pointValues);
        currentGo.GetComponent<MeshFilter>().mesh = mesh;
        currentGo.GetComponent<MeshRenderer>().material = material;
        return currentGo;
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
                    if (currentChunk.pointsValues[x, y] != 0)
                    {
                        return true;
                    }
                }
                catch (System.IndexOutOfRangeException)  // CS0168
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
                    if (currentChunk.pointsValues[x, y] != 0)
                    {
                        currentChunk.pointsValues[x, y] = 0;
                        result = true;
                    }
                }
                catch (System.IndexOutOfRangeException)  // CS0168
                {
                    Debug.Log("Out of range point array");
                    return false;
                }
            }
        }
        Mesh mesh = marchingSquaresGenerator.Generate(currentChunk.pointsValues);
        currentChunk.chunkGameObject.GetComponent<MeshFilter>().mesh = mesh;
        return result;
    }
}

