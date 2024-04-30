using UnityEngine;
using System.Collections;

public class Road_Remover : MonoBehaviour
{
    public float destroyDelay = 2f; // 도로 삭제 지연 시간 (초)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            // 태그가 "Car"인 객체가 트리거에 충돌하면 도로 삭제 코루틴 시작
            StartCoroutine(DestroyRoad());
        }
    }

    private IEnumerator DestroyRoad()
    {
        // destroyDelay 시간 동안 대기 후 도로 삭제
        yield return new WaitForSeconds(destroyDelay);

        // 부모 게임 오브젝트(프리팹) 삭제
        Destroy(gameObject.transform.parent.gameObject);
    }   
}