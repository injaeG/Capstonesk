using UnityEngine;
using System.Collections;

public class CameraBlinkEffect : MonoBehaviour
{
    public float blinkInterval = 0.5f; // 깜빡이는 간격
    public float blinkDuration = 0.1f; // 깜빡이는 동안의 시간

    private Camera mainCamera;
    private float blinkTimer;
    private bool isBlinking;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        StartCoroutine(BlinkCoroutine());
    }

    IEnumerator BlinkCoroutine()
    {
        while (true)
        {
            // 눈 깜빡임 효과를 위해 카메라의 시야를 조절
            mainCamera.fieldOfView = 10f;

            // 깜빡이는 동안 기다림
            yield return new WaitForSeconds(blinkDuration);

            // 기본 시야로 되돌림
            mainCamera.fieldOfView = 60f;

            // 다음 깜빡임까지 대기
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}