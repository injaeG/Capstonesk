using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public AudioClip collisionSound; // 충돌 시 재생할 오디오 클립
    private AudioSource audioSource; // AudioSource 컴포넌트

    void Start()
    {
        // AudioSource 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();
        
        // AudioSource가 없는 경우 자동으로 추가
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 오디오 클립이 할당되어 있는지 확인
        if (collisionSound == null)
        {
            Debug.LogWarning("충돌 시 재생할 오디오 클립이 설정되지 않았습니다.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 충돌한 객체의 태그가 "Car"인지 확인
        if (collision.gameObject.CompareTag("Car"))
        {
            // 오디오 클립 재생
            if (audioSource != null && collisionSound != null)
            {
                audioSource.PlayOneShot(collisionSound);
            }
        }
    }
}