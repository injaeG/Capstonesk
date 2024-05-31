using UnityEngine;

public class EventFogController : MonoBehaviour
{
    // Main 카메라에 할당된 BadTVEffect 스크립트를 참조하기 위한 변수
    private BadTVEffect badTVEffect;

    // Start 함수에서 Main 카메라에 할당된 BadTVEffect 스크립트를 찾아 참조함
    private void Start()
    {
        // Main 카메라를 찾음
        Camera mainCamera = Camera.main;

        // Main 카메라에 할당된 BadTVEffect 스크립트를 찾음
        badTVEffect = mainCamera.GetComponent<BadTVEffect>();

        // 만약 Main 카메라에 BadTVEffect 스크립트가 없다면 오류 메시지를 출력
        if (badTVEffect == null)
        {
            Debug.LogError("Main camera does not have BadTVEffect script attached!");
        }
    }

    // 충돌했을 때 호출되는 함수
    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 객체가 Car 태그를 가진 경우
        if (other.CompareTag("Car"))
        {
            // BadTVEffect 스크립트의 thickDistort 값을 변경
            badTVEffect.thickDistort = 3.0f;
        }
    }

    // 키 입력을 감지하여 ThickDistort 값을 원래대로 복구
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            badTVEffect.thickDistort = 0.7f; // 원래의 값으로 복구
        }
    }
}