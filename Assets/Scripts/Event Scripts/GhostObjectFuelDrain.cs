using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostObjectFuelDrain : MonoBehaviour
{
    public float fuelDrainRate = 1f; // 연료가 소모되는 속도
    private float timer = 0f; // 오브젝트가 5초 이상 보여졌는지 확인하는 타이머
    private VehicleController vehicleController; // VehicleController 스크립트에 대한 참조
    private float fuelThreshold = 25f; // 연료가 줄어드는 양
    private bool hasDrainedFuel = false; // 연료가 한 번 소모되었는지 확인하는 플래그

    void Start()
    {
        vehicleController = GameObject.FindObjectOfType<VehicleController>();
    }

    void Update()
    {
        // ghost 태그가 있는 모든 오브젝트를 찾습니다.
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("ghost");

        if (!hasDrainedFuel && ghosts.Length > 0)
        {
            timer += Time.deltaTime;

            if (timer >= 5f)
            {
                // 연료 감소
                vehicleController.fuelAmount = Mathf.Max(0, vehicleController.fuelAmount - fuelThreshold);

                // ghost 태그가 있는 모든 오브젝트 삭제
                foreach (GameObject ghost in ghosts)
                {
                    Destroy(ghost);
                }

                hasDrainedFuel = true; // 연료 감소가 이루어졌음을 표시
            }
        }
        else
        {
            timer = 0; // ghost 오브젝트가 없다면 타이머를 초기화합니다.
        }
    }
}
