using UnityEngine;

public class Sound_Handler : MonoBehaviour
{
    public AudioSource collisionSound1; // 첫 번째 충돌 시 재생할 AudioSource
    private bool hasPlayedFirstCollisionSound = false;

    void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트의 태그 확인
        if (collision.gameObject.CompareTag("Car"))
        {
            // 첫 번째 충돌인지 확인
            if (!hasPlayedFirstCollisionSound)
            {
                // 첫 번째 충돌 소리 재생
                if (collisionSound1 != null)
                {
                    collisionSound1.Play();
                }
                else
                {
                    Debug.LogWarning("First collision sound is not assigned to Sound_Handler.");
                }

                // 첫 번째 충돌 소리가 재생되었음을 표시
                hasPlayedFirstCollisionSound = true;
            }
        }
    }
}