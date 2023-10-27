using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;
    public static GameManager Instance
    {
        get
        {
            if(m_instance ==null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }

    public int Score = 0; //KillPoint;

    public bool isGameOver { get; private set; }
    private void Awake()
    {
        if(Instance != this)
        {
            Destroy(gameObject);
        }   
    }
    private void Start()
    {
        //
        FindObjectOfType<PlayerHealth>().OnDeath += EndGame;
    }

    public void AddScore(int count)
    {
        //is not GameOver
        if (!isGameOver)
        {
            Score += count;
            //UI Update
        }
    }
    public void EndGame()
    {

        isGameOver = true; 

        //UnEnable isGameOverUI
    }

}
