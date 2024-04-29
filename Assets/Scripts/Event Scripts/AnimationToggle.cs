using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToggle : MonoBehaviour
{
    private Animator animator; // Animator 컴포넌트를 참조하기 위한 변수
    private bool isAnimationPlaying = false; // 애니메이션 재생 상태를 추적하기 위한 bool 변수

    void Start()
    {
        // 시작 시 Animator 컴포넌트를 가져옵니다.
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // F 키가 눌렸는지 매 프레임마다 확인
        if (Input.GetKeyDown(KeyCode.F))
        {
            // 애니메이션 상태를 토글합니다.
            isAnimationPlaying = !isAnimationPlaying;

            // Animator의 bool 파라미터를 업데이트하여 애니메이션을 켜거나 끕니다.
            animator.SetBool("mirror", isAnimationPlaying);
        }
    }
}