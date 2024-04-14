using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [HideInInspector]
    public float div = 5;
    [HideInInspector]
    public bool stop;

    public struct Controls
    {
        public float throttle;
        public float brakes;
        public float steering;
        public bool clutch;
        public bool handBrake;
    }

    public Controls controls;
    public bool reverse;

    public GameObject steeringWheel; // 핸들 오브젝트에 대한 참조
    public float maxSteeringAngle = 180f; // 핸들이 회전할 수 있는 최대 각도
    private Quaternion initialRotation; // 핸들 오브젝트의 초기 회전값을 저장할 변수

    public int dir { get { return !reverse ? 1 : -1; } }

    void Start()
    {
        // 핸들 오브젝트의 초기 회전값을 저장
        if (steeringWheel != null)
        {
            initialRotation = steeringWheel.transform.localRotation;
        }
    }

    void Update()
    {
        controls.throttle = Mathf.Clamp(Input.GetAxis("Vertical") * dir, 0, 1);
        controls.brakes = -Mathf.Clamp(Input.GetAxis("Vertical") * dir, -1, 0);
        controls.steering = Input.GetAxis("Horizontal");
        controls.handBrake = Input.GetButton("Jump");
        controls.clutch = Input.GetButton("Fire1");

        if (stop)
        {
            controls.brakes = 1;
            controls.throttle = 0;
            controls.steering = 0;
        }

        if (steeringWheel != null)
        {
            float yRotation = controls.steering * maxSteeringAngle; // 조향 입력에 따른 y축 회전값 계산
            // 핸들의 회전을 조절합니다. 초기 회전값에 y축 회전을 조절하여 추가합니다.
            steeringWheel.transform.localRotation = initialRotation * Quaternion.Euler(0, yRotation, 0);
        }
    }
}



