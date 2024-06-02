using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class RoadMaker : MonoBehaviour
{
    public string prefabPath = "PreFabs/Roads"; // 프리팹 경로
    public List<Transform> spawnTransforms; // Inspector에서 편집 가능한 빈 오브젝트 리스트
    private List<GameObject> prefabs;
    private List<int> occupiedIndices; // 이미 프리팹이 생성된 위치 인덱스 리스트

    void Start()
    {
        LoadPrefabs();
        occupiedIndices = new List<int>();
    }

    void LoadPrefabs()
    {
        prefabs = new List<GameObject>();
        GameObject[] loadedPrefabs = Resources.LoadAll<GameObject>(prefabPath);
        foreach (GameObject prefab in loadedPrefabs)
        {
            prefabs.Add(prefab);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            SpawnPrefabAtRandomTransform();
        }
    }

    void SpawnPrefabAtRandomTransform()
    {
        int randomIndex = GetRandomEmptyIndex(); // 빈 위치 인덱스 찾기
        if (randomIndex != -1)
        {
            Transform spawnTransform = spawnTransforms[randomIndex];
            GameObject prefab = prefabs[Random.Range(0, prefabs.Count)]; // 랜덤 프리팹 선택
            Instantiate(prefab, spawnTransform.position, spawnTransform.rotation); // 프리팹 생성
            occupiedIndices.Add(randomIndex); // 생성된 위치 인덱스를 occupiedIndices에 추가
        }
        else
        {
            Debug.Log("No empty points available");
        }
    }

    int GetRandomEmptyIndex()
    {
        List<int> emptyIndices = new List<int>();
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
