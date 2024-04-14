using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public GameObject planePrefab;
    public Transform microbus;
    public GameObject cube;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("cube"))
        {
            Debug.Log("Microbus and cube collided!");
        }
    }

    void SpawnNewPlane()
    {
        // 현재 plane의 위치에서 새로운 위치 계산
        Vector3 nextRoadPosition = CalculateNextRoadPosition();

        // 새로운 planePrefab을 생성하여 도로로 사용
        Instantiate(planePrefab, nextRoadPosition, Quaternion.identity);
    }

    Vector3 CalculateNextRoadPosition()
    {
        // cube 오브젝트의 위치를 기준으로 다음 도로 위치 계산
        Vector3 hitPoint = cube.transform.position;
        Vector3 direction = (hitPoint - microbus.transform.position).normalized;
        float distance = 10f; // 예시 거리
        Vector3 nextRoadPosition = hitPoint + direction * distance;
        return nextRoadPosition;
    }
}