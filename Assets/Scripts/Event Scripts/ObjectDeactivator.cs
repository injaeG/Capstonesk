using UnityEngine;

public class ObjectDeactivator : MonoBehaviour
{
    public Animation animationToggle; // AnimationToggle 스크립트 참조
    public GameObject targetObject; // 꺼질 대상 오브젝트

    void Update()
    {
        // AnimationToggle 스크립트의 애니메이션 재생 상태 확인
        bool isAnimationPlaying = animationToggle != null ? animationToggle.isAnimationPlaying : false;

        // 애니메이션이 true 이상일 때 특정 오브젝트 비활성화
        if (isAnimationPlaying)
        {
            if (targetObject != null && targetObject.activeSelf)
            {
                // 대상 오브젝트를 비활성화
                targetObject.SetActive(false);
                Debug.Log("Object deactivated.");
            }
        }
    }
}
