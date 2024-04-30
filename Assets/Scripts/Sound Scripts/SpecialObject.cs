using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialObject : MonoBehaviour
{
    public AudioSource audioSource; // Inspector에서 지정
    public float delayBeforeDestruction = 2f; // 사운드 재생 후 삭제하기 전 대기 시간

    void Start()
    {
        // AudioSource 컴포넌트가 할당되어 있는지 확인합니다.
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing!", gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // "SpecificObjectName"를 특정 오브젝트의 이름과 교체하세요.
        if (other.gameObject.name == "carMAIN" && audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play(); // 사운드 재생
            Destroy(gameObject, delayBeforeDestruction + audioSource.clip.length); // 사운드 재생이 끝난 후 오브젝트 삭제
        }
    }

}
