using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPrefabSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn; // 소환할 프리팹
    private GameObject spawnedPrefab; // 소환된 프리팹
    private bool hasSpawned = false; // 프리팹이 생성되었는지 확인하는 변수

    void Start()
    {
        // 프리팹을 PrefabSpawner 스크립트가 있는 오브젝트의 위치에서 소환합니다.
        if (!hasSpawned)
        {
            spawnedPrefab = Instantiate(prefabToSpawn, this.transform.position, Quaternion.identity);
            spawnedPrefab.transform.parent = this.transform;

            hasSpawned = true; // 프리팹이 생성되었음을 표시
        }
    }
}
