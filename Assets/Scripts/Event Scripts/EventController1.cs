using System.Linq;
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
    public SlenderMan slenderMan;

    public LayerMask terrainLayerMask; // 지형 레이어 마스크

    private bool eventObjectExists = true;

    void Awake()
    {
        // AudioSource 컴포넌트를 가져옵니다.
        audioSource = GetComponent<AudioSource>();

        eventcheckaudio = GetComponent<AudioSource>();
    }

    public void initEvent(Transform parentTransform)
    {
        Debug.Log("init Event");

        // PreFabs/event 디렉토리에 있는 모든 프리팹을 로드합니다.
        eventPrefabs = Resources.LoadAll<GameObject>("PreFabs/event");

        //int randnum = Random.Range(0, 3);

        int randnum = 0;

        if (randnum == 0)
            EyeGhostSpawnEventPrefab(parentTransform.gameObject);
        else if (randnum == 1)
            HitchhikerSpawnEventPrefab(parentTransform.gameObject);
        else
            slenderMan.SpawnEventPrefab();
    }

    private GameObject hitchPoint;

    void EyeGhostSpawnEventPrefab(GameObject parentObject)
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

        Transform[] childTransforms = parentObject.GetComponentsInChildren<Transform>();
        GameObject[] roadObjects = System.Array.FindAll(childTransforms, t => t.CompareTag("Road")).Select(t => t.gameObject).ToArray();

        if (roadObjects.Length == 0) return;

        int count = -1;
        float spawnHeightOffset = 1.0f; // eye_ghost 프리팹을 도로 오브젝트보다 약간 높게 생성하는 오프셋

        for (int j = 0; j < 4; j++)
        {
            GameObject randomRoadObject = roadObjects[Random.Range(0, roadObjects.Length)];

            instances = new GameObject[selectedPrefabs.Length];

            for (int k = 0; k < 2; k++)
            {
                count++;
                if (count >= selectedPrefabs.Length) break;

                //Vector3 randomPosition = new Vector3(
                //    Random.Range(randomRoadObject.transform.position.x - 17, randomRoadObject.transform.position.x + 17),
                //    randomRoadObject.transform.position.y + spawnHeightOffset, // 도로의 높이 + 오프셋
                //    Random.Range(randomRoadObject.transform.position.z - 10, randomRoadObject.transform.position.z + 10)
                //);

                //Vector3 spawnPosition = FindRoadPositionNearby();

                if (randomRoadObject == null)
                {
                    Debug.LogError("Terrain object not assigned!");
                    return;
                }

                MeshRenderer meshRenderer = randomRoadObject.GetComponent<MeshRenderer>();
                if (meshRenderer == null)
                {
                    Debug.LogError("Terrain object does not have a MeshRenderer component!");
                    return;
                }

                Bounds terrainBounds = meshRenderer.bounds;

                // 지형의 범위 내에서 랜덤 위치를 선택합니다
                Vector3 randomPosition = new Vector3(
                    Random.Range(terrainBounds.min.x, terrainBounds.max.x),
                    0, // 높이는 나중에 설정합니다.
                    Random.Range(terrainBounds.min.z, terrainBounds.max.z)
                );

                // 지형의 높이를 계산하기 위해 아래로 레이캐스트를 수행합니다.
                Ray ray = new Ray(randomPosition + Vector3.up * 100f, Vector3.down);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainLayerMask))
                {
                    // 지형의 높이를 얻은 후 스폰 높이 오프셋을 더합니다.
                    randomPosition.y = hit.point.y + spawnHeightOffset;
                }
                else
                {
                    Debug.Log("Raycast failed");
                    // 레이캐스트가 지형을 맞추지 못한 경우, 기본 높이를 사용할 수 있습니다.
                    randomPosition.y = randomRoadObject.transform.position.y + spawnHeightOffset; // 기본 높이
                }

                instances[count] = Instantiate(selectedPrefabs[count], randomPosition, Quaternion.identity);
                instantiateAndDestroyCoroutine = StartCoroutine(InstantiateAndPlayAudio(instances[count]));
            }
        }
    }

    void HitchhikerSpawnEventPrefab(GameObject parentObject)
    {
        Debug.Log("hitchhiker");

        if (eventPrefabs.Length == 0) return;

        GameObject selectedPrefab = null;

        do
        {
            int index = Random.Range(0, eventPrefabs.Length);
            selectedPrefab = eventPrefabs[index];
        } while (selectedPrefab == null || (!selectedPrefab.CompareTag("hitch_human") && !selectedPrefab.CompareTag("hitch_ghost")));

        audioSource.clip = hitchhikerSound;

        // 부모 객체 안에 있는 "hitchpoint" 태그를 가진 오브젝트를 찾음
        Transform[] childTransforms = parentObject.GetComponentsInChildren<Transform>(true);
        GameObject[] hitchPoints = System.Array.FindAll(childTransforms, t => t.CompareTag("hitchpoint")).Select(t => t.gameObject).ToArray();

        if (hitchPoints.Length == 0) return;

        GameObject randomhitchPoint = hitchPoints[Random.Range(0, hitchPoints.Length)];
        hitchPoint = randomhitchPoint;
        randomhitchPoint.SetActive(true);

        Transform[] children = randomhitchPoint.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children)
        {
            child.gameObject.SetActive(true);
        }

        instance = Instantiate(selectedPrefab, randomhitchPoint.transform.position, Quaternion.identity);
        instantiateAndDestroyCoroutine = StartCoroutine(InstantiateAndPlayAudio(instance, randomhitchPoint));
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

    public void EyeGhostDestroy(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            Destroy(other.gameObject);
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

    //public void EyeGhostDestroyAll()
    //{
    //    foreach (GameObject inst in instances)
    //    {
    //        if (inst == null)
    //        {
    //            // 객체가 비어있으면 발생시킬 이벤트 추가
    //            Debug.Log("눈알 귀신이 없습니다.");
    //            // 혹은 다른 이벤트를 발생시킬 수 있습니다.
    //            eventcheckaudio.clip = failSound;
    //            eventcheckaudio.Play();

    //            audioSource.Stop();

    //            if (instantiateAndDestroyCoroutine != null)
    //            {
    //                StopCoroutine(instantiateAndDestroyCoroutine);
    //                instantiateAndDestroyCoroutine = null;
    //            }
    //        }
    //    }
    //}

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

        hitchPoint.SetActive(false);
    }

    bool iseyetriggered = false;

    IEnumerator InstantiateAndPlayAudio(GameObject instance)
    {
        audioSource.Play();
        yield return new WaitForSeconds(60);

        if (instance.CompareTag("eye_ghost"))
        {
            Debug.Log("eye_ghost 실패");

            eventcheckaudio.clip = failSound;

            vehicleController.fuelAmount -= 20f;
        }

        Destroy(instance);

        audioSource.Stop();

        eventcheckaudio.Play();
    }

    IEnumerator InstantiateAndPlayAudio(GameObject instance, GameObject hitchPoint)
    {
        audioSource.Play();
        yield return new WaitForSeconds(60);

        if (instance.CompareTag("hitch_ghost"))
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

        hitchPoint.SetActive(false);

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