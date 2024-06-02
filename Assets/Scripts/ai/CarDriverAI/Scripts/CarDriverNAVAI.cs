using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarDriverNAVAI : MonoBehaviour
{
    [SerializeField] private Transform targetPositionTransform;
    [SerializeField] private float stoppingDistance = 15f;
    [SerializeField] private bool zigzagChase = false; // 좌우로 움직이면서 추격하는 방식 여부
    [SerializeField] private float zigzagFrequency = 5f; // 좌우 움직임 빈도

    private NavMeshAgent navMeshAgent;
    private CarDriver carDriver;
    private Vector3 targetPosition;
    private float lastTurnAmount = 0f;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        carDriver = GetComponent<CarDriver>();
    }

    private void Start()
    {
        if (targetPositionTransform != null)
        {
            SetTargetPosition(targetPositionTransform.position);
        }

        // Disable NavMeshAgent's automatic movement
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;
    }

    private void Update()
    {
        if (targetPositionTransform != null)
        {
            SetTargetPosition(targetPositionTransform.position);
        }

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            navMeshAgent.SetDestination(targetPosition);
            Vector3 desiredVelocity = navMeshAgent.desiredVelocity;
            Vector3 steeringDirection = desiredVelocity.normalized;

            if (zigzagChase)
            {
                steeringDirection += transform.right * Mathf.Sin(Time.time * zigzagFrequency);
                steeringDirection.Normalize();
            }

            float forwardAmount = Vector3.Dot(transform.forward, steeringDirection);
            float turnAmount = Vector3.SignedAngle(transform.forward, steeringDirection, Vector3.up) / 45f; // 45 degrees for max turn amount

            // Apply calculated values to the CarDriver component
            carDriver.SetInputs(forwardAmount, turnAmount);
            lastTurnAmount = turnAmount;
        }
        else
        {
            // Stop the car when the target is reached
            carDriver.SetInputs(0f, 0f);
        }
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
