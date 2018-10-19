using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour {
    private SoundManager soundManager;

    public void LoadGameOver()
    {
        StartCoroutine(DelayTilLoadGameOver());
    }

    private IEnumerator DelayTilLoadGameOver()
    {
        soundManager = GameObject.FindGameObjectWithTag("Sound Manager").GetComponent<SoundManager>();
        yield return new WaitForSeconds(soundManager.GetPlayerDeadSFX().length);
        SceneManager.LoadScene("Game Over");
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
        if (!GameSession.instance) { return; }
        GameSession.instance.ResetGame();
    }

    public void LoadStartMenu()
    {
        SceneManager.LoadScene("Start");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
