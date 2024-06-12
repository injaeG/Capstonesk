using UnityEngine;

public class SlenderManSpawner : MonoBehaviour
{
    public GameObject emptyObjectPrefab; // Prefab for the empty object
    public GameObject[] prefabsToSpawn; // Array of prefabs to be spawned
    public float spawnProbability = 0.5f; // Probability to spawn the prefabs (0 to 1)

    //void Start()
    //{
    //    // Instantiate the empty object at this script's position
    //    GameObject emptyObject = Instantiate(emptyObjectPrefab, transform.position, Quaternion.identity);

    //    // Generate a random value between 0 and 1
    //    float randomValue = Random.Range(0f, 1f);

    //    // Check if the random value is less than the spawn probability
    //    if (randomValue < spawnProbability)
    //    {
    //        // Randomly select a prefab from the array
    //        int randomIndex = Random.Range(0, prefabsToSpawn.Length);
    //        GameObject selectedPrefab = prefabsToSpawn[randomIndex];

    //        // Instantiate the selected prefab at the position of the empty object with the prefab's default rotation
    //        Instantiate(selectedPrefab, emptyObject.transform.position, selectedPrefab.transform.rotation);
    //    }
    //    else
    //    {
    //        Debug.Log("Prefab not spawned based on spawn probability");
    //    }

    //    // Destroy the empty object
    //    Destroy(emptyObject);
    //}


    public void SlenderManSpawnEventPrefab()
    {
        // Instantiate the empty object at this script's position
        GameObject emptyObject = Instantiate(emptyObjectPrefab, transform.position, Quaternion.identity);

        // Generate a random value between 0 and 1
        float randomValue = Random.Range(0f, 1f);

        // Check if the random value is less than the spawn probability
        if (randomValue < spawnProbability)
        {
            // Randomly select a prefab from the array
            int randomIndex = Random.Range(0, prefabsToSpawn.Length);
            GameObject selectedPrefab = prefabsToSpawn[randomIndex];

            // Instantiate the selected prefab at the position of the empty object with the prefab's default rotation
            Instantiate(selectedPrefab, emptyObject.transform.position, selectedPrefab.transform.rotation);
        }
        else
        {
            Debug.Log("Prefab not spawned based on spawn probability");
        }

        // Destroy the empty object
        Destroy(emptyObject);
    }
}