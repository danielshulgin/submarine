using System.Collections.Generic;
using UnityEngine;

public static class RoomHandler
{
    //TODO with height/width ratio
    public static List<Room> CreateRoomsWithOffset(List<Room> rooms, float minRoomMargin, float maxRoomMargin)
    {
        var result = new List<Room>();
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
            result.Add(new Room(topLeftCorner, topRightCorner, bottomLeftCorner, bottomRightCorner));
        }
        return result;
    }
}

