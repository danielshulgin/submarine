﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoSpawner : MonoBehaviour
{

    public GameObject torpedoPrefab;

    //TODO: call SpawnTorpedo method as player!
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = 0;
            SpawnTorpedo(target);
        }
    }

    protected void SpawnTorpedo(Vector3 _target)
    {
        SpawnTorpedo(_target, this.transform.position);
    }

    protected void SpawnTorpedo(Vector3 _target, Vector3 _start)
    {
        float angle = Mathf.Atan2(_target.y - _start.y, _target.x - _start.x) * Mathf.Rad2Deg;

        //print(_start + " " + _target + " " + angle);

        GameObject torpedo = GameObject.Instantiate(torpedoPrefab);
        torpedo.transform.position = _start;
        torpedo.transform.RotateAround(_start, Vector3.forward, angle + 180);

        torpedo.GetComponent<Torpedo>().Init(_target);
    }

}
