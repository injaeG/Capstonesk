using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPrefabs : MonoBehaviour
{
    private bool hasCollided = false; // 충돌 여부를 나타내는 변수
    private float collisionTime = 0f; // 충돌 시간을 추적하는 변수

    void OnTriggerEnter(Collider other)
    {
        if (!hasCollided && other.CompareTag("Car"))
        {
            hasCollided = true; // 충돌 발생
            Debug.Log("Car과 충돌 감지");

            // 충돌 후 10초 뒤에 부모 객체를 삭제
            Invoke("DestroyParentObject", 10f);
        }
    }

    void DestroyParentObject()
    {
        // 부모 객체가 있는지 확인하고 있다면 삭제
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Debug.LogWarning("Parent object not found.");
        }
    }
}