using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadMaker : MonoBehaviour
{
    public string prefabPath = "PreFabs/Roads"; // 프리팹 경로
    public List<Transform> spawnTransforms; // 생성 위치 리스트
    private List<GameObject> prefabs; // 로드 프리팹들
    private List<int> occupiedIndices; // 이미 생성된 위치 인덱스 리스트
    private GameObject world; // 월드 오브젝트 (부모로 설정)

    // 추가
    public GameObject eventPrefabGasStation; // 가스 스테이션 이벤트 프리팹
    public int gasStationFrequency = 5; // 가스 스테이션 이벤트 등장 빈도
    public GameObject eventPrefabFog; // 안개 이벤트 프리팹
    public GameObject prefabSign; // 표지판 프리팹

    void Start()
    {
        LoadPrefabs(); // 프리팹 로드
        occupiedIndices = new List<int>(); // 이미 생성된 위치 초기화
        world = GameObject.Find("월드-------------------------"); // "World" 오브젝트를 이름으로 찾기

        if (world == null)
        {
            Debug.LogError("World object not found!"); // 월드 오브젝트를 찾을 수 없을 때 오류 메시지 출력
        }
    }

    void LoadPrefabs()
    {
        prefabs = new List<GameObject>(); // 프리팹 리스트 초기화
        GameObject[] loadedPrefabs = Resources.LoadAll<GameObject>(prefabPath); // 리소스에서 모든 프리팹 로드
        foreach (GameObject prefab in loadedPrefabs)
        {
            prefabs.Add(prefab); // 로드된 프리팹들을 리스트에 추가
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car")) // 자동차와 충돌했을 때
        {
            SpawnPrefabAtRandomTransform(); // 랜덤한 위치에 프리팹 생성
        }
    }

    void SpawnPrefabAtRandomTransform()
    {
        int randomIndex = GetRandomEmptyIndex(); // 빈 위치 인덱스 찾기
        if (randomIndex != -1) // 빈 위치를 찾았을 경우
        {
            Transform spawnTransform = spawnTransforms[randomIndex]; // 생성 위치 선택
            GameObject prefab;

            // 가스 스테이션 이벤트를 랜덤하게 생성하는 로직
            if (Random.Range(1, gasStationFrequency + 1) == 1)
            {
                prefab = eventPrefabGasStation; // 가스 스테이션 이벤트 프리팹 선택
            }
            else
            {
                prefab = prefabs[Random.Range(0, prefabs.Count)]; // 랜덤 프리팹 선택
            }

            GameObject instance = Instantiate(prefab, spawnTransform.position, spawnTransform.rotation); // 프리팹 생성

            if (world != null)
            {
                instance.transform.SetParent(world.transform); // 월드 오브젝트의 자식으로 설정
            }

            occupiedIndices.Add(randomIndex); // 생성된 위치 인덱스를 리스트에 추가

            // 생성된 프리팹이 표지판인 경우 안개 이벤트를 다음에 생성
            if (prefab == prefabSign) // 표지판 프리팹이 생성된 경우
            {
                if (randomIndex < spawnTransforms.Count - 1)
                {
                    Transform nextSpawnTransform = spawnTransforms[randomIndex + 1]; // 다음 생성 위치 선택
                    Instantiate(eventPrefabFog, nextSpawnTransform.position, eventPrefabFog.transform.rotation); // 안개 이벤트 생성
                }
                else
                {
                    Debug.Log("No next spawn point available for fog event"); // 다음 생성 위치가 없는 경우 메시지 출력
                }
            }
        }
        else
        {
            Debug.Log("No empty points available"); // 빈 위치가 없을 때 메시지 출력
        }
    }

    int GetRandomEmptyIndex()
    {
        List<int> emptyIndices = new List<int>(); // 빈 위치 인덱스 리스트
        for (int i = 0; i < spawnTransforms.Count; i++)
        {
            if (!occupiedIndices.Contains(i))
            {
                emptyIndices.Add(i); // 비어 있는 위치 인덱스 리스트에 추가
            }
        }

        if (emptyIndices.Count > 0)
        {
            return emptyIndices[Random.Range(0, emptyIndices.Count)]; // 랜덤한 빈 위치 인덱스 반환
        }
        else
        {
            return -1; // 빈 위치가 없을 때
        }
    }
}