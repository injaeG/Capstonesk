using UnityEngine;

public class ObjectDeactivator : MonoBehaviour
{
    public Animation animationToggle; // AnimationToggle ��ũ��Ʈ ����
    public GameObject targetObject; // ���� ��� ������Ʈ

    void Update()
    {
        // AnimationToggle ��ũ��Ʈ�� �ִϸ��̼� ��� ���� Ȯ��
        bool isAnimationPlaying = animationToggle != null ? animationToggle.isAnimationPlaying : false;

        // �ִϸ��̼��� true �̻��� �� Ư�� ������Ʈ ��Ȱ��ȭ
        if (isAnimationPlaying)
        {
            if (targetObject != null && targetObject.activeSelf)
            {
                // ��� ������Ʈ�� ��Ȱ��ȭ
                targetObject.SetActive(false);
                Debug.Log("Object deactivated.");
            }
        }
    }
}
