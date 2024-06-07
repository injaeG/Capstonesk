using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string nextSceneName; // 전환할 다음 씬의 이름

    void Update()
    {
        // 마우스 클릭 또는 아무 키를 누르면 다음 씬으로 전환
        if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        // 다음 씬으로 전환
        SceneManager.LoadScene(nextSceneName);
    }
}