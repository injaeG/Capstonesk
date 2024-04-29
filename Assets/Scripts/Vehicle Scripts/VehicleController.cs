using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [HideInInspector]
    public float div = 5; // 숨겨진 변수, 외부에서 접근하지 못하게 함
    [HideInInspector]
    public bool stop; // 차량 정지 상태를 제어하는 불린 변수

    public GameObject speedometerNeedle; // 계기판 바늘 오브젝트에 대한 참조
    public float maxSpeedometerAngle = -270f; // 바늘이 회전할 수 있는 최대 각도 (예: -270도)
    public float maxSpeed = 200f; // 차량의 최대 속도 (단위 km/h 또는 mph 등 프로젝트에 맞게 조정)
    private Quaternion initialNeedleRotation; // 속도계 바늘의 초기 회전값 저장 변수
    public float decelerationRate = 1f; // 감속률
    public AudioSource brakeSoundSource; // 브레이크 소리를 재생할 오디오 소스 컴포넌트
    public AudioClip brakeSoundClip; // 브레이크 소리 클립

    public float startTime; // 반복 재생을 시작할 시간 (초 단위)
    public float endTime; // 반복 재생을 끝낼 시간 (초 단위)

    private double nextEventTime;
    private bool isPlaying = false;
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
    public float steeringSpeed = 1.0f; // 핸들 회전 속도
    private float currentSpeed; // 현재 속도
    private float currentSteeringAngle; // Current rotation angle of the steering wheel
    private const float steeringSmoothTime = 0.1f; // Time to smooth the steering wheel rotation

    public GameObject fuelGaugeNeedle; // 연료 계기판 바늘 오브젝트에 대한 참조
    private Quaternion initialFuelNeedleRotation; // 연료 계기판 바늘의 초기 회전값 저장 변수
    public float maxFuel = 100f; // 연료 최대량
    public float fuelConsumptionRate = 1f; // 초당 연료 소모량
    private float fuelAmount; // 현재 연료량
    public float maxFuelGaugeAngle = 70f; // 연료 계기판 바늘이 회전할 수 있는 최대 각도
    private Rigidbody vehicleRigidbody; // 차량의 Rigidbody 컴포넌트

    void Start()
    {
        // 핸들 오브젝트의 초기 회전값을 저장
        if (steeringWheel != null)
        {
            initialRotation = steeringWheel.transform.localRotation;
        }

        // 속도계 바늘 오브젝트의 초기 회전값 저장
        if (speedometerNeedle != null)
        {
            initialNeedleRotation = speedometerNeedle.transform.localRotation;
        }

        if (fuelGaugeNeedle != null)
        {
            initialFuelNeedleRotation = fuelGaugeNeedle.transform.localRotation;
        }

        fuelAmount = maxFuel; // 게임 시작 시 연료량을 최대로 설정
        vehicleRigidbody = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 가져오기

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
        // Smoothly update the steering wheel rotation
        float targetSteeringAngle = controls.steering * maxSteeringAngle;
        currentSteeringAngle = Mathf.SmoothDamp(currentSteeringAngle, targetSteeringAngle, ref speedAdjustedSteeringSpeed, steeringSmoothTime);

        if (steeringWheel != null)
        {
            // Update the steering wheel rotation based on the smoothed angle
            steeringWheel.transform.localRotation = initialRotation * Quaternion.Euler(0, currentSteeringAngle, 0);
        }
        // 연료가 있는 경우에만 throttle, brakes, handBrake, clutch 제어 가능
        if (fuelAmount > 0)
        {
            controls.throttle = Mathf.Clamp(Input.GetAxis("Vertical") * dir, 0, 1);
            controls.brakes = -Mathf.Clamp(Input.GetAxis("Vertical") * dir, -1, 0);
            controls.handBrake = Input.GetButton("Jump");
            controls.clutch = Input.GetButton("Fire1");
        }
        else
        {
            // 연료가 없는 경우, throttle, brakes, handBrake, clutch 기능 비활성화
            controls.throttle = 0;
            controls.brakes = 0;
            controls.handBrake = false;
            controls.clutch = false;
            // 점진적으로 속도 감소
            if (vehicleRigidbody.velocity.magnitude > 0)
            {
                float decelerationRate = 5f; // 적절한 감속률을 설정
                                             // 현재 속도 방향으로 감속을 적용
                Vector3 deceleration = -vehicleRigidbody.velocity.normalized * decelerationRate * Time.deltaTime;
                // 감속 후 속도가 0을 넘지 않도록 체크
                Vector3 nextVelocity = vehicleRigidbody.velocity + deceleration;
                if (nextVelocity.magnitude < deceleration.magnitude)
                {
                    vehicleRigidbody.velocity = Vector3.zero;
                }
                else
                {
                    vehicleRigidbody.velocity = nextVelocity;
                }
            }
        }
        controls.steering = Input.GetAxis("Horizontal");

        if (stop)
        {
            controls.brakes = 1; // 정지 상태일 경우, 브레이크를 최대로 적용
            controls.throttle = 0; // 가속을 0으로 설정
            controls.steering = 0; // 조향을 0으로 설정
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
        UpdateFuelSystem(); // 연료 시스템 업데이트 호출
                            // 플레이어가 "W" 키를 누르고 있지 않다면

        // 모든 경우에 속도에 따른 감속률 조정 적용
        ApplySpeedBasedDeceleration();



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


    IEnumerator FadeOutEngineSoundWithFilters()
    {
        var engines = GameObject.FindGameObjectsWithTag("enginesound");
        float fadeDuration = 2.0f; // 볼륨을 0으로 줄이는데 걸리는 시간 (초)
        float startVolume = 1.0f; // 시작 볼륨. 실제 AudioSource의 볼륨에 따라 조정할 수 있습니다.

        foreach (var engine in engines)
        {
            AudioSource engineAudio = engine.GetComponent<AudioSource>();
            if (engineAudio != null)
            {
                float currentTime = 0;
                while (currentTime < fadeDuration)
                {
                    currentTime += Time.deltaTime;
                    engineAudio.volume = Mathf.Lerp(startVolume, 0, currentTime / fadeDuration);
                    yield return null; // 다음 프레임까지 기다립니다.
                }
                engineAudio.volume = 0; // 마지막으로 볼륨을 확실히 0으로 설정합니다.
                engineAudio.Stop(); // 필요하다면 사운드 재생을 멈출 수 있습니다.
            }
        }
    }


    void UpdateFuelSystem()
    {
        if (fuelAmount > 0 && vehicleRigidbody != null)
        {
            // 연료 소모 계산
            float fuelConsumed = fuelConsumptionRate * vehicleRigidbody.velocity.magnitude * Time.deltaTime;
            fuelAmount -= fuelConsumed;
            fuelAmount = Mathf.Max(fuelAmount, 0); // 연료량이 음수가 되지 않도록 함

            // 연료 계기판 바늘 회전 계산
            float fuelPercent = fuelAmount / maxFuel; // 최대 연료량 대비 현재 연료량의 비율 계산
            float angle = Mathf.Lerp(0, maxFuelGaugeAngle, 1 - fuelPercent); // 연료 비율에 기반해 최소 및 최대 각도 사이에서 선형 보간

            // 연료 계기판 바늘의 회전 설정
            if (fuelGaugeNeedle != null)
            {
                fuelGaugeNeedle.transform.localRotation = initialFuelNeedleRotation * Quaternion.Euler(0, 0, -angle);
            }

            // 바늘이 최대 각도에 도달했을 때 engine 태그를 가진 오브젝트를 꺼줌
            if (Mathf.Approximately(angle, maxFuelGaugeAngle))
            {
                var engines = GameObject.FindGameObjectsWithTag("engine");
                foreach (var engine in engines)
                {
                    engine.SetActive(false); // 오브젝트를 비활성화
                    StartCoroutine(FadeOutEngineSoundWithFilters());
                }

            }


        }
    }

    void ApplySpeedBasedDeceleration()
    {
        float currentSpeed = vehicleRigidbody.velocity.magnitude;
        float maxDecelerationRate = 12f; // 최대 감속률 가정
        float decelerationCoefficient = 0.025f; // 감속 계수 가정

        // currentSpeed의 영향을 줄이기 위해 값을 조정
        float speedFactor = Mathf.Clamp01(currentSpeed * decelerationCoefficient);

        // Mathf.SmoothStep을 사용해 보다 부드러운 감속률 조정 적용
        float adjustedDecelerationRate = Mathf.SmoothStep(decelerationRate, maxDecelerationRate, speedFactor);

        Vector3 deceleration = -vehicleRigidbody.velocity.normalized * adjustedDecelerationRate * Time.deltaTime;

        // 감속 적용
        vehicleRigidbody.velocity += deceleration;
    }
}