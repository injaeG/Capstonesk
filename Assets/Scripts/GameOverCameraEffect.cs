using UnityEngine;
using System.Collections;

public class GameOverCameraEffect : MonoBehaviour
{
    public float cameraZoomOutDuration = 2f; // ī�޶� �ܾƿ� �ð�
    public float cameraAngleDuration = 1.5f; // ī�޶� ���� ���� �ð�
    public float cameraAngleTarget = 45f; // ī�޶� ���� ��ǥ��

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
        float targetPosition = startPosition + 10f; // ī�޶� �ܾƿ� �Ÿ�
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