using UnityEngine;

public class MoveForwardHitch : MonoBehaviour
{
    public float fixedY = 1.0f; // 고정할 Y 좌표 값

    void Update()
    {
        // 'Car' 태그가 있는 오브젝트를 찾습니다.
        GameObject car = GameObject.FindGameObjectWithTag("Car");

        // 'Car' 태그가 있는 오브젝트가 있는 경우에만 이동합니다.
        if (car != null)
        {
            // 오브젝트를 'Car' 오브젝트의 방향으로 회전시킵니다.
            Vector3 lookDirection = new Vector3(car.transform.position.x, transform.position.y, car.transform.position.z);
            transform.LookAt(lookDirection);
        }
    }
}