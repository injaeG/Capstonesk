using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; // �̱��� ������ ���� �ν��Ͻ�

    void Awake()
    {
        // SoundManager �̱��� �ν��Ͻ� ����
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
        // Right_Sound�� ���
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