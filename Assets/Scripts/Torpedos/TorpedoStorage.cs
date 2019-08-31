using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoStorage : MonoBehaviour
{
    public static Transform parent;

    private void Awake()
    {
        parent = this.transform;
    }
}
