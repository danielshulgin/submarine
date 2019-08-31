using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedo : MonoBehaviour
{
    Vector3 torpedoTarget = new Vector3(0,0,-100);
    public float speed = .1f;

    public static int torpedoCounter = 0;
    bool isPlayer = false;

    bool isAlive = true;

    public void Init(Vector3 _target, bool _isPlayer = false)
    {
        torpedoTarget = _target;
        isPlayer = _isPlayer;

        if (!isPlayer)
            torpedoCounter++;
        else
        {
            GameObject bolb = Instantiate(Radar.radar.blobPrefab, this.transform.position, Quaternion.identity, this.transform);
        }
    }

    private void Awake()
    {
        this.transform.SetParent(TorpedoStorage.parent);
        StartCoroutine(Charge());
    }

    IEnumerator Charge()
    {
        yield return new WaitForSeconds(.5f);
        this.GetComponent<Collider2D>().enabled = true;
    }

    private void FixedUpdate()
    {
        if (isAlive)
            this.transform.position = this.transform.position + this.transform.up * speed;

        if (Vector3.Distance(this.transform.position, torpedoTarget) <= .5)
            Explode();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Explode();
    }

    public List<AudioClip> onDestroy;

    void Explode()
    {
        if (onDestroy.Count > 0)
        {
            GameObject go = Instantiate(AudioManager.instance.audioPlayerPrefab, this.transform.position, Quaternion.identity);
            go.GetComponent<AudioSource>().PlayOneShot(onDestroy[Random.Range(0, onDestroy.Count - 1)]);
            Destroy(go.gameObject, 5);
        }

        if (!isPlayer)
            torpedoCounter--;

        isAlive = false;
        GetComponent<Collider2D>().enabled = false;

        if (this.transform.childCount > 0)
            Destroy(this.transform.GetChild(0).gameObject);

        Destroy(this.gameObject);
    }


}
