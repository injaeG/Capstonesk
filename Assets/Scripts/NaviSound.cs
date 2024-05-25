using UnityEngine;
using System.Collections;

public class NaviSound : MonoBehaviour
{
    public AudioClip audioClip;

    bool isTrigger = false;

    void OnTriggerEnter(Collider other)
    {
        if (!isTrigger)
        {
            isTrigger = true;
            GetComponent<AudioSource>().PlayOneShot(audioClip);
            StartCoroutine(DestroyObject());
        }
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(audioClip.length);
        Destroy(gameObject);
    }
}