using UnityEngine;

public class Sound_Handler : MonoBehaviour
{
    public AudioSource rightSound; // Right_Sound 태그 객체와 충돌 시 재생할 AudioSource

    void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트의 태그 확인
        if (collision.gameObject.CompareTag("Car") && collision.gameObject.CompareTag("Right_Sound"))
        {
            // Right_Sound 태그를 가진 객체와 Car 태그를 가진 객체가 충돌했을 때 소리 재생
            PlayRightSound();
        }
    }

    void PlayRightSound()
    {
        // Right_Sound 태그 객체와 충돌 시 소리 재생
        if (rightSound != null)
        {
            rightSound.Play();
        }
        else
        {
            Debug.LogWarning("Right sound is not assigned to Sound_Handler.");
        }
    }
}