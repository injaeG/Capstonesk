using UnityEngine;

public class Fuel_Charge_fake : MonoBehaviour
{
    private bool fuelCharged = false; // 충전 여부를 확인하기 위한 플래그
    public AudioClip chargeSound; // 인스펙터 창에서 소리를 지정할 수 있게 public으로 선언
    private AudioSource audioSource;

    private void Start()
    {
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
                // 여기서 차량의 연료량을 설정
                vehicleController.fuelAmount -= 15; // 현재 충전량에서 200을 뺌
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