using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation : MonoBehaviour
{
    public GameObject[] targetObjects; // 조절할 오브젝트 배열
    public float minDelay = 1f; // 최소 지연 시간
    public float maxDelay = 3f; // 최대 지연 시간

    void Start()
    {
        foreach (var obj in targetObjects)
        {
            StartCoroutine(ToggleObjectWithVaryingTimes(obj));
        }
    }

    IEnumerator ToggleObjectWithVaryingTimes(GameObject obj)
    {
        while (true)
        {
            // 켜지는 데 걸리는 랜덤한 지연 시간
            float delayToTurnOn = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delayToTurnOn);
            obj.SetActive(true);

            // 꺼지는 데 걸리는 랜덤한 지연 시간
            float delayToTurnOff = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delayToTurnOff);
            obj.SetActive(false);
        }
    }
}

