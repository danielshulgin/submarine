using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public float moveSpeed = 18f;
    // The target marker.
    Transform target;

    // Angular speed in radians per sec.
    public float speed = 1f;
    public bool speedDebuff = false;

    void Update()
    {
        Vector3 nextPosition = new Vector3(
            transform.position.x + Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed,
            transform.position.y + Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed,
            0f);
        if (nextPosition != transform.position && !MapManager.instance.PointDestroy(nextPosition, 3f))
        {
            gameObject.transform.position = nextPosition;

            Vector3 targetDir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);

            float step = speed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.up, targetDir, step, 0.0f);

            var angle = Mathf.Atan2(newDir.y, newDir.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {
            if (!speedDebuff)
            {
                speedDebuff = true;
                StartCoroutine(SpeedDebuff());
            }
        }
    }
    IEnumerator SpeedDebuff()
    {
        float prevMoveSpeed = moveSpeed;
        moveSpeed = 0f;
        int counter = 6;
        for (int i = 1; i <= counter; i++)
        {
            moveSpeed = (prevMoveSpeed * i) / counter;
            yield return new WaitForSeconds(0.1f);
        }
        speedDebuff = false;
    }
}
