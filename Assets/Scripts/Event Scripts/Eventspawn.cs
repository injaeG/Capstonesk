using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public string eventTag = "event";
    public string carTag = "Car";
    public GameObject[] prefabs; // Inspector에서 프리팹을 직접 할당
    private bool hasSpawned = false; // 프리팹이 이미 생성되었는지 확인하는 변수
    private List<Vector3> spawnedPositions = new List<Vector3>(); // 생성된 프리팹 위치를 기록하는 리스트

    void Start()
    {
        // 아무 작업도 필요 없습니다.
    }

    void Update()
    {
        if (!hasSpawned) // 프리팹이 아직 생성되지 않았다면
        {
            GameObject[] eventTargets = GameObject.FindGameObjectsWithTag(eventTag);
            GameObject[] carTargets = GameObject.FindGameObjectsWithTag(carTag);

            if (eventTargets.Length == 0 || carTargets.Length == 0)
            {
                return; // 이벤트 타겟 또는 카 타겟이 없는 경우
            }

            GameObject farthestEventTarget = null;
            float maxDistance = float.MinValue;

            foreach (var eventTarget in eventTargets)
            {
                foreach (var carTarget in carTargets)
                {
                    float distance = Vector3.Distance(eventTarget.transform.position, carTarget.transform.position);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        farthestEventTarget = eventTarget;
                    }
                }
            }

            if (farthestEventTarget != null)
            {
                Vector3 spawnPosition = farthestEventTarget.transform.position + Vector3.up * 10f;

                // 이미 생성된 위치인지 확인합니다.
                if (!spawnedPositions.Contains(spawnPosition))
                {
                    // 프리팹 배열에서 랜덤하게 하나를 선택합니다.
                    GameObject prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];

                    // 선택한 프리팹을 타겟 오브젝트의 위치에 생성합니다.
                    Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

                    // 생성된 위치를 기록합니다.
                    spawnedPositions.Add(spawnPosition);

                    hasSpawned = true; // 프리팹이 생성되었음을 표시
                }
            }
        }
    }

    // 특정 조건에 따라 프리팹을 다시 생성할 수 있도록 하는 메소드
    public void ResetSpawner()
    {
        hasSpawned = false;
    }
}
