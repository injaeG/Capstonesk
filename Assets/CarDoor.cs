using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDoor : MonoBehaviour, IInteractable
{
    private Animator animator;
    private bool isOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        // isOpen 값을 반전시킵니다.
        isOpen = !isOpen;

        // 애니메이터의 bool 파라미터를 설정하여 애니메이션 상태를 변경합니다.
        animator.SetBool("isOpen", isOpen);

        // 디버그 로그를 통해 문의 상태를 출력합니다.
        if (isOpen)
        {
            Debug.Log("차량 문을 엽니다.");
        }
        else
        {
            Debug.Log("차량 문을 닫습니다.");
        }
    }
}

