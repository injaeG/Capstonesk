using UnityEngine;

public class MoveForwardGhost : MonoBehaviour
{
    public float speed = 10.0f; // 오브젝트의 이동 속도
    public float fixedY = 1.0f; // 고정할 Y 좌표 값

    //void Update()
    //{
    //    // 'Car' 태그가 있는 오브젝트를 찾습니다.
    //    GameObject car = GameObject.FindGameObjectWithTag("Car");

    //    // 'Car' 태그가 있는 오브젝트가 있는 경우에만 이동합니다.
    //    if (car != null)
    //    {
    //        // 'Car' 오브젝트의 위치를 가져오되, Y 좌표는 고정된 값을 사용합니다.
    //        Vector3 targetPosition = new Vector3(car.transform.position.x, fixedY, car.transform.position.z);

    //        // 현재 오브젝트의 위치를 'Car' 태그가 있는 오브젝트의 위치로 부드럽게 이동시킵니다.
    //        Vector3 newPosition = Vector3.Lerp(new Vector3(transform.position.x, fixedY, transform.position.z), targetPosition, speed * Time.deltaTime);

    //        // 이동한 위치를 현재 오브젝트에 적용합니다.
    //        transform.position = newPosition;

    //        // 오브젝트를 'Car' 오브젝트의 방향으로 회전시킵니다.
    //        Vector3 lookDirection = new Vector3(car.transform.position.x, transform.position.y, car.transform.position.z);
    //        transform.LookAt(lookDirection);
    //    }
    //}
}
