using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get
        {
            if(m_instance == null)
            {
                //find Scene add Class
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }
    private static GameManager m_instance;

    public int Score = 0;
    //
    public bool isGameOver { get; private set; }

    private void Start()
    {
        //is player die GameOver Event add
        FindObjectOfType<PlayerHealth>().OnDeath += EndGame;
    }
    public void AddScore(int count)
    {
        Score += count;
    }
    public void EndGame()
    {
        //starting Evnet
        isGameOver = true;
        //is gameoverPlay
        
    }
}
