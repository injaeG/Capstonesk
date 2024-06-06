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

    public AudioClip successSound;
    public AudioClip failSound;

    private AudioSource audioSource;
    private AudioSource eventcheckaudio;
    private GameObject instance;
    private GameObject[] instances;
    private Coroutine instantiateAndDestroyCoroutine;

    public VehicleController vehicleController;

    private bool eventObjectExists = true;

    void Awake()
    {
        // AudioSource 컴포넌트를 가져옵니다.
        audioSource = GetComponent<AudioSource>();

        eventcheckaudio = GetComponent<AudioSource>();
    }

    void Start()
    {
        //initEvent();
    }

    public void initEvent()
    {
        // 'Event' 레이어에 있는 오브젝트가 있는지 확인합니다.
        //GameObject[] eventObjects = GameObject.FindObjectsOfType<GameObject>();
        //bool eventObjectExists = true;

        if (true)
        {
            Debug.Log("aa");

            if (eventObjectExists)
            {
                // PreFabs/event 디렉토리에 있는 모든 프리팹을 로드합니다.
                eventPrefabs = Resources.LoadAll<GameObject>("PreFabs/event");
                foreach (GameObject taggedPrefab in GameObject.FindGameObjectsWithTag("event"))
                {
                    if (Random.Range(0, 3) == 0)
                        EyeGhostSpawnEventPrefab();
                    else
                        SpawnRandomEventPrefab();

                    //EyeGhostSpawnEventPrefab();

                    //SpawnRandomEventPrefab();
                }
            }
        }
    }

    private GameObject randomhitchPoint;

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
        else if (selectedPrefab.CompareTag("hitch_ghost") || selectedPrefab.CompareTag("hitch_human")) // 히치하이커
        {
            audioSource.clip = hitchhikerSound;
            GameObject[] hitchPoints = GameObject.FindGameObjectsWithTag("hitchpoint");
            if (hitchPoints.Length == 0) return;

            randomhitchPoint = hitchPoints[Random.Range(0, hitchPoints.Length)];
            randomhitchPoint.SetActive(true);

            Transform[] children = randomhitchPoint.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in children)
            {
                child.gameObject.SetActive(true);
            }

            instance = Instantiate(selectedPrefab, randomhitchPoint.transform.position, Quaternion.identity);
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

    public void HitchDestroy()
    {
        if (instance.CompareTag("hitch_ghost"))
        {
            Debug.Log("귀신입니다.");

            eventcheckaudio.clip = failSound;

            vehicleController.fuelAmount -= 20f;
        }
        else
        {
            Debug.Log("사람입니다.");

            eventcheckaudio.clip = successSound;
        }
        StopAndDestroyEarly();

        eventcheckaudio.Play();

        randomhitchPoint.SetActive(false);
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

    bool iseyetriggered = false;

    IEnumerator InstantiateAndPlayAudio(GameObject instance)
    {
        audioSource.Play();
        yield return new WaitForSeconds(60);

        if (instances != null)
        {


            eventcheckaudio.clip = successSound;
            eventcheckaudio.Play();
        }

        if (instance.CompareTag("eye_ghost"))
        {

           
        }
        else if (instance.CompareTag("hitch_ghost"))
        {
            Debug.Log("귀신입니다.");

            eventcheckaudio.clip = successSound;
        }
        else if (instance.CompareTag("hitch_human"))
        {
            Debug.Log("사람입니다.");

            eventcheckaudio.clip = failSound;

            vehicleController.fuelAmount -= 20f;
        }

        Destroy(instance);

        audioSource.Stop();

        eventcheckaudio.Play();
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