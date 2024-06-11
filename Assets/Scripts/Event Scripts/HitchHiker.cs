using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitchHiker : EventController
{
    public void HitchHikerSpawnEventPrefab()
    {
        Debug.Log("Hitchhiker");
        string[] validTags = { "hitch_human", "hitch_ghost" };
        SpawnEventPrefab(validTags);
    }

    public void HitchDestroy()
    {
        if (instance == null)
        {
            Debug.LogError("Instance is null!");
            return;
        }

        if (instance.CompareTag("hitch_ghost"))
        {
            Debug.Log("귀신입니다.");
            eventcheckaudio.clip = failSound;
            vehicleController.fuelAmount -= 20f;
        }
        else if (instance.CompareTag("hitch_human"))
        {
            Debug.Log("사람입니다.");
            eventcheckaudio.clip = successSound;
        }

        DestroyInstance();
        eventcheckaudio.Play();
        randomhitchPoint.SetActive(false);
    }
}
