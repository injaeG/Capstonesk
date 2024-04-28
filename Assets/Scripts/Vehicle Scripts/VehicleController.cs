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

    public float steeringSpeed = 1.0f; // 핸들 회전 속도
    private float currentSpeed; // 현재 속도


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
        currentSpeed = GetComponent<Rigidbody>().velocity.magnitude; // 현재 속도 업데이트

        // 속도에 관계없이 일정한 핸들링 속도를 유지
        float speedAdjustedSteeringSpeed = steeringSpeed;

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
            if (Mathf.Abs(controls.steering) > 0)
            {
                // 사용자가 조향 입력을 할 때
                float yRotation = controls.steering * maxSteeringAngle * speedAdjustedSteeringSpeed;
                steeringWheel.transform.localRotation = initialRotation * Quaternion.Euler(0, yRotation, 0);
            }
            else
            {
                // 사용자가 조향 입력을 하지 않을 때 핸들을 초기 위치로 부드럽게 돌려놓기
                steeringWheel.transform.localRotation = Quaternion.Slerp(steeringWheel.transform.localRotation, initialRotation, Time.deltaTime * steeringSpeed);
            }
        }
    }


}
