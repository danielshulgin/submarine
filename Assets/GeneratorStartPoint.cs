using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class GeneratorStartPoint : MonoBehaviour
{
    public Button RegenerateButton;
    public Vector2 initialRoomTopLeftCorner;
    public Vector2 initialRoomTopRightBound;
    public Vector2 initialRoomBottomLeftBound;
    public Vector2 initialRoomBottomRightBound;

    [Range(0f, 1f)]
    public float spatialDivisionBottomBound = 0.45f;
    [Range(0f, 1f)]
    public float spatialDivisionTopBound = 0.55f;

    [Range(0f, 1f)]
    public float minRoomMargin = 0.1f;
    [Range(0f, 1f)]
    public float maxRoomMargin = 0.2f;

    public int spatialDivisionIterations = 3;

    private List<IBound> spatialConnections = new List<IBound>();
    public List<Room> rooms = new List<Room>();
    private List<Connection> connections = new List<Connection>();
    private int roomId = 0;


    void Start()
    {
        RegenerateButton.onClick.AddListener(() => { SpatialDivision(spatialDivisionIterations); CreateRoomsWithOffset(); });
    }


    void Update()
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

    public void CreateRoomsWithOffset()
    {
        foreach (var room in rooms)
        {
            float width = (room.topLeftCorner.position.y - room.bottomLeftCorner.position.y);
            float height = (room.topRightCorner.position.x - room.topLeftCorner.position.x);

            float rightOffset = UnityEngine.Random.Range(minRoomMargin, maxRoomMargin) * width;
            float leftOffset = UnityEngine.Random.Range(minRoomMargin, maxRoomMargin) * width;
            float topOffset = UnityEngine.Random.Range(minRoomMargin, maxRoomMargin) * height;
            float bottomOffset = UnityEngine.Random.Range(minRoomMargin, maxRoomMargin) * height;
     
            Corner topLeftCorner = 
                new Corner(new Vector2(room.topLeftCorner.position.x + leftOffset, room.topLeftCorner.position.y - topOffset));
            Corner topRightCorner = 
                new Corner(new Vector2(room.topRightCorner.position.x - rightOffset, room.topRightCorner.position.y - topOffset)); 
            Corner bottomLeftCorner = 
                new Corner(new Vector2(room.bottomLeftCorner.position.x + leftOffset, room.bottomLeftCorner.position.y + bottomOffset));
            Corner bottomRightCorner = 
                new Corner(new Vector2(room.bottomRightCorner.position.x - rightOffset, room.bottomRightCorner.position.y + bottomOffset));

            room.topLeftCorner = topLeftCorner;
            room.topRightCorner = topRightCorner;
            room.bottomLeftCorner = bottomLeftCorner;
            room.bottomRightCorner = bottomRightCorner;
        }
    }

    public void SpatialDivision(int iterations)
    {
        roomId = 0;
        rooms = new List<Room>();
        connections = new List<Connection>();
        Room initilRoom = new Room();
        initilRoom.id = roomId++;
        initilRoom.topLeftCorner = new Corner(initialRoomTopLeftCorner);
        initilRoom.topRightCorner = new Corner(initialRoomTopRightBound);
        initilRoom.bottomLeftCorner = new Corner(initialRoomBottomLeftBound);
        initilRoom.bottomRightCorner = new Corner(initialRoomBottomRightBound);
        rooms.Add(initilRoom);

        for (int i = 0; i < iterations; i++)
        {
            List<Room> roomsToAdd = new List<Room>();
            if (i % 2 == 0)
            {
                foreach (var room in rooms)
                {
                    var spaceRatio = UnityEngine.Random.Range(spatialDivisionBottomBound, spatialDivisionTopBound);
                    roomsToAdd.Add(DivideRoomHorizontal(room, spaceRatio));
                }
            }
            else
            {
                foreach (var room in rooms)
                {
                    var spaceRatio = UnityEngine.Random.Range(spatialDivisionBottomBound, spatialDivisionTopBound);
                    roomsToAdd.Add(DivideRoomVertical(room, spaceRatio));
                }
            }
            foreach (var room in roomsToAdd)
            {
                rooms.Add(room);
            }
        }
        foreach (var room in rooms)
        {
            var neighbourRooms = rooms.FindAll(r =>
            (r.topLeftCorner.position.x == room.topRightCorner.position.x &&
            r.topLeftCorner.position.y <= room.topRightCorner.position.y &&
            r.topLeftCorner.position.y >= room.bottomRightCorner.position.y) ||
            (r.bottomLeftCorner.position.x == room.topRightCorner.position.x &&
            r.bottomLeftCorner.position.y <= room.topRightCorner.position.y &&
            r.bottomLeftCorner.position.y >= room.bottomRightCorner.position.y) ||

            (r.topRightCorner.position.x == room.topLeftCorner.position.x &&
            r.topRightCorner.position.y <= room.topLeftCorner.position.y &&
            r.topRightCorner.position.y >= room.bottomLeftCorner.position.y) ||
            (r.bottomRightCorner.position.x == room.topLeftCorner.position.x &&
            r.bottomRightCorner.position.y <= room.topLeftCorner.position.y &&
            r.bottomRightCorner.position.y >= room.bottomLeftCorner.position.y) ||

            (r.bottomLeftCorner.position.y == room.topRightCorner.position.y &&
            r.bottomLeftCorner.position.x <= room.topRightCorner.position.x &&
            r.bottomLeftCorner.position.x >= room.topLeftCorner.position.x) ||
            (r.bottomLeftCorner.position.y == room.topRightCorner.position.y &&
            r.bottomRightCorner.position.x <= room.topRightCorner.position.x &&
            r.bottomRightCorner.position.x >= room.topLeftCorner.position.x) ||

            (r.topLeftCorner.position.y == room.bottomRightCorner.position.y &&
            r.topLeftCorner.position.x <= room.bottomRightCorner.position.x &&
            r.topLeftCorner.position.x >= room.bottomLeftCorner.position.x) ||
            (r.topLeftCorner.position.y == room.bottomRightCorner.position.y &&
            r.topRightCorner.position.x <= room.bottomRightCorner.position.x &&
            r.topRightCorner.position.x >= room.bottomLeftCorner.position.x));
            if (neighbourRooms != null)
            {
                foreach (var neighbourRoom in neighbourRooms)
                {
                    if (neighbourRoom != room)
                    {
                        Connection connection = new Connection(room, neighbourRoom);
                        if (connections.Find(other => other.Equals(connection)) == null)
                            connections.Add(connection);
                    }
                }
            }
        }
        Debug.Log(connections.Count);
    }

    private Room DivideRoomHorizontal(Room room, float spaceRatio)
    {
        Room secondRoom = new Room();
        secondRoom.id = roomId++;
        Corner leftCorner = new Corner(room.bottomLeftCorner.position +
            spaceRatio * (room.topLeftCorner.position - room.bottomLeftCorner.position));
        Corner rightCorner = new Corner(room.bottomRightCorner.position +
            spaceRatio * (room.topRightCorner.position - room.bottomRightCorner.position));
        secondRoom.bottomLeftCorner = room.bottomLeftCorner;
        secondRoom.bottomRightCorner = room.bottomRightCorner;
        secondRoom.topLeftCorner = leftCorner;
        secondRoom.topRightCorner = rightCorner;

        room.bottomLeftCorner = leftCorner;
        room.bottomRightCorner = rightCorner;
        return secondRoom;
    }

    private Room DivideRoomVertical(Room room, float spaceRatio)
    {
        Room secondRoom = new Room();
        secondRoom.id = roomId++;

        Corner upCorner = new Corner(room.topLeftCorner.position +
            spaceRatio * (room.topRightCorner.position - room.topLeftCorner.position));
        Corner downCorner = new Corner(room.bottomLeftCorner.position +
            spaceRatio * (room.bottomRightCorner.position - room.bottomLeftCorner.position));

        secondRoom.topRightCorner = room.topRightCorner;
        secondRoom.bottomRightCorner = room.bottomRightCorner;
        secondRoom.topLeftCorner = upCorner;
        secondRoom.bottomLeftCorner = downCorner;

        room.topRightCorner = upCorner;
        room.bottomRightCorner = downCorner;
        return secondRoom;
    }
}


