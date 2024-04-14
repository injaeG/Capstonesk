using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRoadGenerator : MonoBehaviour
{
    public GameObject roadPrefab;
    public Transform player;
    public float roadLength = 10.0f;
    public int roadsOnScreen = 15;
    private float spawnZ = 0.0f;
    private float roadWidth = 4.0f;
    private int roadDirection; // 0 for forward, 1 for right, -1 for left
    private List<GameObject> activeRoads;

    void Start()
    {
        activeRoads = new List<GameObject>();
        for (int i = 0; i < roadsOnScreen; i++)
        {
            SpawnRoad();
        }
    }

    void Update()
    {
        if (player.position.z - 100 > (spawnZ - roadsOnScreen * roadLength))
        {
            roadDirection = Random.Range(-1, 2); // Randomly choose forward, right, or left
            SpawnRoad();
            DeleteRoad();
        }
    }

    void SpawnRoad()
    {
        Vector3 spawnPosition = new Vector3(0, 0, spawnZ);
        if (roadDirection == 1) // Right
        {
            spawnPosition.x = roadWidth;
        }
        else if (roadDirection == -1) // Left
        {
            spawnPosition.x = -roadWidth;
        }
        GameObject road = Instantiate(roadPrefab, spawnPosition, transform.rotation);
        activeRoads.Add(road);
        spawnZ += roadLength;
    }

    void DeleteRoad()
    {
        Destroy(activeRoads[0]);
        activeRoads.RemoveAt(0);
    }
}
