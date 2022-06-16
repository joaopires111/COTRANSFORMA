using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public Transform PlayerTransform;
    private Vector3 _cameraOffset;

    public float SmoothFactor = 0.2f;
    public bool LookAtPlayer = true;
    public float RotationsSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        LookAtPlayer = true;
        _cameraOffset = transform.position - PlayerTransform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Quaternion camTurnAngleX =
                Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationsSpeed, Vector3.up);
            Quaternion camTurnAngleY =
                Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * RotationsSpeed, Vector3.left);
            _cameraOffset = camTurnAngleX * _cameraOffset;
            _cameraOffset = camTurnAngleY * _cameraOffset;
        }

        Vector3 newPos = PlayerTransform.position + _cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);

        if(LookAtPlayer || Input.GetMouseButton(0))
            transform.LookAt(PlayerTransform);
        
    }

}
