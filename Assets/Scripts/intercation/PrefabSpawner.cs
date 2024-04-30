using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public string targetTag = "event";
    private GameObject[] prefabs;
    private bool hasSpawned = false; // 프리팹이 이미 생성되었는지 확인하는 변수

    void Start()
    {
        // "PreFabs/event" 경로에서 모든 프리팹을 불러옵니다.
        prefabs = Resources.LoadAll<GameObject>("PreFabs/event");
    }

    void Update()
    {
        if (!hasSpawned) // 프리팹이 아직 생성되지 않았다면
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);

            foreach (var target in targets)
            {
                // 프리팹 배열에서 랜덤하게 하나를 선택합니다.
                GameObject prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];

                if (prefabToSpawn.name == "ghost")
                {
                    // 'ghost' 프리팹인 경우, 대상 오브젝트의 위치에서 생성합니다.
                    Instantiate(prefabToSpawn, this.transform.position, Quaternion.identity, this.transform);
                }
                else
                {
                    // 그 외의 경우, 현재 스크립트가 부착된 오브젝트의 위치에서 생성합니다.
                    // 해당 라인은 필요 없으므로 제거합니다: GameObject spawnedPrefab;
                    GameObject spawnedPrefab = prefabToSpawn;
                }

                hasSpawned = true; // 프리팹이 생성되었음을 표시
                break; // 한 번 생성 후 반복문을 빠져나옵니다.
            }
        }
    }
}
