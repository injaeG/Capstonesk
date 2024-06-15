using UnityEngine;

public class MoveForwardGhost : MonoBehaviour
{
    public float speed = 10.0f; // 오브젝트의 이동 속도
    public float fixedYOffset = 1.0f; // 지형 위에서 고정할 Y 좌표 오프셋 값
    public float raycastDistance = 100.0f; // 레이캐스트 거리

    //void Update()
    //{
    //    // 'Car' 태그가 있는 오브젝트를 찾습니다.
    //    GameObject car = GameObject.FindGameObjectWithTag("Car");

    //    // 'Car' 태그가 있는 오브젝트가 있는 경우에만 이동합니다.
    //    if (car != null)
    //    {
    //        // 'Car' 오브젝트의 위치를 가져옵니다.
    //        Vector3 targetPosition = car.transform.position;

    //        // 지형의 높이를 감지하기 위해 레이캐스트를 아래 방향으로 발사합니다.
    //        RaycastHit hit;
    //        Vector3 raycastOrigin = new Vector3(targetPosition.x, targetPosition.y + raycastDistance, targetPosition.z);

    //        if (Physics.Raycast(raycastOrigin, Vector3.down, out hit, raycastDistance * 2))
    //        {
    //            // 레이캐스트가 지형에 닿았을 때, 해당 지형의 높이에 고정 오프셋을 더한 값을 사용합니다.
    //            targetPosition.y = hit.point.y + fixedYOffset;
    //        }
    //        else
    //        {
    //            // 레이캐스트가 지형을 감지하지 못한 경우, 기본 Y 오프셋을 적용합니다.
    //            targetPosition.y = fixedYOffset;
    //        }

    //        // 현재 오브젝트의 위치를 'Car' 태그가 있는 오브젝트의 위치로 부드럽게 이동시킵니다.
    //        Vector3 newPosition = Vector3.Lerp(new Vector3(transform.position.x, transform.position.y, transform.position.z), targetPosition, speed * Time.deltaTime);

    //        // 이동한 위치를 현재 오브젝트에 적용합니다.
    //        transform.position = newPosition;

    //        // 오브젝트를 'Car' 오브젝트의 방향으로 회전시킵니다.
    //        Vector3 lookDirection = new Vector3(car.transform.position.x, transform.position.y, car.transform.position.z);
    //        transform.LookAt(lookDirection);
    //    }
    //}
}