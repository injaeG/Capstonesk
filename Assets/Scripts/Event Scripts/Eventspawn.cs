using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
public string targetTag = "event";
public GameObject[] prefabs; // Inspector에서 프리팹을 직접 할당
private bool hasSpawned = false; // 프리팹이 이미 생성되었는지 확인하는 변수


void Start()
{
    // 아무 작업도 필요 없습니다.
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

            // 선택한 프리팹을 타겟 오브젝트의 위치에 생성합니다.
                Instantiate(prefabToSpawn, target.transform.position + Vector3.up * 10f, Quaternion.identity);

            hasSpawned = true; // 프리팹이 생성되었음을 표시
            break; // 한 번 생성 후 반복문을 빠져나옵니다.
        }
    }
}
}