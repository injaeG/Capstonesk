using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpeedometerController : MonoBehaviour
{
    public GameObject speedometerNeedle; // Speedometer needle object
    public float maxSpeedometerAngle = 270f; // Maximum angle of the speedometer needle
    public float minSpeedometerAngle = -270f; // Minimum angle of the speedometer needle

    private Quaternion initialNeedleRotation; // Initial rotation of the speedometer needle

    void Start()
    {
        // Store the initial rotation of the speedometer needle
        initialNeedleRotation = speedometerNeedle.transform.localRotation;
    }

    void Update()
    {
        // Get the x-axis input
        float xInput = Input.GetAxis("Horizontal");

        // Calculate the angle of the speedometer needle based on the x-axis input
        float angle = Mathf.Lerp(minSpeedometerAngle, maxSpeedometerAngle, xInput);

        // Rotate the speedometer needle object
        speedometerNeedle.transform.localRotation = initialNeedleRotation * Quaternion.Euler(0, angle, 0);
    }
}
