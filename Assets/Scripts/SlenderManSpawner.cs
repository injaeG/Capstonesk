using UnityEngine;

public class SlenderManSpawner : MonoBehaviour
{
    public GameObject emptyObjectPrefab; // Prefab for the empty object
    public GameObject[] prefabsToSpawn; // Array of prefabs to be spawned
    public float spawnProbability = 0.5f; // Probability to spawn the prefabs (0 to 1)
    public float movementSpeed = 5f; // Movement speed of the spawned prefab

    void Start()
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
            GameObject spawnedPrefab = Instantiate(selectedPrefab, emptyObject.transform.position, selectedPrefab.transform.rotation);

            // Add a Collider component to the spawned prefab and set it as a trigger
            Collider collider = spawnedPrefab.AddComponent<BoxCollider>();
            collider.isTrigger = true;

            // Get the rigidbody component of the spawned prefab
            Rigidbody rb = spawnedPrefab.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Calculate the direction the prefab should move towards (in this case, the direction the prefab is facing)
                Vector3 moveDirection = spawnedPrefab.transform.forward.normalized;

                // Set the velocity of the rigidbody to move in the calculated direction with the specified speed
                rb.velocity = moveDirection * movementSpeed * 2f;
            }
            else
            {
                Debug.LogWarning("Rigidbody component not found on the spawned prefab.");
            }
        }
        else
        {
            Debug.Log("Prefab not spawned based on spawn probability");
        }

        // Destroy the empty object
        Destroy(emptyObject);
    }
}