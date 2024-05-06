using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public string gameOverSceneName = "GameOverScene";

    private void Awake()
    {
        GameOver();
    }

    private void GameOver()
    {
        SceneManager.LoadScene(gameOverSceneName);
    }
}