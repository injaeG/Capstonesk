using UnityEngine;

public class RoadMaker : MonoBehaviour
{
    // 생성할 도로 프리팹 배열
    public GameObject[] roadPrefabs;
    // 도로 생성 위치를 식별할 태그
    public string roadSpawnPlaceTag = "Road_Spawn_Place";
    // 충돌 처리 여부 플래그
    private bool isCollisionHandled = false;

    void Start()
    {
        // "Prefabs/Roads" 경로의 모든 도로 프리팹 로드
        roadPrefabs = Resources.LoadAll<GameObject>("Prefabs/Roads");
        if (roadPrefabs == null || roadPrefabs.Length == 0)
        {
            Debug.LogError("No road prefabs found in Resources/Prefabs/Roads.");
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

    // 빈 오브젝트를 비활성화하는 메서드
    private void DeactivateSpawnPlace(GameObject spawnPlace)
    {
        spawnPlace.SetActive(false);
    }

    // 충돌 시 처리
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
                // roadSpawnPlaceTag 태그를 가진 오브젝트의 위치를 기준으로 도로 프리팹 생성
                InstantiateRandomRoadPrefab(closestSpawnPlace);
                // 충돌한 오브젝트 비활성화
                DeactivateSpawnPlace(closestSpawnPlace);
            }

            isCollisionHandled = false; // 충돌 처리 완료
        }
    }

    // 주어진 위치에 다른 오브젝트가 있는지 체크
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

    // 무작위 도로 프리팹 생성
private void InstantiateRandomRoadPrefab(GameObject spawnPlace)
{
    if (roadPrefabs != null && roadPrefabs.Length > 0)
    {
        int randomIndex = Random.Range(0, roadPrefabs.Length);
        GameObject roadPrefabToInstantiate = roadPrefabs[randomIndex];
        // spawn 오브젝트의 위치를 기준으로 도로 프리팹 생성
        Vector3 spawnPosition = spawnPlace.transform.position;
        // spawn 오브젝트의 회전값을 사용하여 도로의 y축 회전값을 설정
        Quaternion spawnRotation = Quaternion.Euler(0, spawnPlace.transform.rotation.eulerAngles.y, 0);
        
        // 새로운 도로 프리팹이 겹치지 않도록 위치 확인
        float roadPrefabRadius = 5f; // 프리팹의 반지름 (겹침 확인을 위한 값)
        if (!IsPositionOccupied(spawnPosition, roadPrefabRadius))
        {
            Instantiate(roadPrefabToInstantiate, spawnPosition, spawnRotation);
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
