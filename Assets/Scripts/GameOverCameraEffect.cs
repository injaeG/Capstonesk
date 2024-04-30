using UnityEngine;
using System.Collections;

public class GameOverCameraEffect : MonoBehaviour
{
    public float cameraZoomOutDuration = 2f; // 카메라 줌아웃 시간
    public float cameraAngleDuration = 1.5f; // 카메라 각도 변경 시간
    public float cameraAngleTarget = 45f; // 카메라 각도 목표값

    private void Start()
    {
        OnGameOver();
    }

    private void OnGameOver()
    {
        StartCoroutine(ZoomOutCamera());
        StartCoroutine(RotateCamera());
    }

    private IEnumerator ZoomOutCamera()
    {
        float startPosition = transform.position.z;
        float targetPosition = startPosition + 10f; // 카메라 줌아웃 거리
        float elapsedTime = 0f;

        while (elapsedTime < cameraZoomOutDuration)
        {
            float newZ = Mathf.Lerp(startPosition, targetPosition, elapsedTime / cameraZoomOutDuration);
            transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, targetPosition);
    }

    private IEnumerator RotateCamera()
    {
        float startAngle = transform.rotation.eulerAngles.x;
        float targetAngle = cameraAngleTarget;
        float elapsedTime = 0f;

        while (elapsedTime < cameraAngleDuration)
        {
            float newAngle = Mathf.Lerp(startAngle, targetAngle, elapsedTime / cameraAngleDuration);
            transform.rotation = Quaternion.Euler(newAngle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(targetAngle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}