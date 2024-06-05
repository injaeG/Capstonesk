using UnityEngine;

public class Event_ArrowKey : MonoBehaviour
{
    private bool carCollided = false;
    private bool arrowKeyCollided = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car") && !carCollided && !arrowKeyCollided)
        {
            ChangeVehicleControls(other.gameObject, true);
            carCollided = true;
            Debug.Log("Car와 충돌했습니다. 차량 제어가 변경됩니다.");
        }
        else if (other.CompareTag("Arrow_Key") && !arrowKeyCollided)
        {
            arrowKeyCollided = true;
            Debug.Log("Arrow_Key와 충돌했습니다. 차량 제어가 변경됩니다.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car") && carCollided && !arrowKeyCollided)
        {
            ChangeVehicleControls(other.gameObject, false);
            carCollided = false;
            Debug.Log("Car와 충돌이 끝났습니다. 차량 제어가 원래대로 복구됩니다.");
        }
        else if (other.CompareTag("Arrow_Key_Og") && arrowKeyCollided)
        {
            arrowKeyCollided = false;
            Debug.Log("Arrow_Key_Og와 충돌이 끝났습니다. 차량 제어가 원래대로 복구됩니다.");
        }
    }

    private void ChangeVehicleControls(GameObject vehicle, bool reverseControls)
    {
        VehicleController vehicleController = vehicle.GetComponent<VehicleController>();
        if (vehicleController != null)
        {
            // W 키로 전진, S 키로 후진, A 키로 좌회전, D 키로 우회전
            float horizontalInput = reverseControls ? -Input.GetAxis("Horizontal") : Input.GetAxis("Horizontal");
            vehicleController.SetControls(reverseControls ? -1 : 1, reverseControls ? 1 : 0, horizontalInput);
        }
    }
}