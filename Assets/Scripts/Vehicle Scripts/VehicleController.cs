using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [HideInInspector]
    public float div = 5;
    [HideInInspector]
    public bool stop;
    public GameOverScreenController gameOverScreenController;
    private bool isGameOverCreated = false;
    public GameObject speedometerNeedle;
    public float maxSpeedometerAngle = -270f;
    public float maxSpeed = 200f;
    private Quaternion initialNeedleRotation;
    public float decelerationRate = 1f;
    public AudioSource brakeSoundSource;
    public AudioClip brakeSoundClip;

    public float startTime;
    public float endTime;

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

    public GameObject steeringWheel;
    public float maxSteeringAngle = 180f;
    private Quaternion initialRotation;

    public int dir { get { return !reverse ? 1 : -1; } }
    public float steeringSpeed = 1.0f;
    private float currentSpeed;
    private float currentSteeringAngle;
    private const float steeringSmoothTime = 0.1f;

    public GameObject fuelGaugeNeedle;
    private Quaternion initialFuelNeedleRotation;
    public float maxFuel = 100f;
    public float fuelConsumptionRate = 1f;
    public float fuelAmount;
    public float maxFuelGaugeAngle = 70f;
    private Rigidbody vehicleRigidbody;

    private float currentSpeedometerAngle = 0f;
    private float speedometerSmoothVelocity = 0f;

    public float gravityScale = 2.0f; // 중력의 강도를 설정합니다. 기본값은 1입니다.

    public EventPrefabSpawner eventController;

    public EVENTSTICK eventstick;

    void Start()
    {
        if (steeringWheel != null)
        {
            initialRotation = steeringWheel.transform.localRotation;
        }

        if (speedometerNeedle != null)
        {
            initialNeedleRotation = speedometerNeedle.transform.localRotation;
        }

        if (fuelGaugeNeedle != null)
        {
            initialFuelNeedleRotation = fuelGaugeNeedle.transform.localRotation;
        }

        fuelAmount = maxFuel;
        vehicleRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {

        float speedAdjustedSteeringSpeed = steeringSpeed * (1 + (currentSpeed / maxSpeed));
        float targetSteeringAngle = controls.steering * maxSteeringAngle;
        currentSteeringAngle = Mathf.SmoothDamp(currentSteeringAngle, targetSteeringAngle, ref speedAdjustedSteeringSpeed, steeringSmoothTime);

        if (steeringWheel != null)
        {
            steeringWheel.transform.localRotation = initialRotation * Quaternion.Euler(0, currentSteeringAngle, 0);
        }

        if (fuelAmount > 0)
        {
            controls.throttle = Mathf.Clamp(Input.GetAxis("Vertical") * dir, 0, 1);
            controls.brakes = -Mathf.Clamp(Input.GetAxis("Vertical") * dir, -1, 0);
            controls.handBrake = Input.GetButton("Jump");
            controls.clutch = Input.GetButton("Fire1");
        }
        else
        {
            controls.throttle = 0;
            controls.brakes = 0;
            controls.handBrake = false;
            controls.clutch = false;

            if (vehicleRigidbody.velocity.magnitude > 0)
            {
                float decelerationRate = 5f;
                Vector3 deceleration = -vehicleRigidbody.velocity.normalized * decelerationRate * Time.deltaTime;
                Vector3 nextVelocity = vehicleRigidbody.velocity + deceleration;
                if (nextVelocity.magnitude < deceleration.magnitude)
                {
                    vehicleRigidbody.velocity = Vector3.zero;
                    if (!isGameOverCreated)
                    {
                        gameOverScreenController.TriggerGameOver();
                        isGameOverCreated = true;
                    }
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
            controls.brakes = 1;
            controls.throttle = 0;
            controls.steering = 0;
        }

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
        UpdateFuelSystem();

        ApplySpeedBasedDeceleration();

        currentSpeed = vehicleRigidbody.velocity.magnitude * 3.6f;

        float speedPercent = currentSpeed * 0.2f / maxSpeed;
        float targetAngle = Mathf.Lerp(0, maxSpeedometerAngle, speedPercent);

        if (speedometerNeedle != null)
        {
            currentSpeedometerAngle = Mathf.SmoothDampAngle(currentSpeedometerAngle, targetAngle, ref speedometerSmoothVelocity, 0.2f); // 부드러운 시간을 0.5초로 설정
            speedometerNeedle.transform.localRotation = initialNeedleRotation * Quaternion.Euler(0, currentSpeedometerAngle, 0);
        }
    }
    void FixedUpdate()
    {
        // 중력에 강도(scale factor)를 적용하여 원하는 강도로 설정합니다.
        Vector3 gravity = gravityScale * Physics.gravity;
        vehicleRigidbody.AddForce(gravity, ForceMode.Acceleration);
    }

    IEnumerator SmoothSteeringWheelRotation(float targetAngle)
    {
        float startTime = Time.time;
        float startAngle = currentSteeringAngle;
        float adjustedSteeringSmoothTime = steeringSmoothTime * 10;

        while (Time.time - startTime < adjustedSteeringSmoothTime)
        {
            float t = (Time.time - startTime) / adjustedSteeringSmoothTime;
            float acceleration = Mathf.Abs(targetAngle - startAngle) / maxSteeringAngle;
            t = t * acceleration;

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
        float fadeDuration = 2.0f;
        float startVolume = 1.0f;

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
                    yield return null;
                }
                engineAudio.volume = 0;
                engineAudio.Stop();
            }
        }
    }

    void UpdateFuelSystem()
    {
        if (fuelAmount > 0) // 연료가 0보다 큰 경우에만 연료 소비
        {
            float fuelConsumed = fuelConsumptionRate * vehicleRigidbody.velocity.magnitude * Time.deltaTime;
            fuelAmount -= fuelConsumed;
            fuelAmount = Mathf.Max(fuelAmount, 0);

            float fuelPercent = fuelAmount / maxFuel;
            float angle = Mathf.Lerp(0, maxFuelGaugeAngle, 1 - fuelPercent);

            if (fuelGaugeNeedle != null)
            {
                fuelGaugeNeedle.transform.localRotation = initialFuelNeedleRotation * Quaternion.Euler(0, 0, -angle);
            }

            if (Mathf.Approximately(angle, maxFuelGaugeAngle))
            {
                var engines = GameObject.FindGameObjectsWithTag("engine");
                foreach (var engine in engines)
                {
                    engine.SetActive(false);
                    StartCoroutine(FadeOutEngineSoundWithFilters());
                }
            }
        }
    }

    void ApplySpeedBasedDeceleration()
    {
        Vector3 horizontalVelocity = new Vector3(vehicleRigidbody.velocity.x, 0, vehicleRigidbody.velocity.z);
        float currentSpeed = horizontalVelocity.magnitude;
        float maxDecelerationRate = 15f;
        float decelerationCoefficient = 0.025f;
        float speedFactor = Mathf.Clamp01(currentSpeed * decelerationCoefficient);
        float adjustedDecelerationRate = Mathf.SmoothStep(0, maxDecelerationRate, speedFactor);
        Vector3 deceleration = -horizontalVelocity.normalized * adjustedDecelerationRate * Time.deltaTime;

        if (Mathf.Approximately(Input.GetAxis("Vertical"), 0))
        {
            vehicleRigidbody.velocity = new Vector3(vehicleRigidbody.velocity.x + deceleration.x, vehicleRigidbody.velocity.y, vehicleRigidbody.velocity.z + deceleration.z);
        }
    }

    private Coroutine timerCoroutine;

    bool ishitchtrigger = false;

    bool iseventtrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Road_Make"))
        {
            if (!iseventtrigger)
            {
                // 충돌한 객체의 부모 객체를 찾습니다.
                Transform parentTransform = other.transform.parent;

                Debug.Log(parentTransform.gameObject.name);

                if (parentTransform != null)
                {
                    if (parentTransform.gameObject.name == "Event_PreFab_GasStation" || parentTransform.gameObject.name == "Event_Prefab_Fog")
                        return;
                    iseventtrigger = true;
                    eventstick.Spawnstick(parentTransform);
                }
            }
        }

        if (other.CompareTag("Destory"))
        {
            iseventtrigger = false;


        }

        if (other.CompareTag("Game_Over")) // 도로 구역을 벗어났을 때
        {
            Debug.Log("gameover");

            fuelAmount = 0f;

            gameOverScreenController.TriggerGameOver();
        }

        if (other.CompareTag("eye_ghost"))
        {
            Debug.Log("eye_ghost 닿음");

            eventController.EyeGhostDestroy(other);

            fuelAmount -= 5f;
        }

        if (other.CompareTag("hitchstop"))
        {
            if (!ishitchtrigger)
            {
                ishitchtrigger = true;
                // 트리거에 들어올 때 타이머 코루틴 시작
                timerCoroutine = StartCoroutine(ExecuteAfterDelay(2f)); // 2초 후 실행
            }
        }

        //if (other.CompareTag("SlenderMan"))
        //{
        //    Debug.Log("SlenderMan 닿음");

        //    Destroy(other.gameObject);

        //    fuelAmount -= 20f;
        //}
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("hitchstop"))
        {
            if (ishitchtrigger)
            {
                ishitchtrigger = false;
                // 트리거에서 나갈 때 코루틴 중단
                if (timerCoroutine != null)
                {
                    StopCoroutine(timerCoroutine);
                    timerCoroutine = null;
                }
            }
        }
    }


    IEnumerator ExecuteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정된 시간 동안 대기
        ExecuteEvent(); // 지연 후 메서드 실행
    }

    void ExecuteEvent()
    {
        // 이벤트 실행 코드
        Debug.Log("2초 동안 머물러 이벤트 실행!");

        eventController.HitchDestroy();
    }
}