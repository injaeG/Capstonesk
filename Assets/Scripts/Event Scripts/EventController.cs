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
    //public SlenderMan slenderMan;

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

        if (parentTransform.gameObject.name == "Road_Prefabs_L" || parentTransform.gameObject.name == "Road_Prefabs_R")
            EyeGhostSpawnEventPrefab(parentTransform.gameObject);
        else if (parentTransform.gameObject.name != "Event_Prefab_Fog" || parentTransform.gameObject.name != "Event_PreFab_GasStation")
            HitchhikerSpawnEventPrefab(parentTransform.gameObject);
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

                Vector3 randomPosition = new Vector3(
                    Random.Range(randomRoadObject.transform.position.x - 17, randomRoadObject.transform.position.x + 17),
                    randomRoadObject.transform.position.y + spawnHeightOffset, // 도로의 높이 + 오프셋
                    Random.Range(randomRoadObject.transform.position.z - 10, randomRoadObject.transform.position.z + 10)
                );

                //Vector3 spawnPosition = FindRoadPositionNearby();

                instances[count] = Instantiate(selectedPrefabs[count], randomPosition, Quaternion.identity);
                instantiateAndDestroyCoroutine = StartCoroutine(InstantiateAndPlayAudio(instances[count]));
            }
        }
    }

    void HitchhikerSpawnEventPrefab(GameObject parentObject)
    {
        Debug.Log("hitchhiker");

        if (eventPrefabs.Length == 0) return;

        GameObject[] selectedPrefab = new GameObject[2];

        do
        {
            int index = Random.Range(0, eventPrefabs.Length);
            selectedPrefab[0] = eventPrefabs[index];
        } while (selectedPrefab[0] == null || (!selectedPrefab[0].CompareTag("hitch_human")));

        do
        {
            int index = Random.Range(0, eventPrefabs.Length);
            selectedPrefab[1] = eventPrefabs[index];
        } while (selectedPrefab[1] == null || (!selectedPrefab[1].CompareTag("hitch_ghost")));

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

        int i = Random.Range(0, 2);

        instance = Instantiate(selectedPrefab[i], randomhitchPoint.transform.position, Quaternion.identity);
        instantiateAndDestroyCoroutine = StartCoroutine(InstantiateAndPlayAudio(instance, randomhitchPoint));
    }

    int eye_count = 8;

    public void EyeGhostDestroy(Collider other)
    {
        Destroy(other.gameObject);

        eye_count--;

        Debug.Log(eye_count);

        if (eye_count <= 4)
        {
            // 객체가 비어있으면 발생시킬 이벤트 추가
            Debug.Log("눈알 귀신이 4개 이하입니다.");

            if (instantiateAndDestroyCoroutine != null)
            {
                StopCoroutine(instantiateAndDestroyCoroutine);
                instantiateAndDestroyCoroutine = null;
            }
            audioSource.Stop();

            eventcheckaudio.clip = failSound;
            eventcheckaudio.Play();
        }
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