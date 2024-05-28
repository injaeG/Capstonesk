using UnityEngine;

public class RoadMaker : MonoBehaviour
{
    public GameObject[] roadPrefabs; // 도로 프리팹 배열
    public string roadSpawnPlaceTag = "Road_Spawn_Place"; // 도로 스폰 위치를 나타내는 태그
    private bool isCollisionHandled = false; // 충돌 처리 여부를 나타내는 불리언 변수
    private int lastPrefabIndex = -1; // 마지막으로 생성된 도로 프리팹의 인덱스 (-1은 시작 시 아무것도 생성되지 않았음을 의미)

    private GameObject lastSpawnedRoad; // 마지막으로 생성된 도로 프리팹

    void Start()
    {
        // Resources 폴더에서 도로 프리팹을 로드합니다.
        roadPrefabs = Resources.LoadAll<GameObject>("Prefabs/Roads");
        if (roadPrefabs == null || roadPrefabs.Length == 0)
        {
            Debug.LogError("No road prefabs found in Resources/Prefabs/Roads.");
        }
    }

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

    private void DeactivateSpawnPlace(GameObject spawnPlace)
    {
        spawnPlace.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isCollisionHandled && other.gameObject.CompareTag("Car"))
        {
            isCollisionHandled = true;
            Debug.Log("충돌 감지");

            Vector3 collisionPoint = other.ClosestPointOnBounds(transform.position);
            GameObject closestSpawnPlace = FindClosestSpawnPlace(collisionPoint);
            if (closestSpawnPlace != null)
            {
                InstantiateRandomRoadPrefab(closestSpawnPlace);
                DeactivateSpawnPlace(closestSpawnPlace);
            }

            isCollisionHandled = false;
        }
    }

    private bool IsPositionOccupied(Vector3 position, float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Road"))
            {
                return true;
            }
        }
        return false;
    }

    private void InstantiateRandomRoadPrefab(GameObject spawnPlace)
    {
        if (roadPrefabs != null && roadPrefabs.Length > 0)
        {
            int randomIndex;
            do
            {
                // 이전에 생성된 프리팹과 다른 프리팹을 선택하기 위해 반복합니다.
                randomIndex = Random.Range(0, roadPrefabs.Length);
            } while (randomIndex == lastPrefabIndex && roadPrefabs.Length > 1); // 도로 프리팹 배열에 한 가지 이상의 프리팹이 있을 경우에만 실행

            lastPrefabIndex = randomIndex; // 마지막으로 생성된 프리팹의 인덱스를 업데이트합니다.
            GameObject roadPrefabToInstantiate = roadPrefabs[randomIndex];
            Vector3 spawnPosition = spawnPlace.transform.position;
            Quaternion spawnRotation = Quaternion.Euler(0, spawnPlace.transform.rotation.eulerAngles.y, 0);

            float radius = 1f;

            // 이전에 생성된 프리팹이 있을 경우 삭제합니다.
            if (lastSpawnedRoad != null)
            {
                Destroy(lastSpawnedRoad);
                lastSpawnedRoad = null; // 삭제 후 null로 설정하여 안전하게 처리
            }

            if (!IsPositionOccupied(spawnPosition, radius))
            {
                // 새로운 도로를 인스턴스화하고 추적합니다.
                lastSpawnedRoad = Instantiate(roadPrefabToInstantiate, spawnPosition, spawnRotation);
            }
            else
            {
                Debug.LogWarning("Cannot instantiate road prefab at the spawn place. Position is occupied.");
            }
        }
        else
        {
            Debug.LogWarning("No road prefabs are loaded.");
        }
    }
}