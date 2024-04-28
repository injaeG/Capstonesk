using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public float mouseSensitivity = 100.0f;
    public Transform playerBody;
    private float xRotation = 0.0f;
    private float yRotation = 0.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        yRotation += mouseX;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);
        // 카메라의 위아래 회전
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // 플레이어 몸체의 좌우 회전


        // 플레이어 몸체의 위아래 회전을 추가합니다.
        // 주의: 이는 일반적인 FPS 컨트롤과는 다른 행동을 유발할 수 있습니다.
        // playerBody.Rotate(Vector3.right * mouseY); // 이 코드는 몸체가 이상하게 회전할 수 있으므로 사용하지 않는 것이 좋습니다.
    }
}