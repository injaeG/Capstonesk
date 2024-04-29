using UnityEngine;

public class RoadMaker : MonoBehaviour
{
    // 생성할 도로 프리팹 배열
    public GameObject[] roadPrefabs;
    // 도로 생성 위치를 식별할 태그
    public string roadSpawnPlaceTag = "Road_Spawn_Place";
    // 충돌 처리 여부 플래그
    private bool isCollisionHandled = false;
    // 충돌 감지 대상 오브젝트
    public GameObject targetObject;

    void Start()
    {
        // "Prefabs/Roads" 경로의 모든 도로 프리팹 로드
        roadPrefabs = Resources.LoadAll<GameObject>("Prefabs/Roads");
        if (roadPrefabs == null || roadPrefabs.Length == 0)
        {
            Debug.LogError("No road prefabs found in Resources/Prefabs/Roads.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // 충돌 처리가 아직 안 되었고, 대상 오브젝트와 충돌한 경우
        if (!isCollisionHandled && other.gameObject.CompareTag("Car"))
        {
            isCollisionHandled = true; // 충돌 처리 시작
            Debug.Log("충돌 감지");

            // 충돌 지점 계산
            Vector3 collisionPoint = other.ClosestPointOnBounds(transform.position);

            // 가장 가까운 Road_Spawn_Place 태그를 가진 빈 오브젝트 찾기
            GameObject closestSpawnPlace = FindClosestSpawnPlace(collisionPoint);
            if (closestSpawnPlace != null)
            {
                // 가장 가까운 위치에 도로 생성
                InstantiateRandomRoadPrefab(closestSpawnPlace.transform.position);
                // 충돌한 오브젝트 삭제
                Destroy(closestSpawnPlace);
            }

            isCollisionHandled = false; // 충돌 처리 완료
        }
    }


    // 주어진 위치에서 가장 가까운 Road_Spawn_Place 태그를 가진 오브젝트 찾기
    private GameObject FindClosestSpawnPlace(Vector3 position)
    {
        GameObject[] spawnPlaces = GameObject.FindGameObjectsWithTag(roadSpawnPlaceTag);
        GameObject closestSpawnPlace = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject spawnPlace in spawnPlaces)
        {
            float distance = Vector3.Distance(position, spawnPlace.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestSpawnPlace = spawnPlace;
            }
        }

        return closestSpawnPlace;
    }

    // 무작위 도로 프리팹 생성
    private void InstantiateRandomRoadPrefab(Vector3 position)
    {
        if (roadPrefabs != null && roadPrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, roadPrefabs.Length);
            GameObject roadPrefabToInstantiate = roadPrefabs[randomIndex];
            Instantiate(roadPrefabToInstantiate, position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No road prefabs are loaded.");
        }
    }
}
