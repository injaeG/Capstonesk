using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GamePlay : MonoBehaviour {

    private Transform playerTransform;
    private Vector3 lastTransform;
    Text health;
    Text gameOver;
    Text score;
    float puan = 0;
	public int carHealth = 100;
    private bool isGameOver = false;
	void Start () {
        health = GameObject.Find("healthText").GetComponent<Text>();
        gameOver = GameObject.Find("gameOverText").GetComponent<Text>();
        score = GameObject.Find("scoreText").GetComponent<Text>();
        playerTransform = GameObject.FindGameObjectWithTag("Car").transform;
        lastTransform = playerTransform.position;
	}
	

	void Update () {
        if(!isGameOver)
            puan += (float)Vector3.Distance(lastTransform, playerTransform.position);

        score.text = "Score :" +(int) puan;
        health.text = "Health :" + carHealth;
		if (carHealth <= 0 || playerTransform.position.y < -5) {
            gameOver.text = "GAME OVER";
            isGameOver = true;
		}
        lastTransform = playerTransform.position;
	}

	void OnCollisionEnter(Collision col){
		
		if (col.collider.tag == "LeftBarrier" || col.collider.tag == "RightBarrier") {
			carHealth -= 10;
		}
		if (col.collider.tag == "AICar") {
			carHealth -= 20;
		}

	}
}
