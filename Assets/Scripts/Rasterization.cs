using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Rasterization : MonoBehaviour
{
    public GeneratorStartPoint generatorStartPoint;
    public Button button;
    public CellType[,] cells;
    public int height = 100;
    public int width = 100;
    [Range(0f, 1f)]
    public float noizeRation = 0.2f;
    public float cellSideLength;
    public Material highlighted;
    List<GameObject> gameObjects;
    public int cellFilterCycles = 1;
    public int neighbourCountToLive = 3;
    public int neighbourCountToBorn = 5;

    private void Start()
    {
        button.onClick.AddListener(() => Vizualize(generatorStartPoint.rooms));
    }

    public void Update()
    {

    }

    public void Vizualize(List<Room> rooms)
    {
        cells = new CellType[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cells[x, y] = CellType.Dead;
                
            }
        }
        foreach (var room in rooms)
        {
            int xMin = Mathf.FloorToInt(room.bottomLeftCorner.position.x / cellSideLength);
            int xMax = Mathf.CeilToInt((room.bottomRightCorner.position.x) / cellSideLength);
            int yMin = Mathf.FloorToInt((room.bottomLeftCorner.position.y) / cellSideLength);
            int yMax = Mathf.CeilToInt((room.topLeftCorner.position.y) / cellSideLength);
            //Debug.Log($"{xMin} {xMax} {yMin} {yMax} {pointsValues.Length}");
            for (int x = xMin; x < xMax; x++)
            {
                for (int y = yMin; y < yMax; y++)
                {
                    try
                    {
                        cells[x, y] = CellType.StrictlyLive;
                    }
                    catch (System.IndexOutOfRangeException)  // CS0168
                    {
                    }
                }
            }
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (UnityEngine.Random.Range(0f, 1f) < noizeRation)
                {
                    cells[x, y] = CellType.Dead;
                }
            }
        }
        for (int i = 0; i < cellFilterCycles; i++)
        {
            CellFilter();
        }
        if (gameObjects != null)
        foreach (var go in gameObjects)
        {
            Destroy(go);
        }
        gameObjects = new List<GameObject>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var gameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
                gameObject.transform.position = new Vector3(x, y, 0f) * cellSideLength;
                if (cells[x, y] == CellType.Dead)
                {
                    gameObject.GetComponent<MeshRenderer>().material = highlighted;
                }
                gameObjects.Add(gameObject);
            }
        }
    }

    public void CellFilter()
    {
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                int count = 0;
                if (cells[x, y] == CellType.Live || cells[x, y] == CellType.StrictlyLive)
                {
                    ++count;
                }
                if (cells[x, y + 1] == CellType.Live || cells[x, y + 1] == CellType.StrictlyLive)
                {
                    ++count;
                }
                if (cells[x + 1, y + 1] == CellType.Live || cells[x + 1, y + 1] == CellType.StrictlyLive)
                {
                    ++count;
                }
                if (cells[x + 1, y] == CellType.Live || cells[x + 1, y] == CellType.StrictlyLive)
                {
                    ++count;
                }
                if (cells[x + 1, y - 1] == CellType.Live || cells[x + 1, y - 1] == CellType.StrictlyLive)
                {
                    ++count;
                }
                if (cells[x, y - 1] == CellType.Live || cells[x, y - 1] == CellType.StrictlyLive)
                {
                    ++count;
                }
                if (cells[x - 1, y - 1] == CellType.Live || cells[x - 1, y - 1] == CellType.StrictlyLive)
                {
                    ++count;
                }
                if (cells[x - 1, y] == CellType.Live || cells[x - 1, y] == CellType.StrictlyLive)
                {
                    ++count;
                }
                if (cells[x - 1, y + 1] == CellType.Live || cells[x - 1, y + 1] == CellType.StrictlyLive)
                {
                    ++count;
                }
                //Debug.Log("OK" + count);
                if (count >= neighbourCountToBorn && cells[x, y] == CellType.Dead)
                {
                    cells[x, y] = CellType.Live;
                }
                else if(count < neighbourCountToLive && cells[x, y] == CellType.Live)
                {
                    cells[x, y] = CellType.Dead;
                }
            }
        }
    }
}

public enum CellType
{
    Dead, Live, StrictlyDead, StrictlyLive
}

public class Cell
{

} 

