using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EVENTSTICK : MonoBehaviour
{
    public GameObject prefab; // 기본 Prefab
    public GameObject specialPrefab; // 특별 Prefab
    public float spawnProbability = 0.5f; // Prefab을 생성할 확률 (0에서 1 사이)
    private GameObject instantiatedPrefab; // 생성된 Prefab의 참조
    private static GameObject latestSignObject; // 가장 최근의 "표지판생성" 오브젝트
    private RoadMaker roadMaker; // RoadMaker 스크립트 참조

    private int signSpawnCount = 0; // 생성된 표지판 개수 카운트

    void Start()
    {
        // 최근 "표지판생성" 오브젝트 찾기
        FindLatestSignObject();

        // RoadMaker 스크립트 찾기
        FindRoadMaker();

        // Prefab 시도
        TrySpawnPrefab();
    }

    void OnTransformChildrenChanged()
    {
        Debug.Log("자식이 변경되었습니다. 새로운 '표지판생성' 오브젝트를 확인합니다.");

        // 자식이 변경될 때마다 최신의 표지판 오브젝트 재확인
        FindLatestSignObject();

        // Prefab 시도
        TrySpawnPrefab();
    }

    void FindLatestSignObject()
    {
        GameObject[] signObjects = GameObject.FindGameObjectsWithTag("표지판생성");
        Debug.Log($"'표지판생성' 태그를 가진 오브젝트를 발견했습니다: {signObjects.Length}개.");
        if (signObjects.Length > 0)
        {
            latestSignObject = signObjects[signObjects.Length - 1];
            Debug.Log("최신 표지판 오브젝트 발견: " + latestSignObject.name);
        }
        else
        {
            Debug.LogError("'표지판생성' 태그를 가진 오브젝트를 찾을 수 없습니다.");
        }
    }

    void FindRoadMaker()
    {
        roadMaker = FindObjectOfType<RoadMaker>();
        if (roadMaker == null)
        {
            Debug.LogError("RoadMaker 스크립트를 찾을 수 없습니다.");
        }
    }

    void TrySpawnPrefab()
    {
        // 최신 "표지판생성" 오브젝트가 존재하는지 확인
        if (latestSignObject != null)
        {
            // 0에서 1 사이의 랜덤 값 생성
            float randomValue = Random.Range(0f, 1f);
            Debug.Log($"생성된 랜덤 값: {randomValue}");

            // 생성 확률보다 랜덤 값이 작은지 확인
            if (randomValue < spawnProbability)
            {
                // 기본 Prefab을 최신 표지판 오브젝트의 위치에 기본 회전으로 생성
                instantiatedPrefab = Instantiate(prefab, latestSignObject.transform.position, prefab.transform.rotation, latestSignObject.transform);
                Debug.Log("Prefab 생성됨.");

                // Prefab 모니터링 코루틴 시작
                StartCoroutine(MonitorPrefab());

                // 표지판 생성 카운트 증가
                signSpawnCount++;

                // RoadMaker 스크립트 중지 조건
                if (signSpawnCount >= 4 && roadMaker != null)
                {
                    roadMaker.enabled = false;
                    Debug.Log("RoadMaker 스크립트가 중지되었습니다.");
                }

                // 5개의 표지판이 생성되면 특별 Prefab 생성
                if (signSpawnCount >= 5)
                {
                    InstantiateSpecialPrefab();
                }
            }
            else
            {
                Debug.Log("확률 검사에 의해 Prefab이 생성되지 않았습니다.");
            }
        }
        else
        {
            Debug.LogError("'표지판생성' 태그를 가진 최신 오브젝트를 찾을 수 없습니다.");
        }
    }

    IEnumerator MonitorPrefab()
    {
        while (true)
        {
            // 생성된 Prefab이 파괴되었는지 확인
            if (instantiatedPrefab == null)
            {
                Debug.Log("Prefab이 파괴되었습니다. 다시 생성을 시도합니다.");
                // 다시 Prefab 생성을 시도
                TrySpawnPrefab();
                yield break; // 코루틴 종료
            }
            // 다시 확인하기 전에 한 프레임 기다림
            yield return null;
        }
    }

    void InstantiateSpecialPrefab()
    {
        // "Road_Spawn_Place" 태그를 가진 오브젝트 찾기
        GameObject roadSpawnPlace = GameObject.FindGameObjectWithTag("Road_Spawn_Place");
        
        if (roadSpawnPlace != null)
        {
            // 특별 Prefab을 "Road_Spawn_Place" 오브젝트의 위치와 회전으로 생성
            GameObject instance = Instantiate(specialPrefab, roadSpawnPlace.transform.position, roadSpawnPlace.transform.rotation);
            Debug.Log("특별 Prefab 생성됨.");

            // 특별 Prefab 생성 후 표지판 생성 카운트 초기화
            signSpawnCount = 0;
        }
        else
        {
            Debug.LogError("'Road_Spawn_Place' 태그를 가진 오브젝트를 찾을 수 없습니다.");
        }
    }
}
