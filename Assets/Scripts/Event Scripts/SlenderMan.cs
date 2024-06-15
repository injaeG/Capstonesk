using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlenderMan : MonoBehaviour
{
    public GameObject prefab; // Instantiate할 프리팹
    public float spawnProbability = 0.5f; // 프리팹이 생성될 확률 (0에서 1 사이)

    public void SpawnEventPrefab(Transform parentTransform)
    {
        Debug.Log("SlenderMan");

        // 부모 객체의 자식 객체 중 이름이 '슬렌더'인 객체를 찾습니다.
        Transform childTransform = parentTransform.Find("슬렌더");

        // 자식 객체가 존재하는지 확인
        if (childTransform != null)
        {
            Instantiate(prefab, childTransform.position, prefab.transform.rotation);

            //// 0과 1 사이의 랜덤 값을 생성
            //float randomValue = Random.Range(0f, 1f);

            //// 랜덤 값이 생성 확률보다 작은지 확인
            //if (randomValue < spawnProbability)
            //{
            //    // 프리팹을 자식 객체의 위치에 프리팹의 기본 회전 상태로 생성
            //    Instantiate(prefab, childTransform.position, prefab.transform.rotation);
            //}
        }
        else
        {
            Debug.LogError("자식 객체 '슬렌더'를 찾을 수 없습니다.");
        }
    }
}

