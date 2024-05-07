using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameOverManager : MonoBehaviour
{
    //public string gameOverSceneName = "GameOverScene"; // 게임 오버 신의 이름
    public GameOverScreenController gameOverScreenController;

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트의 태그 확인
        if (other.CompareTag("Game_Over"))
        {
            // 특정 태그를 가진 물체와 부딪혔으므로 게임 오버 처리 실행
            Debug.Log("aaa");
            GameOver();
        }
    }


    private void GameOver()
    {
        // 게임 오버 처리: 게임 오버 신으로 전환
        //SceneManager.LoadScene(gameOverSceneName);

        gameOverScreenController.ShowGameOverScreen();
    }
}