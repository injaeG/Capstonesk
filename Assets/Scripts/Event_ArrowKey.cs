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
            Debug.Log("Carï¿½ï¿½ ï¿½æµ¹ï¿½ß½ï¿½ï¿½Ï´ï¿½. ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½î°¡ ï¿½ï¿½ï¿½ï¿½Ë´Ï´ï¿½.");
        }
        else if (other.CompareTag("Arrow_Key") && !arrowKeyCollided)
        {
            arrowKeyCollided = true;
            Debug.Log("Arrow_Keyï¿½ï¿½ ï¿½æµ¹ï¿½ß½ï¿½ï¿½Ï´ï¿½. ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½î°¡ ï¿½ï¿½ï¿½ï¿½Ë´Ï´ï¿½.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car") && carCollided && !arrowKeyCollided)
        {
            ChangeVehicleControls(other.gameObject, false);
            carCollided = false;
            Debug.Log("Carï¿½ï¿½ ï¿½æµ¹ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Ï´ï¿½. ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½î°¡ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Ë´Ï´ï¿½.");
        }
        else if (other.CompareTag("Arrow_Key_Og") && arrowKeyCollided)
        {
            arrowKeyCollided = false;
            Debug.Log("Arrow_Key_Ogï¿½ï¿½ ï¿½æµ¹ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Ï´ï¿½. ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½î°¡ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Ë´Ï´ï¿½.");
        }
    }

    private void ChangeVehicleControls(GameObject vehicle, bool reverseControls)
    {
        VehicleController vehicleController = vehicle.GetComponent<VehicleController>();
        if (vehicleController != null)
        {
<<<<<<< HEAD
=======
            // W Å°·Î ÀüÁø, S Å°·Î ÈÄÁø, A Å°·Î ÁÂÈ¸Àü, D Å°·Î ¿ìÈ¸Àü
>>>>>>> parent of 412c208 (Merge branch 'main' of https://github.com/injaeG/Capstonesk)
            vehicleController.controls.throttle = reverseControls ? -1 : 1;
            vehicleController.controls.brakes = reverseControls ? 1 : 0;
            vehicleController.controls.steering = reverseControls ? -Input.GetAxis("Horizontal") : Input.GetAxis("Horizontal");
        }
    }
}