using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField]
    private float horizontalSpeed = 10f;
    [SerializeField]
    private float verticalSpeed = 10f;
    [SerializeField]
    private float clampAngle = 80f;

    private Vector3 startingRotation;

    protected override void Awake()
    {
        base.Awake();
        // 카메라의 초기 회전 값을 저장합니다.
        startingRotation = transform.localRotation.eulerAngles;
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                Vector2 deltaInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                startingRotation.x += deltaInput.x * horizontalSpeed * deltaTime;
                startingRotation.y -= deltaInput.y * verticalSpeed * deltaTime; // y 축 회전은 일반적으로 마우스 이동의 반대 방향입니다.
                startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);

                // Quaternion.Euler는 y, x, z 순서로 회전을 적용합니다.
                state.RawOrientation = Quaternion.Euler(startingRotation.y, startingRotation.x, 0f);
            }
        }
    }
}
