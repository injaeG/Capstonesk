using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spotlight : MonoBehaviour, IInteractable
{
    private Animator animator;
    private bool spotlighton = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        //  값을 반전시킵니다.
        spotlighton = !spotlighton;

        // 애니메이터의 bool 파라미터를 설정하여 애니메이션 상태를 변경합니다.
        animator.SetBool("spotlighton", spotlighton);

        // 디버그 로그를 통해 문의 상태를 출력합니다.
        if (spotlighton)
        {
            Debug.Log("전조등을 킵니다.");
        }
        else
        {
            Debug.Log("전조등을 끕니다.");
        }
    }
}
