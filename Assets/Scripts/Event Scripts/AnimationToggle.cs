using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToggle : MonoBehaviour
{
    private Animator animator; // Animator ������Ʈ�� �����ϱ� ���� ����
    private bool isAnimationPlaying = false; // �ִϸ��̼� ��� ���¸� �����ϱ� ���� bool ����

    void Start()
    {
        // ���� �� Animator ������Ʈ�� �����ɴϴ�.
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // F Ű�� ���ȴ��� �� �����Ӹ��� Ȯ��
        if (Input.GetKeyDown(KeyCode.F))
        {
            // �ִϸ��̼� ���¸� ����մϴ�.
            isAnimationPlaying = !isAnimationPlaying;

            // Animator�� bool �Ķ���͸� ������Ʈ�Ͽ� �ִϸ��̼��� �Ѱų� ���ϴ�.
            animator.SetBool("mirror", isAnimationPlaying);
        }
    }
}