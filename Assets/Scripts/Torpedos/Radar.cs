using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    public static Radar radar;
    public GameObject blobPrefab;

    AudioSource source;
    public AudioClip sonarSound;

    LineRenderer line;
    float angle = 0f;

    private void Awake()
    {
        radar = this;
        line = GetComponent<LineRenderer>();
    }

    public float lineLen = 4.77f;

    private void Start()
    {
        GetComponent<AudioSource>().PlayOneShot(sonarSound);
    }

    void FixedUpdate()
    {
        angle += Time.deltaTime;
        if (angle >= Mathf.PI * 2)
        {
            angle -= Mathf.PI * 2;
            GetComponent<AudioSource>().PlayOneShot(sonarSound);
        }

        Vector3 pos = new Vector3(lineLen * Mathf.Sin(angle), lineLen * Mathf.Cos(angle), 0);
        float newAngle = Mathf.Atan2(pos.y - this.transform.position.y, pos.x - this.transform.position.x) * Mathf.Rad2Deg;
        //print(newAngle);
        //transform.GetChild(0).rotation = new Quaternion((Vector3.forward, Time.deltaTime * Mathf.Deg2Rad);
        transform.GetChild(0).rotation = Quaternion.EulerAngles(0, 0, - angle);// .SetEulerAngles(new Vector3(0, 0, angle));
        line.SetPosition(1, pos);

        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, pos, lineLen * this.transform.lossyScale.y);

        Debug.DrawRay(this.transform.position, pos, Color.red);

        if (hit.collider != null)
        {
            pos = hit.point;

            if (hit.transform.childCount > 0)
            {
                try
                {
                    hit.transform.GetComponentInChildren<Blob>().counter = 500;
                }
                catch
                {

                }
            }
            else
            {
                GameObject bolb = Instantiate(blobPrefab, pos, Quaternion.identity, hit.transform);
            }
        }
    }
}
