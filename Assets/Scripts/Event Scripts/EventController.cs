using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPrefabSpawner : MonoBehaviour
{
    private GameObject[] eventPrefabs; // PreFabs/event 디렉토리에 있는 모든 프리팹
    public Transform spawnPoint; // 빈 오브젝트 위치 (Ghost 태그가 있을 경우)

    void Start()
    {
        // PreFabs/event 디렉토리에 있는 모든 프리팹을 로드합니다.
        eventPrefabs = Resources.LoadAll<GameObject>("PreFabs/event");
        // 이벤트 태그가 있는 프리팹을 찾아서 처리합니다.
        foreach (GameObject taggedPrefab in GameObject.FindGameObjectsWithTag("event"))
        {
            SpawnRandomEventPrefab();
        }
    }

    void SpawnRandomEventPrefab()
    {
        if (eventPrefabs.Length == 0) return;

        int index = Random.Range(0, eventPrefabs.Length);
        GameObject selectedPrefab = eventPrefabs[index];

        // 'ghost' 태그가 있는 경우
        if (selectedPrefab.CompareTag("ghost"))
        {
            Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity, spawnPoint);
        }
        else
        {
            // 'road' 태그가 있는 모든 오브젝트를 찾습니다.
            GameObject[] roadObjects = GameObject.FindGameObjectsWithTag("road");

            // 'road' 태그가 있는 오브젝트가 없는 경우, 함수를 종료합니다.
            if (roadObjects.Length == 0) return;

            // 'road' 태그가 있는 오브젝트 중에서 랜덤으로 하나를 선택합니다.
            GameObject randomRoadObject = roadObjects[Random.Range(0, roadObjects.Length)];

            // 선택한 오브젝트의 위치에서 프리팹을 생성합니다.
            Instantiate(selectedPrefab, randomRoadObject.transform.position, Quaternion.identity);
        }
    }
}