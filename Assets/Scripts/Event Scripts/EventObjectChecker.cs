using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObjectChecker : MonoBehaviour
{
    public VehicleController vehicleController; // VehicleController 인스턴스

    void Update()
    {
        // 'EVENT' 레이어에 있는 오브젝트가 있는지 확인합니다.
        GameObject[] eventObjects = GameObject.FindObjectsOfType<GameObject>();
        bool eventObjectExists = false;
        foreach (GameObject obj in eventObjects)
        {
            if (obj.layer == LayerMask.NameToLayer("EVENT"))
            {
                eventObjectExists = true;
                break;
            }
        }

        // 'EVENT' 레이어에 있는 오브젝트가 없으면 연료를 감소시킵니다.
        if (!eventObjectExists)
        {
            StartCoroutine(DecreaseFuelAfterDelay());
        }
    }

    IEnumerator DecreaseFuelAfterDelay()
    {
        // 5초 동안 기다립니다.
        yield return new WaitForSeconds(5f);

        // 'EVENT' 레이어에 있는 오브젝트가 아직 존재하면 연료를 감소시킵니다.
        GameObject[] eventObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in eventObjects)
        {
            if (obj.layer == LayerMask.NameToLayer("EVENT"))
            {
                vehicleController.fuelConsumptionRate -= 20f;
                break;
            }
        }
    }
}
