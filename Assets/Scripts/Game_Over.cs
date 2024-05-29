using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Over : MonoBehaviour
{
    // ���� ���� ���θ� ��Ÿ���� ����
    private bool isGameOver = false;

    // �浹�� �����ϴ� �޼���
    private void OnCollisionEnter(Collision collision)
    {
        // ���� �浹�� ��ü�� "Player" �±׸� ���� ��ü���
        if (collision.gameObject.CompareTag("Car"))
        {
            // ���� ���� ó�� ����
            GameOver();
        }
    }

    // ���� ���� ó�� �޼���
    private void GameOver()
    {
        // ���� ���� ���¸� true�� ����
        isGameOver = true;

        // ���� ���� ���� �߰� ���� (��: ���� ���� UI�� Ȱ��ȭ�ϰų�, ���� ���� ��)
        Debug.Log("Game Over!");

        // ���÷� ���� ����
        // Application.Quit(); // ���� ����
    }
}