using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int totalEnemies;
    public int totalDiamonds;

    private int enemiesKilled = 0;
    private int diamondsCollected = 0;

    public GameObject levelCompletedUI;
    public GameObject gameOverUI;
    
    public Animator fadeAnimator;

    private void Awake()
    {
        Instance = this;
     
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
       

        CheckLevelCompleted();
    }

    public void DiamondCollected()
    {
        diamondsCollected++;

        CheckLevelCompleted();
    }

    private void CheckLevelCompleted()
    {
        if (enemiesKilled == totalEnemies && diamondsCollected == totalDiamonds)
        {
            LevelComplete();
        }
    }

    public void ShowGameOver()
    {
        StartCoroutine(GameOverRoutine());
    }
    private IEnumerator GameOverRoutine()
    {
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 0f;
        gameOverUI.SetActive(true);
    }

    public void RestartLevel()
    {
       
        StartCoroutine(RestartLevelRoutine());
    }

    private IEnumerator RestartLevelRoutine()
    {
        
       fadeAnimator.SetTrigger("FadeIn");

      
        yield return new WaitForSecondsRealtime(1f);

        fadeAnimator.SetTrigger("FadeOut");


        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void LevelComplete()
    {
        StartCoroutine(LevelCompleteRoutine());
    }

    private IEnumerator LevelCompleteRoutine()
    {
        yield return new WaitForSecondsRealtime(3f);
        levelCompletedUI.SetActive(true);
       
        
        
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
