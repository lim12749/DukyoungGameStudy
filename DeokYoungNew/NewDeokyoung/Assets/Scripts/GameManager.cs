using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Player Playerobject;

    public GameObject PlayButtton;
    public GameObject GameOver;
    private int score;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        Pause();
    }
    public void PlayFnc()
    {
        score = 0;
        scoreText.text = score.ToString();

        PlayButtton.SetActive(false);
        GameOver.SetActive(false);

        Time.timeScale = 1f;
        Playerobject.enabled = true;

        Pipe[] pipes = FindObjectsOfType<Pipe>();
        
        for(int i=0; i<pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }

    }
    private void Pause()
    {
        Time.timeScale = 0f;
        Playerobject.enabled = false;
    }

    public void GameOverFnc()
    {
        Debug.Log("???");
        GameOver.SetActive(true);
        PlayButtton.SetActive(true);

        Pause();
    }
    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }
}
