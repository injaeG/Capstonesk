using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public float baseMouseSensitivity = 100.0f;
    public float mouseSensitivity;
    public Transform playerBody;
    private float xRotation = 0.0f;
    private float yRotation = 0.0f;
    public float targetFrameRate = 60.0f; // 목표 프레임레이트
    public float minXRotation = -70f;
    public float maxXRotation = 30f;
    public float minYRotation = -110f;
    public float maxYRotation = 110f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Calculate the mouse sensitivity based on the screen resolution
        float referenceResolutionWidth = 1920f;
        float referenceResolutionHeight = 1080f;
        float currentResolutionWidth = Screen.width;
        float currentResolutionHeight = Screen.height;
        float widthRatio = currentResolutionWidth / referenceResolutionWidth;
        float heightRatio = currentResolutionHeight / referenceResolutionHeight;
        mouseSensitivity = baseMouseSensitivity * (widthRatio + heightRatio) / 2;

        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        yRotation += mouseX;

        // Use the public variables to clamp the rotation values
        xRotation = Mathf.Clamp(xRotation, minXRotation, maxXRotation);
        yRotation = Mathf.Clamp(yRotation, minYRotation, maxYRotation);
    

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}