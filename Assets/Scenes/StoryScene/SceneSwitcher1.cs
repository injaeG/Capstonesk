using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneSwitcher1 : MonoBehaviour
{
    public float delayBeforeSwitch = 10f; // 전환 전 대기할 시간
    public string nextSceneName = "StoryScene_2"; // 다음으로 전환할 씬의 이름

    void Start()
    {
        StartCoroutine(SwitchSceneAfterDelay());
    }

    IEnumerator SwitchSceneAfterDelay()
    {
        // delayBeforeSwitch만큼 대기
        yield return new WaitForSeconds(delayBeforeSwitch);

        // 다음 씬으로 전환
        SceneManager.LoadScene(nextSceneName);
    }
}