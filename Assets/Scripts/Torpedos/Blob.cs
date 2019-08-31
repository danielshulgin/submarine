using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour
{
    SpriteRenderer rend;

    void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        StartCoroutine(Decay());
    }

    public int counter = 500;

    IEnumerator Decay()
    {
        while (counter > 0)
        {
            counter -= 1;
            Color c = rend.color;
            c.a = counter / 500f;
            rend.color = c;

            yield return null;
        }

        Destroy(this.gameObject);
    }
}
