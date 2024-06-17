using UnityEngine;

public class PlayOnGameStart : MonoBehaviour
{
    public AudioClip startClip; // 시작 시 재생할 오디오 클립
    private AudioSource audioSource; // AudioSource 참조

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            // AudioSource가 없으면 오류를 출력하고 스크립트를 비활성화합니다.
            Debug.LogError("AudioSource not found on GameObject.");
            enabled = false;
            return;
        }

        // 2초 뒤에 PlaySound 메서드를 호출합니다.
        Invoke("PlaySound", 2f);
    }

    void PlaySound()
    {
        // 오디오 클립 설정 및 재생
        audioSource.clip = startClip;
        audioSource.Play();
    }
}