using UnityEngine;

public class Fuel_Charge : MonoBehaviour
{
    private bool fuelCharged = false; // 충전 여부를 확인하기 위한 플래그

    private void OnTriggerStay(Collider other)
    {
        if (fuelCharged) return; // 이미 충전된 경우 더 이상 처리하지 않음

        if (other.CompareTag("Car"))
        {
            // 충돌한 객체가 Car 태그를 가진 경우
            VehicleController vehicleController = other.GetComponent<VehicleController>();
            if (vehicleController != null)
            {
                // 여기서 차량의 연료량을 설정
                vehicleController.fuelAmount = 1000; // 예시로 100으로 설정
                fuelCharged = true; // 충전 완료
            }
        }
    }
}