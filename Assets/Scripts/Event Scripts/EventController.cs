*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public AudioClip hitchhikerSound;
    public AudioClip failSound;
    public AudioClip successSound;
    public AudioSource eventcheckaudio;
    public GameObject[] eventPrefabs;
    public GameObject randomhitchPoint;
    public VehicleController vehicleController;
    protected GameObject instance;

    protected void SpawnEventPrefab(string[] validTags)
    {
        GameObject selectedPrefab = null;

        do
        {
            int index = Random.Range(0, eventPrefabs.Length);
            selectedPrefab = eventPrefabs[index];
        } while (selectedPrefab == null || !IsValidTag(selectedPrefab, validTags));

        instance = Instantiate(selectedPrefab, randomhitchPoint.transform.position, Quaternion.identity);
        StartCoroutine(InstantiateAndPlayAudio(instance));
    }

    private bool IsValidTag(GameObject prefab, string[] validTags)
    {
        foreach (string tag in validTags)
        {
            if (prefab.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }

    protected void DestroyInstance()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
    }

    IEnumerator InstantiateAndPlayAudio(GameObject instance)
    {
        AudioSource audioSource = instance.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.clip = hitchhikerSound;
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
        }
    }
}
