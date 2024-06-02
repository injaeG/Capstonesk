using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPrefabSpawner : MonoBehaviour
{
    private GameObject[] eventPrefabs; // PreFabs/event 디렉토리에 있는 모든 프리팹
    public Transform spawnPoint; // 빈 오브젝트 위치 (Ghost 태그가 있을 경우)

    public AudioClip eye_ghostSound;
    public AudioClip eventSound;
    public AudioClip hitchhikerSound;

    private AudioSource audioSource;
    private GameObject instance;
    private Coroutine instantiateAndDestroyCoroutine;

    void Awake()
    {
        // AudioSource 컴포넌트를 가져옵니다.
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        // PreFabs/event 디렉토리에 있는 모든 프리팹을 로드합니다.
        eventPrefabs = Resources.LoadAll<GameObject>("PreFabs/event");
        foreach (GameObject taggedPrefab in GameObject.FindGameObjectsWithTag("event"))
        {
            if (Random.Range(0, 3) == 0)
                EyeGhostSpawnEventPrefab();
            else
                SpawnRandomEventPrefab();
        }
    }

    void SpawnRandomEventPrefab()
    {
        Debug.Log("Not eye_ghost");

        if (eventPrefabs.Length == 0) return;

        GameObject selectedPrefab = null;

        do
        {
            int index = Random.Range(0, eventPrefabs.Length);
            selectedPrefab = eventPrefabs[index];
        } while (selectedPrefab.CompareTag("eye_ghost") && selectedPrefab != null); // 수정: 무한 루프 방지

        if (selectedPrefab == null) return; // selectedPrefab가 null인 경우 처리를 중단합니다.

        if (selectedPrefab.CompareTag("ghost"))
        {
            audioSource.clip = hitchhikerSound;
            instance = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity, spawnPoint);
        }
        else
        {
            audioSource.clip = eventSound;
            Vector3 spawnPosition = FindRoadPositionNearby();
            instance = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
        }

        instantiateAndDestroyCoroutine = StartCoroutine(InstantiateAndPlayAudio(instance));
    }

    void EyeGhostSpawnEventPrefab()
    {
        Debug.Log("eye_ghost");

        audioSource.clip = eye_ghostSound;

        if (eventPrefabs.Length == 0) return;

        GameObject[] selectedPrefabs = new GameObject[8];

        for (int i = 0; i < 8; i++)
        {
            do
            {
                int index = Random.Range(0, eventPrefabs.Length);
                selectedPrefabs[i] = eventPrefabs[index];
            } while (!selectedPrefabs[i].CompareTag("eye_ghost") && selectedPrefabs[i] != null);
        }

        GameObject[] roadObjects = GameObject.FindGameObjectsWithTag("Road");
        if (roadObjects.Length == 0) return;

        int count = -1;

        for (int j = 0; j < 4; j++)
        {
            GameObject randomRoadObject = roadObjects[Random.Range(0, roadObjects.Length)];

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
                instance = Instantiate(selectedPrefabs[count], spawnPosition, Quaternion.identity);
                instantiateAndDestroyCoroutine = StartCoroutine(InstantiateAndPlayAudio(instance));
            }
        }
    }

    private Vector3 FindRoadPositionNearby()
    {
        // 'road' 태그가 있는 모든 오브젝트를 찾습니다.
        GameObject[] roadObjects = GameObject.FindGameObjectsWithTag("Road");

        // 'road' 태그가 있는 오브젝트가 없는 경우, 현재 위치 반환
        if (roadObjects.Length == 0) return spawnPoint.position;

        // 'road' 태그가 있는 오브젝트 중에서 랜덤으로 하나를 선택합니다.
        GameObject randomRoadObject = roadObjects[Random.Range(0, roadObjects.Length)];

        // 선택한 오브젝트의 위치를 반환합니다.
        return randomRoadObject.transform.position;
    }

    IEnumerator InstantiateAndPlayAudio(GameObject instance)
    {
        audioSource.Play();
        yield return new WaitForSeconds(60);
        Destroy(instance);
        audioSource.Stop();
    }

    public void StopAndDestroyEarly()
    {
        if (instantiateAndDestroyCoroutine != null)
        {
            StopCoroutine(instantiateAndDestroyCoroutine);
            instantiateAndDestroyCoroutine = null;
        }

        if (instance != null)
        {
            Destroy(instance);
            instance = null;
        }

        audioSource.Stop();
    }
}
