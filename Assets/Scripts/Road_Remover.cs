using UnityEngine;
using System.Collections;

public class Road_Remover : MonoBehaviour
{
    public float destroyDelay = 2f;
    public string requiredComponentName = "RoadComponent"; // 필요한 컴포넌트 이름

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            // 태그가 "Car"인 객체와 트리거에 충돌하면 파괴 절차 시작
            StartCoroutine(DestroyRoad());
        }
    }

    private IEnumerator DestroyRoad()
    {
        // destroyDelay 시간 동안 대기
        yield return new WaitForSeconds(destroyDelay);

        // 부모 객체를 가져옴
        var parentObject = gameObject.transform.parent.gameObject;

        // 특정 컴포넌트를 가지고 있는 자식 오브젝트 수를 계산
        int componentCount = 0;
        foreach (Transform child in parentObject.transform)
        {
            if (child.GetComponent(requiredComponentName) != null)
            {
                componentCount++;
            }
        }

        // 특정 컴포넌트를 가진 자식 오브젝트가 3개 이상이면 파괴
        if (componentCount >= 1)
        {
            Destroy(parentObject);
        }
    }
}