public interface IBound
{
    Room FirstRoom { get; set; }
    Room SecondRoom { get; set; }
    Corner FirstCorner { get; set; }
    Corner SecondCorner { get; set; }
}


public class HorizontalBound : IBound
{
    public Room FirstRoom { get => upRooom; set => upRooom = value; }
    public Room SecondRoom { get => bottomRoom; set => bottomRoom = value; }

    public Corner FirstCorner { get => leftCorner; set => leftCorner = value; }
    public Corner SecondCorner { get => rightCorner; set => leftCorner = value; }

    public Room upRooom;
    public Room bottomRoom;
    public Corner leftCorner;
    public Corner rightCorner;
}


public class VerticalBound : IBound
{
    public Room FirstRoom { get => leftRoom; set => leftRoom = value; }
    public Room SecondRoom { get => rightRoom; set => rightRoom = value; }
    public Corner FirstCorner { get => topCorner; set => topCorner = value; }
    public Corner SecondCorner { get => bottomCorner; set => bottomCorner = value; }

    public Room leftRoom;
    public Room rightRoom;
    public Corner topCorner;
    public Corner bottomCorner;
}


public class Room : IEquatable<Room>
{
    public Corner topLeftCorner;
    public Corner topRightCorner;
    public Corner bottomLeftCorner;
    public Corner bottomRightCorner;

    public int id = 0;

    public Vector2 Center
    {
        get => (topLeftCorner.position +
topRightCorner.position + bottomLeftCorner.position + bottomRightCorner.position) * .25f;
    }

    public bool Equals(Room other)
    {
        return id == other.id;
    }
}
public class Connection : IEquatable<Connection>
{
    public Room first;
    public Room second;

    public Connection(Room first, Room second)
    {
        this.first = first;
        this.second = second;
    }

    public bool Equals(Connection other)
    {
        return (other.first.Equals(first)) && (other.second.Equals(second)) || ((other.second.Equals(first)) && (other.first.Equals(second)));
    }
}

public class Corner
{
    public Vector2 position;

    public Corner(Vector2 position)
    {
        this.position = position;
    }
}
