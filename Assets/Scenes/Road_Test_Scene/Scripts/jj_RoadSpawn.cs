using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jj_RoadSpawn : MonoBehaviour
{
    //public GameObject other;
    // Start is called before the first frame update

    public Vector3 SpawnOffset;
    public GameObject prefabcube;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (GameObject)
        //{
           
        //}
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RoadSpawner"))
        {
            Vector3 spawnPosition = transform.position + new Vector3(-3.2f,20.9f,-57.3f);
            Instantiate(prefabcube, spawnPosition, Quaternion.identity);
            Debug.Log("aaa");
        }
        
    }
}
