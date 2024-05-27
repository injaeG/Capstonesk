using UnityEngine;

public class RoadMaker : MonoBehaviour
{
    public GameObject[] roadPrefabs;
    public string roadSpawnPlaceTag = "Road_Spawn_Place";
    private bool isCollisionHandled = false;

    void Start()
    {
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

    private bool IsPositionOccupied(Vector3 position, float checkDistance)
    {
        // Raycast를 이용하여 지정된 위치에서 아래 방향으로 도로가 있는지 확인
        if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, checkDistance))
        {
            if (hit.collider.CompareTag("Road"))
            {
                return true; // 도로 프리팹이 이미 존재함
            }
        }
        return false; // 도로 프리팹이 존재하지 않음
    }

    private void InstantiateRandomRoadPrefab(GameObject spawnPlace)
    {
        if (roadPrefabs != null && roadPrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, roadPrefabs.Length);
            GameObject roadPrefabToInstantiate = roadPrefabs[randomIndex];
            Vector3 spawnPosition = spawnPlace.transform.position;
            Quaternion spawnRotation = Quaternion.Euler(0, spawnPlace.transform.rotation.eulerAngles.y, 0);

            float checkDistance = 10f; // Raycasting으로 확인할 거리
            if (!IsPositionOccupied(spawnPosition + Vector3.up * checkDistance, checkDistance))
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