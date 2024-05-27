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
            int randomIndex = Random.Range(0, roadPrefabs.Length);
            GameObject roadPrefabToInstantiate = roadPrefabs[randomIndex];
            Vector3 spawnPosition = spawnPlace.transform.position;
            Quaternion spawnRotation = Quaternion.Euler(0, spawnPlace.transform.rotation.eulerAngles.y, 0);
            
            float radius = 10f; // 구체의 반지름을 정의합니다. 이 예제에서는 5 유닛으로 설정합니다.
            if (!IsPositionOccupied(spawnPosition, radius))
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