using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road_Spawner : MonoBehaviour
{
    public List<GameObject> roads;
    private float offset = 50f;

    // Start is called before the first frame update
    void Start()
    {
        if(roads != null && roads.Count > 0)
        {
            roads = roads.OrderBy(r => r.transform.position.z).ToList();
        }
    }

    // Update is called once per frame
    public void MoveRoad()
    {
        GameObject movedRoad = roads
    }
}
