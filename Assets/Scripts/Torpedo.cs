using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedo : MonoBehaviour
{
    Vector3 torpedoTarget = new Vector3(0,0,-100);
    float speed = .1f;

    public static int torpedoCounter = 0;

    

    public void Init(Vector3 _target)
    {
        torpedoTarget = _target;
        torpedoCounter++;
    }

    private void FixedUpdate()
    {
        this.transform.position = this.transform.position + this.transform.up * speed;

        if (Vector3.Distance(this.transform.position, torpedoTarget) <= .5)
            Explode();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Explode();
    }

    void Explode()
    {
        torpedoCounter--;
        print("Boom!");
        Destroy(this.gameObject);
    }


}
