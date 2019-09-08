using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Unity;
using System;

class MapManager : MonoBehaviour
{
    public int radiusOfActiveChunks = 5;
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
    public MapGeneratorArguments args;

    public static MapManager instance;

    private void Awake()
    {
        realChunkLenght = xChunkLength * sideLength;
        realMapXLenght = realChunkLenght * xMapLength;
        realMapYLenght = realChunkLenght * yMapLength;
        instance = this;
        marchingSquaresGenerator = new MarchingSquaresGenerator(xChunkLength, yChunkLength, sideLength);
        FirstGeneration();
    }





    private void AddNoize(CellType[,] initialCells)
    {

        for (int row = 0; row <= initialCells.GetLength(1) - 1; row++)
        {
            for (int column = 0; column <= initialCells.GetLength(0) - 1; column++)
            {
                if(initialCells[column, row] == CellType.Live && (UnityEngine.Random.Range(0f, 1f) > 0.7f)){
                    initialCells[column, row] = CellType.Dead;
                }
            }
        }
    }

    private void FirstGeneration()
    {
        mapChunks = new MapChunk[xMapLength, yMapLength];

        var initialRoom = new Room(new Vector2(0f, 100f),
            new Vector2(100f, 100f),
            new Vector2(0f, 0f),
            new Vector2(100f, 0f));
        var rooms = SpatialDivision.Divide(args.spatialDivisionIterations, initialRoom, args.spatialDivisionBottomBound, args.spatialDivisionTopBound);
        rooms = RoomHandler.CreateRoomsWithOffset(rooms, args.minRoomMargin, args.maxRoomMargin);

        var cells = Rasterization.Rasterize(rooms, 100, 100, args.noizeRation, 0.9f);
        cells = CellularAutomaton.CellFilter(args.cellFilterCycles, cells);
        //cells = Mugnifier.Impose(cells, (yMapLength + 1) * yChunkLength, (xMapLength + 1) * xChunkLength);
        //cells = CellularAutomaton.CellFilter(2, cells);
        cells = Mugnifier.Impose(cells, (yMapLength + 1) * yChunkLength, (xMapLength + 1) * xChunkLength);
        AddNoize(cells);
        cells = CellularAutomaton.CellFilter(1, cells);
        int[,] values = CellularAutomaton.ConvertCellToPointArray(cells);

        for (int x = 0; x < xMapLength; x++)
        {
            for (int y = 0; y < yMapLength; y++)
            {

                //TODO
                //float salt = UnityEngine.Random.Range(0f, 1f) * 0.1f;
                //float perlinNoizeDenominator = 1f / sideLength / 2f + salt;
                int[,] chunkPointsValues = new int[xChunkLength + 1, yChunkLength + 1];
                    //RandomGenerator.GenerateValues(xChunkLength, yChunkLength, perlinNoizeDenominator);
                for (int xi = 0; xi < x + xChunkLength + 1; xi++)
                {
                    for (int yi = 0; yi < y + yChunkLength + 1; yi++)
                    {
                        try
                        {
                            chunkPointsValues[xi, yi] = values[xi + x * (xChunkLength), yi + y * (yChunkLength)];
                        }
                        catch (System.IndexOutOfRangeException)  // CS0168
                        {
                        }
                    }
                }
                Coordinates chunkCoordinates = new Coordinates(x, y);
                /*GameObject chunkGameObject = GenerateMapChunk(chunkPointsValues);
                chunkGameObject.SetActive(true);
                chunkGameObject.transform.position = GetPositionFromChunkCoordinates(chunkCoordinates);*/

                mapChunks[x, y] =
                    new MapChunk(chunkPointsValues, chunkCoordinates, null);
                //
            }
        }
    }

    Renderer rend;

    //void Start()
    //{
    //    rend = GetComponent<Renderer>();

    //    // Use the Specular shader on the material
    //    rend.material.shader = Shader.Find("Echolocation");
    //}
    public float value = 0f;
    private void Update()
    {
        MapChunk nextMapChunk = GetChunkFromPosition(submarine.transform.position);
        if (currentChunk != nextMapChunk)
        {
            //TODO
            currentChunk = nextMapChunk;
            RegenerateMap(GetChunksInRadius(nextMapChunk.coordinates.x, nextMapChunk.coordinates.y, radiusOfActiveChunks));
        }
        //rend.material.SetFloat("Radius", value);
        value += Time.deltaTime * 7;
        if (value > 15f)
        {
            value = 0f;
        }
    }

    private MapChunk GetChunkFromPosition(Vector3 position)
    {
        int x = Mathf.CeilToInt(position.x / (realChunkLenght));
        int y = Mathf.CeilToInt(position.y / (realChunkLenght));
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
            //activeChunks[coordinate].chunkGameObject.SetActive(false);
            Destroy(activeChunks[coordinate].chunkGameObject);
            activeChunks.Remove(coordinate);
        }

        foreach (var coordinate in nextValues)
        {
            if (!activeChunks.ContainsKey(coordinate))
            {
                activeChunks[coordinate] = GetChunkFromCoordinates(coordinate.x, coordinate.y);
                activeChunks[coordinate].chunkGameObject = GenerateMapChunk(activeChunks[coordinate].pointsValues);
                activeChunks[coordinate].chunkGameObject.transform.position = GetPositionFromChunkCoordinates(coordinate);
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
        currentGo.AddComponent<Test>();
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

