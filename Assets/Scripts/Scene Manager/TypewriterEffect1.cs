using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class TypewriterEffectWithVoice : MonoBehaviour
{
    public float letterDelay = 0.1f; // 한 글자가 표시되는 데 걸리는 시간
    public AudioClip[] voiceClips; // 텍스트에 대응하는 음성 클립 배열
    private string[] texts; // 모든 텍스트를 포함하는 배열
    private int currentIndex = 0; // 현재 텍스트의 인덱스

    private Text textComponent;
    private AudioSource audioSource;

    void Start()
    {
        textComponent = GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();

        // 텍스트 요소에서 전체 텍스트 가져오기
        texts = new string[] {
            "긴급 속보입니다. 오늘 새벽 수 많은 인명피해를 낸 음주운전 차량이 추격전 끝에 멈춰 섰다는 내용이 전해졌습니다.",
            "설마 저게 나인가..?",
            "계속되는 어지럼증과 메스꺼움이 속보의 주인공이 나라고 말해주고 있는 것 같았다",
            "음주 또는 약물의 영향으로 정상적인 운전이 곤란한 상태에서 자동차등을 운전하여 사람을 상해에 이르게 한 사람은 1년 이상 15년 이하의 징역 또는 1천만원 이상 3천만원 이하의 벌금에 처해지고, 사망에 이르게 한 사람은 무기 또는 3년 이상의 징역에 처해집니다.",
            "한잔 술로 포기하기엔 모두의 인생이 너무 아름답습니다.",
        };

        // 타자 효과 시작
        StartCoroutine(ShowTextAndVoice());
    }

    IEnumerator ShowTextAndVoice()
    {
        while (currentIndex < texts.Length)
        {
            string currentText = texts[currentIndex];
            textComponent.text = ""; // 이전 텍스트 삭제

            for (int i = 0; i < currentText.Length; i++)
            {
                textComponent.text += currentText[i];

                // letterDelay 시간만큼 기다림
                yield return new WaitForSeconds(letterDelay);
            }

            // 음성 재생
            PlayVoice(currentIndex);

            // 음성이 재생되는 동안 키 입력을 기다림
            while (audioSource.isPlaying)
            {
                if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
                {
                    audioSource.Stop();
                    break;
                }
                yield return null;
            }

            // 마우스 버튼 혹은 아무 키를 입력받기 위해 대기
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.anyKeyDown);

            // 현재 텍스트의 인덱스 증가
            currentIndex++;

            // 대기 시간 후에 다음 텍스트 생성
            yield return new WaitForSeconds(0.1f); // 다음 텍스트가 나타나기 전 대기 시간
        }

        // 모든 텍스트가 표시된 후에 다음 씬으로 전환
        LoadNextScene();
    }

    void PlayVoice(int index)
    {
        if (index < voiceClips.Length && voiceClips[index] != null)
        {
            audioSource.clip = voiceClips[index];
            audioSource.Play();
        }
    }

    void LoadNextScene()
    {
        // 다음 씬의 인덱스를 얻어옴
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // 마지막 씬이 아닌 경우에만 다음 씬으로 전환
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("다음 씬이 없습니다.");
        }
    }
}