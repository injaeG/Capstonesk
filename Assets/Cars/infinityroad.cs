using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infinityroar : MonoBehaviour
{
    public GameObject roadPrefab;
    public Transform player;
    public float spawnZ = 0.0f;
    public float roadLength = 10.0f;
    public int roadsOnScreen = 15;
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
            SpawnRoad();
            DeleteRoad();
        }
    }

    void SpawnRoad()
    {
        GameObject road = Instantiate(roadPrefab, transform.forward * spawnZ, transform.rotation);
        activeRoads.Add(road);
        spawnZ += roadLength;
    }

    void DeleteRoad()
    {
        Destroy(activeRoads[0]);
        activeRoads.RemoveAt(0);
    }
}