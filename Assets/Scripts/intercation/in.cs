using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kin : MonoBehaviour, Interactable
{
    private bool isInteracting;
    private Animator animator;
    private AudioSource audioSource;
    public AudioClip[] audioClips; // 오디오 클립 배열
    private int currentAudioIndex = 0; // 현재 재생할 오디오 클립의 인덱스
    public float animationResetDelay = 5f; // 애니메이션을 기본값으로 되돌리기까지의 지연 시간(초)
    public bool playOnce = false; // 오디오 클립을 한 번만 재생할지 여부를 결정하는 변수
    private bool hasPlayedOnce = false; // 오디오 클립이 이미 한 번 재생되었는지를 확인하는 변수
    private bool isAnimationActive = false;
    private float defaultOutlineWidth = 0f; // 아웃라인 기본 너비
    public float activeOutlineWidth = 10f; // 인터랙션 활성화 시 아웃라인 너비
    public bool audioEnabled = false; // 오디오 클립 재생 여부를 결정하는 변수
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        Outline outline = GetComponent<Outline>();
        if (outline != null)
        {
            defaultOutlineWidth = outline.OutlineWidth; // 시작 시 아웃라인 기본 너비 저장
        }
    }

    public bool IsInteracting
    {
        get { return isInteracting; }
        private set { isInteracting = value; }
    }

    public void Interact()
    {
        Outline outline = GetComponent<Outline>();

        if (outline != null)
        {
            // 아웃라인 너비를 증가시켜 인터랙션이 활성화되었음을 나타냅니다.
            outline.OutlineWidth = Mathf.Min(outline.OutlineWidth + 1f, 100f); // 예: 최대 너비를 10으로 제한
            IsInteracting = true;
        }
    }

    public void ResetInteraction()
    {
        Outline outline = GetComponent<Outline>();
        if (outline != null)
        {
            // 아웃라인 너비를 감소시켜 원래 상태로 되돌립니다.
            outline.OutlineWidth = Mathf.Max(outline.OutlineWidth - 1f, 0f); // 예: 최소 너비를 0으로 제한
        }
        IsInteracting = false;
    }


    public void SecondaryInteract()
    {
        isAnimationActive = !isAnimationActive;
        animator.SetBool("mirror", isAnimationActive);

        // audioEnabled가 false이고, playOnce가 false이거나 오디오 클립이 아직 한 번도 재생되지 않았을 때만 오디오 클립 재생
        if (!audioEnabled && (!playOnce || !hasPlayedOnce))
        {
            // 오디오 클립 재생
            PlayNextAudioClip();
            hasPlayedOnce = true; // 오디오 클립이 한 번 재생되었음을 표시
        }

        if (isAnimationActive)
        {
            // 애니메이션 활성화와 resetAnimationEnabled가 true일 때만 코루틴 실행
            StartCoroutine(ResetAnimationAfterDelay());
        }
    }
    IEnumerator ResetAnimationAfterDelay()
    {
        // 현재 애니메이션의 길이를 얻습니다.
        float currentAnimationLength = GetCurrentAnimationLength();

        // 애니메이션 길이 동안 대기
        yield return new WaitForSeconds(currentAnimationLength);

        // 추가 지정된 지연 시간만큼 더 대기
        yield return new WaitForSeconds(animationResetDelay);

        // 지연 시간이 만료되면 애니메이션 상태를 초기화
        if (isAnimationActive)
        {
            isAnimationActive = false;
            animator.SetBool("mirror", isAnimationActive);

        }
    }

    private float GetCurrentAnimationLength()
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfo.Length > 0)
        {
            return clipInfo[0].clip.length;
        }
        else
        {
            // 애니메이션 클립을 찾을 수 없는 경우 기본 지연 시간을 반환
            return animationResetDelay;
        }
    }

    // 다음 오디오 클립을 재생하는 메서드
    void PlayNextAudioClip()
    {
        if (audioClips.Length > 0)
        {
            audioSource.clip = audioClips[currentAudioIndex];
            audioSource.Play();

            // 다음 오디오 클립으로 인덱스 업데이트
            currentAudioIndex = (currentAudioIndex + 1) % audioClips.Length;
        }
    }
}
