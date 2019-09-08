using UnityEngine;

public class Point
{
    public Vector3 position;
    public int triangleIndex = -1;

    public Point(Vector3 position, int triangleIndex)
    {
        this.position = position;
        this.triangleIndex = triangleIndex;
    }

    public Point(Vector3 position)
    {
        this.position = position;
    }
}
