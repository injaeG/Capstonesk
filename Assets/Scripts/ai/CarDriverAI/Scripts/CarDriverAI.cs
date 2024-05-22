using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriverAI : MonoBehaviour
{
    [SerializeField] private Transform targetPositionTransform;
    [SerializeField] private Transform frontTransform; // front 오브젝트를 참조
    [SerializeField] private float rayDistance = 20f;
    [SerializeField] private LayerMask obstacleLayerMask;
    [SerializeField] private bool zigzagChase = false; // 좌우로 움직이면서 추격하는 방식 여부

    private CarDriver carDriver;
    private Vector3 targetPosition;
    private bool isReversing = false;
    private float reverseTimer = 0f;
    private float baseReverseDuration = 0.25f; // 기본 후진 시간
    private int collisionCount = 0;

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
                        turnAmount = Mathf.Sin(Time.time * 2f) * 0.5f; // 좌우로 움직이면서 추격
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
                }

                float angleToDir = Vector3.SignedAngle(frontTransform.forward, dirToMovePosition, Vector3.up);
                float maxTurnAmount = 0.5f; // 최대 회전량을 0.5로 제한
                turnAmount = Mathf.Clamp(angleToDir / 180f, -maxTurnAmount, maxTurnAmount); // 각도에 비례해 회전량 조정, 최대 회전량을 제한
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

        for (int i = 0; i < raysCount; i++)
        {
            float rayAngle = Mathf.Lerp(-angle, angle, i / (raysCount - 1f));
            Vector3 rayDirection = Quaternion.Euler(0, rayAngle, 0) * frontTransform.forward;
            if (Physics.Raycast(frontTransform.position, rayDirection, out RaycastHit hit, rayDistance, obstacleLayerMask))
            {
                CreateMarkerAtHitLocation(hit.point); // 충돌 위치에 마커 생성

                if (rayAngle < 0)
                {
                    turnAmount = Mathf.Max(turnAmount, 1f - (hit.distance / rayDistance));
                }
                else
                {
                    turnAmount = Mathf.Min(turnAmount, -1f + (hit.distance / rayDistance));
                }
                return true;
            }
        }

        return false;
    }

    // 충돌 위치에 작은 구체를 생성하여 마커로 사용하는 함수
void CreateMarkerAtHitLocation(Vector3 position)
{
    GameObject marker = new GameObject("Marker"); // 빈 게임 오브젝트 생성
    marker.transform.position = position;

    // 시각적으로 표시하기 위해 Sphere Collider 추가
    SphereCollider sphereCollider = marker.AddComponent<SphereCollider>();
    sphereCollider.isTrigger = true; // 필요에 따라 Trigger로 설정
    sphereCollider.radius = 1f; // 반지름 설정

    // 필요한 경우 추가적인 컴포넌트 구성

    Destroy(marker, 1f); // 1초 후에 마커 삭제
}


    private void Reverse()
    {
        if (reverseTimer > 0)
        {
            carDriver.SetInputs(-1f, 0f); // 후진
            reverseTimer -= Time.deltaTime;
        }
        else if (reverseTimer < 0)
        {
            carDriver.SetInputs(1f, 0f); // 전진 (후면 충돌 시)
            reverseTimer += Time.deltaTime;
        }
        else
        {
            isReversing = false;
        }
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
