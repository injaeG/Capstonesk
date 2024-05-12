using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameOverManager : MonoBehaviour
{
    //public string gameOverSceneName = "GameOverScene"; // ���� ���� ���� �̸�
    public GameOverScreenController gameOverScreenController;

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� �±� Ȯ��
        if (other.CompareTag("Game_Over"))
        {
            // Ư�� �±׸� ���� ��ü�� �ε������Ƿ� ���� ���� ó�� ����
            Debug.Log("gameover");
            GameOver();
        }
    }


    private void GameOver()
    {
        // ���� ���� ó��: ���� ���� ������ ��ȯ
        //SceneManager.LoadScene(gameOverSceneName);

        gameOverScreenController.ShowGameOverScreen();
    }
}