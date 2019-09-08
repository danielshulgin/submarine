using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GeneratorStartPoint : MonoBehaviour
{
    public Button regenerateButton;
    public Button rasterizeButton;

    public MapGeneratorArguments args;
    public Material highlighted;

    public List<Room> rooms = new List<Room>();
    private List<Connection> connections = new List<Connection>();
    public CellType[,] cells;
    List<GameObject> gameObjects;


    void Start()
    {
        regenerateButton.onClick.AddListener(() => { Division(); });
        rasterizeButton.onClick.AddListener(() => Vizualize(rooms));
    }


    void Update()
    {
        RoomDebug();
    }

    public void Division()
    {
        var initilRoom = new Room(args.initialRoomTopLeftCorner, args.initialRoomTopRightBound, args.initialRoomBottomLeftBound, args.initialRoomBottomRightBound);
        rooms = SpatialDivision.Divide(args.spatialDivisionIterations, initilRoom, args.spatialDivisionBottomBound, args.spatialDivisionTopBound);
        connections = SpatialDivision.FindConnections(rooms);
        rooms = RoomHandler.CreateRoomsWithOffset(rooms, args.minRoomMargin, args.maxRoomMargin);
    }

    public void Vizualize(List<Room> rooms)
    {
        cells = Rasterization.Rasterize(rooms, args.height, args.width, args.noizeRation, args.cellSideLength);
        int counter = 0;
        foreach (var cell in cells)
        {
            if (cell == CellType.Live || cell == CellType.StrictlyLive)
            {
                counter++;
            }
        }
        Debug.Log(counter / (float)(args.height * args.width));
        cells = CellularAutomaton.CellFilter(args.cellFilterCycles, cells);
        VizualizationDebug();
    }

    private void RoomDebug()
    {
        if (rooms != null && rooms.Count > 0)
        {
            foreach (var room in rooms)
            {
                Debug.DrawLine(room.topLeftCorner.position, room.topRightCorner.position);
                Debug.DrawLine(room.topLeftCorner.position, room.bottomLeftCorner.position);
                Debug.DrawLine(room.bottomRightCorner.position, room.topRightCorner.position);
                Debug.DrawLine(room.bottomLeftCorner.position, room.bottomRightCorner.position);
            }
            foreach (var connection in connections)
            {
                Debug.DrawLine(connection.first.Center, connection.second.Center, Color.red);
            }
        }
    }

    private void VizualizationDebug()
    {
        if (gameObjects != null)
            foreach (var go in gameObjects)
            {
                Destroy(go);
            }
        gameObjects = new List<GameObject>();
        for (int x = 0; x < args.width; x++)
        {
            for (int y = 0; y < args.height; y++)
            {
                var gameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
                gameObject.transform.position = new Vector3(x, y, 0f) * args.cellSideLength;
                if (cells[x, y] == CellType.Live || cells[x, y] == CellType.StrictlyLive)
                {
                    gameObject.GetComponent<MeshRenderer>().material = highlighted;
                }
                gameObjects.Add(gameObject);
            }
        }
    }
}

