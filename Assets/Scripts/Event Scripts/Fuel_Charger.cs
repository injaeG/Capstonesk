using UnityEngine;

public class Fuel_Charge : MonoBehaviour
{
    private bool fuelCharged = false; // 충전 여부를 확인하기 위한 플래그
    private int maxFuelAmount = 200; // 차량 연료의 최대치

    public AudioClip chargeSound; // 충전 시 재생할 효과음
    private AudioSource audioSource;

    private void Start()
    {
        // AudioSource 컴포넌트를 추가하고 초기화
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (fuelCharged) return; // 이미 충전된 경우 더 이상 처리하지 않음

        if (other.CompareTag("Car"))
        {
            // 충돌한 객체가 Car 태그를 가진 경우
            VehicleController vehicleController = other.GetComponent<VehicleController>();
            if (vehicleController != null)
            {
                // 현재 연료량이 최대 연료량보다 적은지 확인
                if (vehicleController.fuelAmount < maxFuelAmount)
                {
                    // 최대 연료량을 초과하지 않도록 연료를 추가
                    vehicleController.fuelAmount = Mathf.Min(vehicleController.fuelAmount + 200, maxFuelAmount);
                    fuelCharged = true; // 충전 완료

                    // 소리 재생
                    if (chargeSound != null)
                    {
                        audioSource.PlayOneShot(chargeSound);
                    }
                }
            }
        }
    }
}