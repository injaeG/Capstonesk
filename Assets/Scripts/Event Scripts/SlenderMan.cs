using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlenderMan : MonoBehaviour
{
    public GameObject prefab; // Prefab to instantiate
    public float spawnProbability = 0.5f; // Probability to spawn the prefab (0 to 1)

    //void Start()
    //{

    //    GameObject signObject = GameObject.Find("슬렌더");

    //    // Check if the object exists
    //    if (signObject != null)
    //    {
    //        // Generate a random value between 0 and 1
    //        float randomValue = Random.Range(0f, 1f);

    //        // Check if the random value is less than the spawn probability
    //        if (randomValue < spawnProbability)
    //        {
    //            // Instantiate the prefab at the position of the sign object with the prefab's default rotation
    //            Instantiate(prefab, signObject.transform.position, prefab.transform.rotation);
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("Object with name '슬렌더' not found");
    //    }
    //}

    public void SpawnEventPrefab()
    {
        GameObject signObject = GameObject.Find("슬렌더");

        // Check if the object exists
        if (signObject != null)
        {
            // Generate a random value between 0 and 1
            float randomValue = Random.Range(0f, 1f);

            // Check if the random value is less than the spawn probability
            if (randomValue < spawnProbability)
            {
                // Instantiate the prefab at the position of the sign object with the prefab's default rotation
                Instantiate(prefab, signObject.transform.position, prefab.transform.rotation);
            }
        }
        else
        {
            Debug.LogError("Object with name '슬렌더' not found");
        }
    }
}
