using UnityEngine;

public class Room 
{
    public Corner topLeftCorner;
    public Corner topRightCorner;
    public Corner bottomLeftCorner;
    public Corner bottomRightCorner;

    public Vector2 Center
    {
        get => (topLeftCorner.position 
            + topRightCorner.position
            + bottomLeftCorner.position
            + bottomRightCorner.position)
            * .25f;
    }

    public Room(
        Corner topLeftCorner, 
        Corner topRightCorner, 
        Corner bottomLeftCorner, 
        Corner bottomRightCorner)
    {
        this.topLeftCorner = topLeftCorner;
        this.topRightCorner = topRightCorner;
        this.bottomLeftCorner = bottomLeftCorner;
        this.bottomRightCorner = bottomRightCorner;
    }

    public Room(
        Vector2 initialRoomTopLeftCorner,
        Vector2 initialRoomTopRightBound,
        Vector2 initialRoomBottomLeftBound,
        Vector2 initialRoomBottomRightBound)
    {
        this.topLeftCorner = new Corner(initialRoomTopLeftCorner);
        this.topRightCorner = new Corner(initialRoomTopRightBound);
        this.bottomLeftCorner = new Corner(initialRoomBottomLeftBound);
        this.bottomRightCorner = new Corner(initialRoomBottomRightBound);
    }
    public Room()
    {
    }
}

