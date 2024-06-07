using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EVENTSTICK : MonoBehaviour
{
    public GameObject prefab; // Prefab to instantiate
    public float spawnProbability = 0.5f; // Probability to spawn the prefab (0 to 1)
    private Quaternion prefabRotation; // Variable to store the prefab's rotation

    void Start()
    {
        // Store the prefab's initial rotation
        prefabRotation = prefab.transform.rotation;

        // Find the object named "표지판생성"
        GameObject signObject = GameObject.Find("표지판생성");

        // Check if the object exists
        if (signObject != null)
        {
            // Generate a random value between 0 and 1
            float randomValue = Random.Range(0f, 1f);

            // Check if the random value is less than the spawn probability
            if (randomValue < spawnProbability)
            {
                // Instantiate the prefab at the position of the sign object with the stored rotation
                Instantiate(prefab, signObject.transform.position, prefabRotation);
            }
        }
        else
        {
            Debug.LogError("Object with name '표지판생성' not found");
        }
    }
}