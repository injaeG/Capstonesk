using UnityEngine;

public class Slender : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            // Destroy the GameObject this script is attached to
            Destroy(gameObject);

            // Reduce fuel amount by 20 for any colliding object
            if (other.GetComponent<VehicleController>() != null)
            {
                other.GetComponent<VehicleController>().fuelAmount -= 20f;
            }
        }
    }
}