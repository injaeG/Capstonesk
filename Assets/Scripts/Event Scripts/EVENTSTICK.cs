using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EVENTSTICK : MonoBehaviour
{
    public GameObject prefab; // Prefab to instantiate
    public float spawnProbability = 0.5f; // Probability to spawn the prefab (0 to 1)
    private GameObject instantiatedPrefab; // Reference to the instantiated prefab
    private static GameObject latestSignObject; // Static reference to the most recent "표지판생성" object

    void Start()
    {
        // Find the most recent "표지판생성" object
        FindLatestSignObject();

        // Try to instantiate the prefab
        TrySpawnPrefab();
    }

    void OnTransformChildrenChanged()
    {
        Debug.Log("Children changed, checking for new '표지판생성' objects.");

        // Re-check for the latest sign object whenever children change
        FindLatestSignObject();

        // Try to instantiate the prefab
        TrySpawnPrefab();
    }

    void FindLatestSignObject()
    {
        GameObject[] signObjects = GameObject.FindGameObjectsWithTag("표지판생성");
        Debug.Log($"Found {signObjects.Length} objects with tag '표지판생성'.");
        if (signObjects.Length > 0)
        {
            latestSignObject = signObjects[signObjects.Length - 1];
            Debug.Log("Latest sign object found: " + latestSignObject.name);
        }
        else
        {
            Debug.LogError("No object with tag '표지판생성' found");
        }
    }

    void TrySpawnPrefab()
    {
        // Check if the most recent "표지판생성" object exists
        if (latestSignObject != null)
        {
            // Generate a random value between 0 and 1
            float randomValue = Random.Range(0f, 1f);
            Debug.Log($"Random value generated: {randomValue}");

            // Check if the random value is less than the spawn probability
            if (randomValue < spawnProbability)
            {
                // Instantiate the prefab at the position of the latest sign object with the prefab's default rotation
                instantiatedPrefab = Instantiate(prefab, latestSignObject.transform.position, prefab.transform.rotation, latestSignObject.transform);
                Debug.Log("Prefab instantiated.");

                // Start the coroutine to monitor the prefab
                StartCoroutine(MonitorPrefab());
            }
            else
            {
                Debug.Log("Prefab not instantiated due to probability check.");
            }
        }
        else
        {
            Debug.LogError("Latest object with tag '표지판생성' not found");
        }
    }

    IEnumerator MonitorPrefab()
    {
        while (true)
        {
            // Check if the instantiated prefab has been destroyed
            if (instantiatedPrefab == null)
            {
                Debug.Log("Prefab destroyed. Trying to spawn again.");
                // Try to spawn the prefab again
                TrySpawnPrefab();
                yield break; // Exit the coroutine
            }
            // Wait for a frame before checking again
            yield return null;
        }
    }
}
