using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    private bool gamePaused = false; // 게임이 일시정지되었는지 여부

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 이미 존재하는 경우 이 인스턴스는 파괴
        }
    }

    public bool IsGamePaused()
    {
        return gamePaused;
    }

    public void PauseGame()
    {
        gamePaused = true;
        Time.timeScale = 0f; // 게임 일시 정지
    }

    public void ResumeGame()
    {
        gamePaused = false;
        Time.timeScale = 1f; // 게임 재개
    }
}