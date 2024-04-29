using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [HideInInspector]
    public float div = 5;
    [HideInInspector]
    public bool stop;

    public GameObject speedometerNeedle; // 계기판 바늘 오브젝트에 대한 참조
    public float maxSpeedometerAngle = -270f; // 바늘이 회전할 수 있는 최대 각도 (예: -270도)
    public float maxSpeed = 200f; // 차량의 최대 속도 (단위 km/h 또는 mph 등 프로젝트에 맞게 조정)
    private Quaternion initialNeedleRotation; // 속도계 바늘의 초기 회전값 저장 변수

    public AudioSource brakeSoundSource; // 브레이크 소리를 재생할 오디오 소스 컴포넌트
    public AudioClip brakeSoundClip; // 브레이크 소리 클립

    public float startTime; // 반복 재생을 시작할 시간 (초 단위)
    public float endTime; // 반복 재생을 끝낼 시간 (초 단위)

    private double nextEventTime;
    private bool isPlaying = false;

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
    private float currentSteeringAngle; // Current rotation angle of the steering wheel
    private const float steeringSmoothTime = 0.1f; // Time to smooth the steering wheel rotation

    public int dir { get { return !reverse ? 1 : -1; } }

    void Start()
    {
        // 핸들 오브젝트의 초기 회전값 저장
        if (steeringWheel != null)
        {
            initialRotation = steeringWheel.transform.localRotation;
        }

        // 속도계 바늘 오브젝트의 초기 회전값 저장
        if (speedometerNeedle != null)
        {
            initialNeedleRotation = speedometerNeedle.transform.localRotation;
        }
    }

    void Update()
    {
        currentSpeed = GetComponent<Rigidbody>().velocity.magnitude * 3.6f; // 현재 속도 업데이트 (m/s에서 km/h로 변환)

        // 속도계 바늘 회전 계산
        float speedPercent = currentSpeed / maxSpeed; // 최대 속도 대비 현재 속도의 비율 계산
        float angle = Mathf.Lerp(0, maxSpeedometerAngle, speedPercent); // 속도 비율에 기반해 최소 및 최대 각도 사이에서 선형 보간

        // 속도계 바늘의 회전 설정
        if (speedometerNeedle != null)
        {
            // y축 기준으로 회전 적용
            speedometerNeedle.transform.localRotation = initialNeedleRotation * Quaternion.Euler(0, angle, 0);
        }

        float speedAdjustedSteeringSpeed = steeringSpeed * (1 + (currentSpeed / maxSpeed));

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

        float targetSteeringAngle = controls.steering * maxSteeringAngle;

        currentSteeringAngle = Mathf.SmoothDamp(currentSteeringAngle, targetSteeringAngle, ref speedAdjustedSteeringSpeed, steeringSmoothTime);

        if (steeringWheel != null)
        {
            // Update the steering wheel rotation based on the smoothed angle
            steeringWheel.transform.localRotation = initialRotation * Quaternion.Euler(0, currentSteeringAngle, 0);
        }


        // 브레이크 소리 재생 로직
        if (controls.handBrake && !brakeSoundSource.isPlaying)
        {
            if (!isPlaying && AudioSettings.dspTime > nextEventTime)
            {
                isPlaying = true;
                brakeSoundSource.PlayScheduled(nextEventTime);
                nextEventTime = AudioSettings.dspTime + (endTime - startTime);
                brakeSoundSource.SetScheduledEndTime(nextEventTime);
            }

            if (AudioSettings.dspTime > nextEventTime)
            {
                nextEventTime += (endTime - startTime);
                isPlaying = false;
            }
        }
        else if (!controls.handBrake && brakeSoundSource.isPlaying)
        {
            brakeSoundSource.Stop();
        }

    }

    IEnumerator SmoothSteeringWheelRotation(float targetAngle)
    {
        float startTime = Time.time;
        float startAngle = currentSteeringAngle;

        while (Time.time - startTime < steeringSmoothTime)
        {
            float t = (Time.time - startTime) / steeringSmoothTime;
            currentSteeringAngle = Mathf.Lerp(startAngle, targetAngle, t);
            steeringWheel.transform.localRotation = initialRotation * Quaternion.Euler(0, currentSteeringAngle, 0);
            yield return null;
        }

        currentSteeringAngle = targetAngle;
        steeringWheel.transform.localRotation = initialRotation * Quaternion.Euler(0, currentSteeringAngle, 0);
    }

}
