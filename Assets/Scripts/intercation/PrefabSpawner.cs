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
        prefabs = Resources.LoadAll<GameObject>("PreFabs/event");
    }

    void Update()
    {
        if (!hasSpawned) // 프리팹이 아직 생성되지 않았다면
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);

            foreach (var target in targets)
            {
                GameObject prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];

                GameObject spawnedPrefab = Instantiate(prefabToSpawn, target.transform.position + new Vector3(0, 0, 0), Quaternion.identity);
                spawnedPrefab.transform.parent = this.transform;

                hasSpawned = true; // 프리팹이 생성되었음을 표시
            }
        }
    }
}

