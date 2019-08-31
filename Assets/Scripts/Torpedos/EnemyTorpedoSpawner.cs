using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTorpedoSpawner : TorpedoSpawner
{
    //TODO: assign global player variable
    public GameObject target;
    public float radius = 10;
    public float miscalc = 1;

    public float startTorpedoCount = 2;
    float torpedoCount = 4;

    private void Awake()
    {
        Reset();
    }

    public void Reset()
    {
        torpedoCount = startTorpedoCount;
    }

    new void Update()
    {
        if (Torpedo.torpedoCounter < Mathf.Round(torpedoCount))
            SpawnRandom();

        torpedoCount += Time.deltaTime / 10;
    }

    void SpawnRandom()
    {
        float angle = Random.Range(0, 360);
        Vector3 randomStart = target.transform.position + new Vector3(Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius, 0);

        Vector3 miscalcedTarget = target.transform.position + new Vector3(Random.Range(-miscalc, miscalc),
                                                                  Random.Range(-miscalc, miscalc), 
                                                                  0);

        SpawnTorpedo(miscalcedTarget, randomStart);
    }
}
