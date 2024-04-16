using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineOnLookAndInteract : MonoBehaviour
{
    public float interactDistance = 2f;
    public Material outlineMaterial; // 아웃라인을 위한 머티리얼
    private Renderer lastRenderer; // 마지막으로 감지된 오브젝트의 렌더러
    private GameObject lastInteractableObject; // 마지막으로 감지된 상호작용 가능한 오브젝트

    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        // 카메라가 바라보는 방향으로 레이를 발사하여 오브젝트를 감지
        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            if (hit.collider.gameObject.CompareTag("object"))
            {
                // 감지된 오브젝트의 렌더러를 가져옴
                Renderer hitRenderer = hit.collider.GetComponent<Renderer>();
                if (hitRenderer != null && hitRenderer != lastRenderer)
                {
                    ClearLastOutline(); // 이전 오브젝트의 아웃라인 제거
                    // 아웃라인 효과 적용
                    hitRenderer.materials = new Material[] { hitRenderer.material, outlineMaterial };
                    lastRenderer = hitRenderer;
                    lastInteractableObject = hit.collider.gameObject; // 상호작용 가능한 오브젝트 저장
                }
            }
            else
            {
                ClearLastOutline(); // 바라본 오브젝트가 "object" 태그가 아니면 이전 오브젝트의 아웃라인 제거
            }
        }
        else
        {
            ClearLastOutline(); // 감지된 오브젝트가 없으면 이전 오브젝트의 아웃라인 제거
        }

        // 'F' 키를 눌렀고 마지막으로 감지된 오브젝트가 상호작용 가능한 오브젝트일 경우 상호작용 실행
        if (Input.GetKeyDown(KeyCode.F) && lastInteractableObject != null)
        {
            IInteractable interactable = lastInteractableObject.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    void ClearLastOutline()
    {
        if (lastRenderer != null)
        {
            // 아웃라인 머티리얼 제거
            Material[] materials = new Material[] { lastRenderer.material };
            lastRenderer.materials = materials;
            lastRenderer = null;
            lastInteractableObject = null; // 상호작용 가능한 오브젝트 정보 초기화
        }
    }
}




