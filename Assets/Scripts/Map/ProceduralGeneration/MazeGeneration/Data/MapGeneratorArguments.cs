using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "MapArguments", menuName = "MapGeneratorArguments", order = 1)]
public class MapGeneratorArguments : ScriptableObject
{
    public Vector2 initialRoomTopLeftCorner = new Vector2(0, 100);
    public Vector2 initialRoomTopRightBound = new Vector2(100, 100);
    public Vector2 initialRoomBottomLeftBound = new Vector2(0, 0);
    public Vector2 initialRoomBottomRightBound = new Vector2(100, 0);

    [Range(0f, 1f)]
    public float spatialDivisionBottomBound = 0.45f;
    [Range(0f, 1f)]
    public float spatialDivisionTopBound = 0.55f;

    [Range(0f, 1f)]
    public float minRoomMargin = 0.1f;
    [Range(0f, 1f)]
    public float maxRoomMargin = 0.2f;

    public int spatialDivisionIterations = 3;

    [Range(0f, 1f)]
    public float noizeRation = 0.2f;
    public int cellFilterCycles = 1;

    public int height = 100;
    public int width = 100;
    public float cellSideLength = 1.2f;
}

