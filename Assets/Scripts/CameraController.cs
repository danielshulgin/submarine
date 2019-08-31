using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class CameraController : MonoBehaviour
{
    [SerializeField] GameObject submarine;
    [Range(0f, 1f)]
    public float lerpPositionK = 0.5f;
    private void LateUpdate()
    {
        //Camera.main.transform.rotation = Quaternion.identity;
        var position = Vector3.Lerp(Camera.main.transform.position,
            submarine.transform.position, lerpPositionK);
        Camera.main.transform.position = new Vector3(position.x, position.y, Camera.main.transform.position.z);
    }
}

