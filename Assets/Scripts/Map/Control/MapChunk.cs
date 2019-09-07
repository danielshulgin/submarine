using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MapChunk
{
    public int[,] pointsValues;
    public Coordinates coordinates;
    public GameObject chunkGameObject;

    public MapChunk(int[,] pointsValues, Coordinates coordinates, GameObject chunkGameObject)
    {
        this.pointsValues = pointsValues;
        this.coordinates = coordinates;
        this.chunkGameObject = chunkGameObject;
    }
}

