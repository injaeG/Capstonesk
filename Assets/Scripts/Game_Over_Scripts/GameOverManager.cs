using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public string gameOverSceneName = "GameOverScene"; // ���� ���� ���� �̸�

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� �±� Ȯ��
        if (other.CompareTag("car") && other.gameObject.CompareTag("Game_Over"))
        {
            // Ư�� �±׸� ���� ��ü�� �ε������Ƿ� ���� ���� ó�� ����
            GameOver();
        }
    }

    private void GameOver()
    {
        // ���� ���� ó��: ���� ���� ������ ��ȯ
        SceneManager.LoadScene(gameOverSceneName);
    }
}