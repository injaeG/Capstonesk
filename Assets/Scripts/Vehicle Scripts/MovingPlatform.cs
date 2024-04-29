using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speedMultiplier = 1.0f; // 속도 배수 조정 가능
    private Rigidbody rb; // Rigidbody 참조를 위한 변수

    void Start()
    {
        // 시작할 때 Rigidbody 컴포넌트를 가져옵니다.
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Rigidbody의 속도(velocity)에 따라 기계 판의 위치를 업데이트합니다.
        Vector3 newPosition = transform.position + (transform.forward * rb.velocity.magnitude * speedMultiplier * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
    }
}

