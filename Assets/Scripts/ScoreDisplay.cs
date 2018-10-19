using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {
    private Text scoreText;
    private GameSession gameSession;

	void Start () {
        scoreText = GetComponent<Text>();
        gameSession = GameSession.instance;
	}
	
	void Update () {
        scoreText.text = gameSession.GetScore().ToString();
	}
}
