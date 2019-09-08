using System.Collections.Generic;

public static class SpatialDivision
{
    //TODO in optimal way

    public static List<Connection> FindConnections(List<Room> rooms)
    {
        var resultConnections = new List<Connection>();
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
                        if (resultConnections.Find(other => other.Equals(connection)) == null)
                            resultConnections.Add(connection);
                    }
                }
            }
        }
        return resultConnections;
    }

    public static List<Room> Divide(int iterations, Room initialRoom, float spatialDivisionBottomBound, float spatialDivisionTopBound)
    {
        var rooms = new List<Room>();
        rooms.Add(initialRoom);

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
        return rooms;
    }

    private static Room DivideRoomHorizontal(Room room, float spaceRatio)
    {
        Room secondRoom = new Room();

        Corner leftCorner = new Corner(room.bottomLeftCorner.position
            + spaceRatio * (room.topLeftCorner.position - room.bottomLeftCorner.position));
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

    private static Room DivideRoomVertical(Room room, float spaceRatio)
    {
        Room secondRoom = new Room();

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

