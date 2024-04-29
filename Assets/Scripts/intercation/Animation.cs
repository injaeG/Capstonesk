using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    private Animator animator; // Animator 컴포넌트를 참조하기 위한 변수
    public bool isAnimationPlaying = false; // 애니메이션 재생 상태를 추적하기 위한 bool 변수
    private int trueCount = 0; // 애니메이션의 bool 값이 true가 된 횟수를 카운트하기 위한 변수
    public GameObject objectToDisable; // 비활성화할 오브젝트를 지정하기 위한 변수

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

            // 애니메이션의 bool 값이 true로 설정되면 trueCount를 증가시킵니다.
            if (isAnimationPlaying)
            {
                trueCount++;
                // trueCount가 3 이상이 되면
                if (trueCount >= 3)
                {
                    // 지정된 오브젝트를 비활성화합니다.
                    if (objectToDisable != null)
                    {
                        objectToDisable.SetActive(false);
                    }

                    // 태그가 "ghost"인 모든 오브젝트를 찾아서 삭제합니다.
                    GameObject[] ghostsToDelete = GameObject.FindGameObjectsWithTag("ghost");
                    foreach (GameObject ghost in ghostsToDelete)
                    {
                        Destroy(ghost); // 오브젝트를 파괴합니다.
                    }

                    trueCount = 0; // 오브젝트가 파괴되면 trueCount를 0으로 초기화합니다.
                }
            }
        }
    }
}
