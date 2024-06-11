using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeGhost : MonoBehaviour
{
    public AudioClip eye_ghostSound;

   // private int count = eventPrefabs.Length;

    void EyeGhostSpawnEventPrefab(GameObject[] eventPrefabs)
    {
        Debug.Log("eye_ghost");

        audioSource.clip = eye_ghostSound;

        if (eventPrefabs.Length == 1) return;

        GameObject[] selectedPrefabs = new GameObject[8];

        for (int i = 0; i < 8; i++) // 인덱스를 0부터 시작하도록 수정
        {
            do
            {
                int index = Random.Range(0, eventPrefabs.Length);
                selectedPrefabs[i] = eventPrefabs[index];
            } while (selectedPrefabs[i] == null || !selectedPrefabs[i].CompareTag("eye_ghost")); // 조건 수정
        }

        GameObject[] roadObjects = GameObject.FindGameObjectsWithTag("Road");
        if (roadObjects.Length == 0) return;

        int count = -1;

        for (int j = 0; j < 4; j++)
        {
            GameObject randomRoadObject = roadObjects[Random.Range(0, roadObjects.Length)];

            instances = new GameObject[selectedPrefabs.Length];

            for (int k = 0; k < 2; k++)
            {
                count++;
                if (count >= selectedPrefabs.Length) break;

                Vector3 randomPosition = new Vector3(
                    Random.Range(randomRoadObject.transform.position.x - 17, randomRoadObject.transform.position.x + 17),
                    1.9f,
                    Random.Range(randomRoadObject.transform.position.z - 10, randomRoadObject.transform.position.z + 10)
                );

                Vector3 spawnPosition = FindRoadPositionNearby();
                instances[count] = Instantiate(selectedPrefabs[count], spawnPosition, Quaternion.identity);
                instantiateAndDestroyCoroutine = StartCoroutine(InstantiateAndPlayAudio(instances[count]));
            }
        }
    }

    public void EyeGhostDestroy(Collider other)
    {
        if (other.CompareTag("Game_Over"))
        {
            Destroy(other.gameObject); // 오타 수정 및 정확한 파라미터 전달
        }

        foreach (GameObject inst in instances)
        {
            if (inst == null)
            {
                // 객체가 비어있으면 발생시킬 이벤트 추가
                Debug.Log("눈알 귀신이 없습니다.");
                // 혹은 다른 이벤트를 발생시킬 수 있습니다.
                eventcheckaudio.clip = failSound;
                eventcheckaudio.Play();

                audioSource.Stop();

                if (instantiateAndDestroyCoroutine != null)
                {
                    StopCoroutine(instantiateAndDestroyCoroutine);
                    instantiateAndDestroyCoroutine = null;
                }
            }
        }
    }

    IEnumerator InstantiateAndPlayAudio(GameObject instance)
    {

    }
}
