using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class TypewriterEffect1 : MonoBehaviour
{
    public float letterDelay = 0.1f; // 한 글자가 표시되는 데 걸리는 시간
    private string[] texts; // 모든 텍스트를 포함하는 배열
    private int currentIndex = 0; // 현재 텍스트의 인덱스

    void Start()
    {
        // 텍스트 요소에서 전체 텍스트 가져오기
        texts = new string[] {
            "도대체 날 왜 쫓아오는거지?",
            "내가 술을 먹었구나",
            "머리도 아프고 속도 좋지 않아",
            "일단 정신을 차려보자"
        };
        
        // 타자 효과 시작
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        while (currentIndex < texts.Length)
        {
            string currentText = texts[currentIndex];
            GetComponent<Text>().text = ""; // 이전 텍스트 삭제

            for (int i = 0; i < currentText.Length; i++)
            {
                GetComponent<Text>().text += currentText[i];

                // letterDelay 시간만큼 기다림
                yield return new WaitForSeconds(letterDelay);
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