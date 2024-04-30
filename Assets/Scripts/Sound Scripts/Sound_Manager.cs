using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; // 싱글톤 패턴을 위한 인스턴스

    void Awake()
    {
        // SoundManager 싱글톤 인스턴스 설정
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayRightSound(AudioSource audioSource)
    {
        // Right_Sound를 재생
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource is missing.");
        }
    }
}