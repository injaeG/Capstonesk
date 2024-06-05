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
            Debug.Log("Car�� �浹�߽��ϴ�. ���� ��� ����˴ϴ�.");
        }
        else if (other.CompareTag("Arrow_Key") && !arrowKeyCollided)
        {
            arrowKeyCollided = true;
            Debug.Log("Arrow_Key�� �浹�߽��ϴ�. ���� ��� ����˴ϴ�.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car") && carCollided && !arrowKeyCollided)
        {
            ChangeVehicleControls(other.gameObject, false);
            carCollided = false;
            Debug.Log("Car�� �浹�� �������ϴ�. ���� ��� ������� �����˴ϴ�.");
        }
        else if (other.CompareTag("Arrow_Key_Og") && arrowKeyCollided)
        {
            arrowKeyCollided = false;
            Debug.Log("Arrow_Key_Og�� �浹�� �������ϴ�. ���� ��� ������� �����˴ϴ�.");
        }
    }

    private void ChangeVehicleControls(GameObject vehicle, bool reverseControls)
    {
        VehicleController vehicleController = vehicle.GetComponent<VehicleController>();
        if (vehicleController != null)
        {
            // W Ű�� ����, S Ű�� ����, A Ű�� ��ȸ��, D Ű�� ��ȸ��
            float horizontalInput = reverseControls ? -Input.GetAxis("Horizontal") : Input.GetAxis("Horizontal");
            vehicleController.SetControls(reverseControls ? -1 : 1, reverseControls ? 1 : 0, horizontalInput);
        }
    }
}