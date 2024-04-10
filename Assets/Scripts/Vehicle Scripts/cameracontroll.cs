using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameracontroll : MonoBehaviour
{
    public float LookSensitivityX = 1f;
    public float LookSensitivityY = 1f;
    public float MinYLookAngle = -90;
    public float MaxYLookAngle = 90;
    public float MinXLookAngle = -130;
    public float MaxXLookAngle = 130;
    public Transform PlayerCamera;
    private float verticalRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerCamera != null)
        {
            float mouseX = Input.GetAxis("Mouse X") * LookSensitivityX;
            float mouseY = Input.GetAxis("Mouse Y") * LookSensitivityY;

            verticalRotation -= mouseY;
            verticalRotation -= mouseX;
            

            PlayerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
    }
}
