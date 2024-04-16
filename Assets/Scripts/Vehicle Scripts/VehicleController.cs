using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [HideInInspector]
    public float div = 5; // 숨겨진 변수, 외부에서 접근하지 못하게 함
    [HideInInspector]
    public bool stop; // 차량 정지 상태를 제어하는 불린 변수

    public struct Controls
    {
        public float throttle; // 가속 페달 입력 값
        public float brakes; // 브레이크 페달 입력 값
        public float steering; // 조향 입력 값
        public bool clutch; // 클러치 입력 상태
        public bool handBrake; // 핸드브레이크 입력 상태
    }

    public Controls controls; // 조작 관련 변수를 저장하는 구조체 인스턴스
    public bool reverse; // 차량의 후진 상태를 제어하는 불린 변수

    public GameObject steeringWheel; // 핸들 오브젝트에 대한 참조
    public float maxSteeringAngle = 180f; // 핸들이 회전할 수 있는 최대 각도
    private Quaternion initialRotation; // 핸들 오브젝트의 초기 회전값을 저장할 변수

    public int dir { get { return !reverse ? 1 : -1; } } // 차량의 전진(1) 또는 후진(-1) 상태를 반환하는 프로퍼티

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
        controls.throttle = Mathf.Clamp(Input.GetAxis("Vertical") * dir, 0, 1); // 가속 페달 입력 값 조절
        controls.brakes = -Mathf.Clamp(Input.GetAxis("Vertical") * dir, -1, 0); // 브레이크 페달 입력 값 조절
        controls.steering = Input.GetAxis("Horizontal"); // 조향 입력 값을 받음
        controls.handBrake = Input.GetButton("Jump"); // 핸드브레이크 입력 상태 확인
        controls.clutch = Input.GetButton("Fire1"); // 클러치 입력 상태 확인

        if (stop)
        {
            controls.brakes = 1; // 정지 상태일 경우, 브레이크를 최대로 적용
            controls.throttle = 0; // 가속을 0으로 설정
            controls.steering = 0; // 조향을 0으로 설정
        }

        if (steeringWheel != null)
        {
            float yRotation = controls.steering * maxSteeringAngle; // 조향 입력에 따른 y축 회전값 계산
            // 핸들의 회전을 조절합니다. 초기 회전값에 y축 회전을 조절하여 추가합니다.
            steeringWheel.transform.localRotation = initialRotation * Quaternion.Euler(0, yRotation, 0);
        }
    }
}



