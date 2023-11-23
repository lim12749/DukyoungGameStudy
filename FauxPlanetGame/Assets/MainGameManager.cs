using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager instance;

   
    public GameObject gameOverUI;

    public bool isGameOver = false;
    private void Awake()
    {
        instance = this;
    }

    public void EndGame()
    {
        gameOverUI.SetActive(true);
    }
    
    public void Continue()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
