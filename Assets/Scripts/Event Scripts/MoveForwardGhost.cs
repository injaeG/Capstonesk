using UnityEngine;

public class MoveForwardGhost : MonoBehaviour
{
    public float speed = 10.0f; // 오브젝트의 이동 속도

    void Update()
    {
        // 'Car' 태그가 있는 오브젝트를 찾습니다.
        GameObject car = GameObject.FindGameObjectWithTag("Car");

        // 'Car' 태그가 있는 오브젝트가 있는 경우에만 이동합니다.
        if (car != null)
        {
            // 현재 오브젝트의 위치를 'Car' 태그가 있는 오브젝트의 위치로 부드럽게 이동시킵니다.
            transform.position = Vector3.Lerp(transform.position, car.transform.position, speed * Time.deltaTime);
        }
    }
}
