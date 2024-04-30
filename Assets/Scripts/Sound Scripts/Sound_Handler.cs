using UnityEngine;

public class Sound_Handler : MonoBehaviour
{
    public AudioSource rightSound; // Right_Sound �±� ��ü�� �浹 �� ����� AudioSource

    void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� �±� Ȯ��
        if (other.CompareTag("Right_Sound"))
        {
            // Right_Sound �±׸� ���� ��ü�� �浹���� �� �Ҹ� ���
            PlayRightSound();
        }
    }

    void PlayRightSound()
    {
        // Right_Sound �±� ��ü�� �浹 �� �Ҹ� ���
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
