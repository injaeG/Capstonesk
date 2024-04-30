using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInteraction : MonoBehaviour
{
    public float interactionDistance = 5f; // 상호작용 가능한 최대 거리

    private Interactable lastInteractable = null; // 마지막으로 상호작용한 객체
    private bool secondaryInteractTriggered = false; // SecondaryInteract가 실행되었는지 여부
    private float secondaryInteractTimer = 0f; // SecondaryInteract가 실행된 이후의 시간
    public GameObject specificObject; // 특정 오브젝트
    void Update()
    {
        RaycastHit hit;
        // 카메라의 위치에서 카메라가 바라보는 방향으로 레이를 발사합니다.
        bool isHit = Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance);
        Interactable interactable = isHit ? hit.collider.GetComponent<Interactable>() : null;

        if (interactable != lastInteractable)
        {
            // 이전 객체의 상호작용을 리셋합니다.
            if (lastInteractable != null)
            {
                lastInteractable.ResetInteraction();
            }

            lastInteractable = interactable;
            secondaryInteractTriggered = false;
            secondaryInteractTimer = 0f;
        }

        // 새로운 객체에 대해 상호작용을 시작합니다.
        if (interactable != null && !interactable.IsInteracting)
        {
            interactable.Interact();
        }

        // 'E' 키를 눌렀을 때, 현재 쳐다보고 있는 객체에 대해 SecondaryInteract 메소드를 실행합니다.
        if (Input.GetKeyDown(KeyCode.E) && interactable != null)
        {
            interactable.SecondaryInteract();
            secondaryInteractTriggered = true;
        }

        // SecondaryInteract가 실행된 이후 5초 이상이 되면 'ghost' 태그가 있는 오브젝트를 삭제합니다.
        if (secondaryInteractTriggered && hit.collider.gameObject == specificObject)
        {
            secondaryInteractTimer += Time.deltaTime;

            if (secondaryInteractTimer >= 5f)
            {
                GameObject ghost = GameObject.FindGameObjectWithTag("ghost");
                if (ghost != null)
                {
                    Destroy(ghost);
                }

                secondaryInteractTriggered = false;
                secondaryInteractTimer = 0f;
            }
        }
    }
}
