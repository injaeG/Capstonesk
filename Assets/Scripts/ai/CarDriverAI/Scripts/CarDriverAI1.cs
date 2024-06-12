using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriverAI1 : MonoBehaviour
{
    [SerializeField] private Transform frontTransform; // 자동차 앞부분의 참조
    [SerializeField] private float rayDistance = 20f; // 레이캐스팅 거리
    [SerializeField] private LayerMask obstacleLayerMask; // 장애물 레이어 마스크
    [SerializeField] private LayerMask roadLayerMask; // 도로 레이어 마스크
    [SerializeField] private bool zigzagChase = false; // 지그재그 이동 활성화
    [SerializeField] private float zigzagFrequency = 5f; // 지그재그 이동 빈도
    [SerializeField] private float stoppingDistance = 15f; // 타겟에서 멈출 거리
    [SerializeField] private float detectionRadius = 1000f; // 타겟 차량을 감지할 반경
    [SerializeField] private float respawnCheckInterval = 5f; // 리스폰 체크 간격
    [SerializeField] private float maxDistanceFromTarget = 50f; // 타겟에서 최대 거리

    private CarDriver carDriver; // CarDriver 컴포넌트 참조
    private Transform targetCarTransform; // 현재 타겟 차량의 트랜스폼
    private Vector3 targetPosition; // 현재 타겟 위치
    private float lastTurnAmount = 0f; // 마지막 회전량 (후진 시 사용)
    private bool isReversing = false; // 후진 플래그
    private VehicleController vehicleController; // VehicleController 컴포넌트 참조

    private void Awake()
    {
        carDriver = GetComponent<CarDriver>();
        vehicleController = GetComponent<VehicleController>(); // VehicleController 참조 가져오기
    }

    private void Start()
    {
        FindNearestTargetCar();
        StartCoroutine(CheckAndRespawn());
    }

    private void Update()
    {
        if (targetCarTransform == null)
        {
            FindNearestTargetCar();
        }

        if (targetCarTransform != null)
        {
            SetTargetPosition(targetCarTransform.position);
        }

        float forwardAmount = 0f;
        float turnAmount = 0f;

        float distanceToTarget = Vector3.Distance(frontTransform.position, targetPosition); // 타겟까지의 거리

        if (distanceToTarget > stoppingDistance)
        {
            // 타겟 방향 계산
            Vector3 dirToMovePosition = (targetPosition - frontTransform.position).normalized;
            float dot = Vector3.Dot(frontTransform.forward, dirToMovePosition);

            if (IsObstacleDetected(out float obstacleTurnAmount))
            {
                turnAmount = obstacleTurnAmount;
                forwardAmount = 0.5f; // 장애물 회피 시 속도 감소
            }
            else
            {
                // 장애물 없음, 타겟으로 이동
                if (dot > 0)
                {
                    forwardAmount = 1f;

                    if (zigzagChase)
                    {
                        turnAmount = Mathf.Sin(Time.time * zigzagFrequency) * 0.5f; // 지그재그 이동
                    }
                    else
                    {
                        float angleToDir = Vector3.SignedAngle(frontTransform.forward, dirToMovePosition, Vector3.up);
                        turnAmount = Mathf.Clamp(angleToDir / 45f, -1f, 1f); // 타겟 각도에 비례한 회전량
                    }
                }
                else
                {
                    // 타겟이 뒤에 있을 경우 후진
                    forwardAmount = -1f;
                    float angleToDir = Vector3.SignedAngle(frontTransform.forward, dirToMovePosition, Vector3.up);
                    turnAmount = Mathf.Clamp(angleToDir / 45f, -1f, 1f);
                }
            }
        }
        else
        {
            // 타겟에 도달, 차를 멈춤
            forwardAmount = 0f;
            turnAmount = 0f;
        }

        // 계산된 값을 CarDriver 컴포넌트에 적용
        carDriver.SetInputs(forwardAmount, turnAmount);
        lastTurnAmount = turnAmount;
    }

    private bool IsObstacleDetected(out float turnAmount)
    {
        turnAmount = 0f;
        int raysCount = 5; // 레이캐스팅 개수
        float angle = 30f; // 레이캐스팅 각도 범위
        bool obstacleDetected = false;

        for (int i = 0; i < raysCount; i++)
        {
            float rayAngle = Mathf.Lerp(-angle, angle, i / (float)(raysCount - 1));
            Vector3 rayDirection = Quaternion.Euler(0, rayAngle, 0) * frontTransform.forward;

            // 씬에서 레이 시각화
            Debug.DrawRay(frontTransform.position, rayDirection * rayDistance, Color.red);

            // 장애물 레이캐스팅
            if (Physics.Raycast(frontTransform.position, rayDirection, out RaycastHit hit, rayDistance, obstacleLayerMask | roadLayerMask))
            {
                if ((roadLayerMask.value & (1 << hit.collider.gameObject.layer)) > 0)
                {
                    continue; // 도로 레이어인 경우 건너뜀
                }

                obstacleDetected = true;

                // 장애물 회피를 위한 회전 방향 결정
                float angleToObstacle = Vector3.SignedAngle(frontTransform.forward, rayDirection, Vector3.up);
                if (angleToObstacle < 0)
                {
                    turnAmount = 1f - (hit.distance / rayDistance); // 우회전
                }
                else
                {
                    turnAmount = -1f + (hit.distance / rayDistance); // 좌회전
                }
            }
        }

        return obstacleDetected;
    }

    private void FindNearestTargetCar()
    {
        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");
        float closestDistance = detectionRadius;
        Transform closestCar = null;

        foreach (GameObject car in cars)
        {
            float distance = Vector3.Distance(transform.position, car.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCar = car.transform;
            }
        }

        if (closestCar != null)
        {
            targetCarTransform = closestCar;
        }
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private IEnumerator CheckAndRespawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnCheckInterval);

            if (!IsOnRoad() || IsFarFromTarget())
            {
                RespawnOnRoad();
            }
        }
    }

    private bool IsOnRoad()
    {
        return Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, roadLayerMask);
    }

    private bool IsFarFromTarget()
    {
        if (targetCarTransform == null) return false;
        float distanceToTarget = Vector3.Distance(transform.position, targetCarTransform.position);
        return distanceToTarget > maxDistanceFromTarget;
    }

    public void RespawnOnRoad()
    {
        // 타겟 차량의 뒤에서 스폰
        Vector3 respawnPosition = FindBehindTargetPosition();
        transform.position = respawnPosition;

        // 타겟을 바라보도록 설정
        if (targetCarTransform != null)
        {
            transform.rotation = Quaternion.LookRotation(targetCarTransform.position - transform.position, Vector3.up);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        }
    }

    private Vector3 FindBehindTargetPosition()
    {
        Vector3 searchCenter = targetCarTransform != null ? targetCarTransform.position : transform.position;
        Vector3 behindPosition = searchCenter - targetCarTransform.forward * 10f; // 타겟 차량 뒤 10미터

        // 높은 위치에서 아래로 레이캐스팅하여 도로 위치 찾기
        behindPosition.y = 1000f;
        RaycastHit hit;
        if (Physics.Raycast(behindPosition, Vector3.down, out hit, Mathf.Infinity, roadLayerMask))
        {
            return hit.point;
        }

        // 도로 위치를 찾지 못한 경우 원래 위치 반환
        return transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == targetCarTransform)
        {
            vehicleController.fuelConsumptionRate -= 20f;
        }
    }
}
