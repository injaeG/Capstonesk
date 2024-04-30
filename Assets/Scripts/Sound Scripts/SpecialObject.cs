using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialObject : MonoBehaviour
{
    public AudioSource audioSource; // Inspector���� ����
    public float delayBeforeDestruction = 2f; // ���� ��� �� �����ϱ� �� ��� �ð�

    void Start()
    {
        // AudioSource ������Ʈ�� �Ҵ�Ǿ� �ִ��� Ȯ���մϴ�.
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing!", gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // "SpecificObjectName"�� Ư�� ������Ʈ�� �̸��� ��ü�ϼ���.
        if (other.gameObject.name == "carMAIN" && audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play(); // ���� ���
            Destroy(gameObject, delayBeforeDestruction + audioSource.clip.length); // ���� ����� ���� �� ������Ʈ ����
        }
    }

}
