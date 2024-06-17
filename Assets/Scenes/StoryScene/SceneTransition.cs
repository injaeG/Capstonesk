using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 "car" 태그를 가진 경우
        if (other.CompareTag("Car"))
        {
            // 다음 씬으로 넘어감
            SceneManager.LoadScene("StoryScene_2");
        }
    }
}