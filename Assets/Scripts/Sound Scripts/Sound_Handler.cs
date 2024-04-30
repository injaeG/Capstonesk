using UnityEngine;

public class Sound_Handler : MonoBehaviour
{
    public AudioSource collisionSound1; // ù ��° �浹 �� ����� AudioSource
 

    private int collisionCount = 0;

    void OnCollisionEnter(Collision collision)
    {
        // �浹�� ������Ʈ�� �±� Ȯ��
        if (collision.gameObject.CompareTag("Car"))
        {
            collisionCount++;

            if (collisionCount == 1)
            {
                // ù ��° �浹 �Ҹ� ���
                if (collisionSound1 != null)
                {
                    collisionSound1.Play();
                }
                else
                {
                    Debug.LogWarning("First collision sound is not assigned to Sound_Handler.");
                }
            }
           
            }
        }
    }

