using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriverAI : MonoBehaviour
{
    [SerializeField] private Transform targetPositionTransform;
    [SerializeField] private Transform frontTransform; // front 오브젝트를 참조
    [SerializeField] private float rayDistance = 20f;
    [SerializeField] private LayerMask obstacleLayerMask;
    [SerializeField] private LayerMask roadLayerMask;
    [SerializeField] private bool zigzagChase = false; // 좌우로 움직이면서 추격하는 방식 여부

    private CarDriver carDriver;
    private Vector3 targetPosition;
    private bool isReversing = false;
    private float reverseTimer = 0f;
    private float baseReverseDuration = 1f; // 기본 후진 시간
    private bool recentlyReversed = false;
    private float noReverseTimer = 0f;
    private float noReverseDuration = 2f; // 일정 시간 동안 후진 금지
    private float lastTurnAmount = 0f;

    private void Awake()
    {
        carDriver = GetComponent<CarDriver>();
    }

    private void Update()
    {
        if (isReversing)
        {
            Reverse();
            return;
        }

        if (recentlyReversed)
        {
            noReverseTimer -= Time.deltaTime;
            if (noReverseTimer <= 0)
            {
                recentlyReversed = false;
            }
        }

        SetTargetPosition(targetPositionTransform.position);

        float forwardAmount = 0f;
        float turnAmount = 0f;

        float reachedTargetDistance = 15f;
        float distanceToTarget = Vector3.Distance(frontTransform.position, targetPosition); // frontTransform을 기준으로 거리 계산
        float speedFactor = Mathf.Clamp01(carDriver.GetSpeed() / 40f); // 속도에 따라 회전 민감도 조정

        if (distanceToTarget > reachedTargetDistance)
        {
            // 장애물 감지 및 회피
            if (IsObstacleDetected(out float obstacleTurnAmount))
            {
                turnAmount = obstacleTurnAmount;
                forwardAmount = 1f - speedFactor; // 장애물 회피 시 속도 감소
            }
            else
            {
                // 장애물이 없을 때 목표 위치로 이동
                Vector3 dirToMovePosition = (targetPosition - frontTransform.position).normalized; // frontTransform을 기준으로 방향 계산
                float dot = Vector3.Dot(frontTransform.forward, dirToMovePosition);

                if (dot > 0)
                {
                    // 목표가 앞에 있을 때
                    forwardAmount = 1f - speedFactor; // 속도에 따라 전진 속도 조정

                    if (zigzagChase)
                    {
                        turnAmount = Mathf.Sin(Time.time * 5f) * 0.5f; // 좌우로 움직이면서 추격
                    }
                    else
                    {
                        float angleToDir = Vector3.SignedAngle(frontTransform.forward, dirToMovePosition, Vector3.up);
                        float maxTurnAmount = 0.5f; // 최대 회전량을 0.5로 제한
                        turnAmount = Mathf.Clamp(angleToDir / 180f, -maxTurnAmount, maxTurnAmount); // 각도에 비례해 회전량 조정, 최대 회전량을 제한
                    }
                }
                else
                {
                    // 목표가 뒤에 있을 때
                    float reverseDistance = 25f;
                    if (distanceToTarget > reverseDistance)
                    {
                        // 후진하기엔 너무 멀리 있을 때
                        forwardAmount = 1f;
                    }
                    else
                    {
                        forwardAmount = -1f;
                    }

                    float angleToDir = Vector3.SignedAngle(frontTransform.forward, dirToMovePosition, Vector3.up);
                    float maxTurnAmount = 0.5f; // 최대 회전량을 0.5로 제한
                    turnAmount = Mathf.Clamp(angleToDir / 180f, -maxTurnAmount, maxTurnAmount); // 각도에 비례해 회전량 조정, 최대 회전량을 제한
                }
            }
        }
        else
        {
            // 목표 위치에 도달했을 때
            if (carDriver.GetSpeed() > 15f)
            {
                forwardAmount = -1f;
            }
            else
            {
                forwardAmount = 0f;
            }
            turnAmount = 0f;
        }

        carDriver.SetInputs(forwardAmount, turnAmount);
    }

    

    private bool IsObstacleDetected(out float turnAmount)
    {
        turnAmount = 0f;
        int raysCount = 5; // 발사할 레이의 수
        float angle = 60f; // 각도 범위 (좌우 30도)
        bool obstacleDetected = false;

        for (int i = 0; i < raysCount; i++)
        {
            float rayAngle = Mathf.Lerp(-angle, angle, i / (float)(raysCount - 1));
            Vector3 rayDirection = Quaternion.Euler(0, rayAngle, 0) * frontTransform.forward;

            // 레이를 씬에 시각적으로 표시
            Debug.DrawRay(frontTransform.position, rayDirection * rayDistance, Color.red);

            // "wall" 레이어와의 충돌 검사
if (Physics.Raycast(frontTransform.position, rayDirection, out RaycastHit hit, rayDistance, obstacleLayerMask | roadLayerMask))
{
    if ((roadLayerMask.value & (1 << hit.collider.gameObject.layer)) > 0)
    {
        continue; // 만약 충돌한 물체가 'road' 레이어에 속한다면 계속 진행
    }

    obstacleDetected = true;

    // 장애물 회피를 위한 회전 방향 설정
    float angleToObstacle = Vector3.SignedAngle(frontTransform.forward, rayDirection, Vector3.up);
    if (angleToObstacle < 0)
    {
        turnAmount = 1f - (hit.distance / rayDistance); // 오른쪽으로 회전
    }
    else
    {
        turnAmount = -1f + (hit.distance / rayDistance); // 왼쪽으로 회전
    }
}

        }

        return obstacleDetected;
    }

    private void Reverse()
    {
        if (reverseTimer > 0)
        {
            carDriver.SetInputs(-1f, lastTurnAmount); // 후진하면서 회전
            reverseTimer -= Time.deltaTime;
        }
        else if (reverseTimer < 0)
        {
            carDriver.SetInputs(1f, lastTurnAmount); // 전진하면서 회전 (후면 충돌 시)
            reverseTimer += Time.deltaTime;
        }
        else
        {
            isReversing = false;
            recentlyReversed = true;
            noReverseTimer = noReverseDuration;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((obstacleLayerMask.value & (1 << collision.gameObject.layer)) > 0 && !recentlyReversed)
        {
            isReversing = true;
            reverseTimer = baseReverseDuration;

        }
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
