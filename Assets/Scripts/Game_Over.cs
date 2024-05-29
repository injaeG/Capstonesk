using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Over : MonoBehaviour
{
    // 게임 오버 여부를 나타내는 변수
    private bool isGameOver = false;

    // 충돌을 감지하는 메서드
    private void OnCollisionEnter(Collision collision)
    {
        // 만약 충돌한 물체가 "Player" 태그를 가진 물체라면
        if (collision.gameObject.CompareTag("Car"))
        {
            // 게임 오버 처리 실행
            GameOver();
        }
    }

    // 게임 오버 처리 메서드
    private void GameOver()
    {
        // 게임 오버 상태를 true로 변경
        isGameOver = true;

        // 게임 오버 동작 추가 가능 (예: 게임 오버 UI를 활성화하거나, 게임 종료 등)
        Debug.Log("Game Over!");

        // 예시로 게임 종료
        // Application.Quit(); // 게임 종료
    }
}