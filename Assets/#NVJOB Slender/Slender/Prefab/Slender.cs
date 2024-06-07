using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slender : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the "Car" tag
        if (other.CompareTag("Car"))
        {
            // Destroy the GameObject this script is attached to
            Destroy(gameObject);

            // Reduce fuel amount by 20
            if (other.GetComponent<VehicleController>() != null)
            {
                other.GetComponent<VehicleController>().fuelAmount -= 20f;
            }
        }
    }
}