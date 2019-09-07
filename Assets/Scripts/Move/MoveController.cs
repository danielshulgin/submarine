using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveController : MonoBehaviour
{
    public float moveSpeed = 18f;
    // The target marker.
    Transform target;

    // Angular speed in radians per sec.
    public float speed = 1f;
    public float angularSpeed = 10f;
    public float maxSpeed = 1f;
    public float maxAngularSpeed = 1f;
    public bool speedDebuff = false;
    private Rigidbody2D SubmarineRigidbody2D;

    private void Awake()
    {
        SubmarineRigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        SubmarineRigidbody2D.AddForce(transform.up * moveDirection.magnitude * Time.deltaTime * moveSpeed);
        if (SubmarineRigidbody2D.velocity.magnitude > 0f)
        {
            var fromAngle = Mathf.Atan2(transform.up.y, transform.up.x) * Mathf.Rad2Deg - 90;
            var toAngle = fromAngle;
            if (moveDirection.magnitude != 0f)
            {
                toAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90;
            }
            //Debug.Log($"{fromAngle} {toAngle}");
            //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            //Debug.Log(Vector2.Angle(new Vector2(transform.up.x, transform.up.y), moveDirection));
            SubmarineRigidbody2D.AddTorque((toAngle - fromAngle) * Time.deltaTime * angularSpeed);
        }
        SubmarineRigidbody2D.angularVelocity = Mathf.Clamp(SubmarineRigidbody2D.angularVelocity, -maxAngularSpeed, maxAngularSpeed);

        if (SubmarineRigidbody2D.velocity.magnitude > maxSpeed)
        {
            SubmarineRigidbody2D.velocity = maxSpeed * SubmarineRigidbody2D.velocity.normalized;
        }
        /*Vector3 nextPosition = new Vector3(
            transform.position.x + Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed,
            transform.position.y + Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed,
            0f);
        if (nextPosition != transform.position *//*&& !MapManager.instance.PointDestroy(nextPosition, 3f)*//*)
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
        }*/
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
