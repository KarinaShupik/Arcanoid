using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager _instance;   
    public static GameManager Instance  => _instance;


    /*Цей метод є частиною життєвого циклу Unity та викликається під час запуску скрипту.
     * У ньому встановлюється екземпляр GameManager як сінглтон (Singleton). Якщо екземпляр 
     * вже існує, то поточний екземпляр знищується, інакше він призначає себе як єдиний екземпляр.*/
    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public bool IsGameStarted { get; set; }
    public int Lives { get; set; }

    public int AvailableLives = 3;

    public GameObject gameOverScreen;
    public GameObject victoryScreen;

    public static event Action<int> OnLivesLost;


    /*Цей метод також є частиною життєвого циклу Unity і викликається перед першим кадром. 
     * В ньому встановлюються початкові значення гри, такі як кількість життів, роздільна здатність екрану. 
     * Також відбувається підписка на події випадання м'яча та знищення цеглин.*/
    private void Start()
    {
        this.Lives = this.AvailableLives;
        Screen.SetResolution(540, 960, false);
        Ball.OnBallDeath += OnBallDeath;
        Brick.OnBrickDestruction += OnBrickDestruction;
    }


    /*Цей метод викликається при знищенні цеглини. Він перевіряє, чи не залишилося більше цеглин у BricksManager.
     * Якщо ні, то відбувається скидання м'яча, зупинка гри та завантаження наступного рівня.*/
    private void OnBrickDestruction(Brick obj)
    {
        if (BricksManager.Instance.RemainingBricks.Count <= 0)
        {
            BallsManager.Instance.ResetBall();
            GameManager.Instance.IsGameStarted = false;
            BricksManager.Instance.LoadNextLevel();
        }
    }

    /*Цей метод викликається для перезапуску гри шляхом перезавантаження поточної сцени за допомогою SceneManager.*/
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /*Цей метод викликається при смерті м'яча. Він зменшує кількість життів і перевіряє, чи залишилося ще м'ячів у
     * BallsManager. Якщо немає, перевіряється кількість життів. Якщо кількість життів менше 1, то активується екран
     * гри завершено. В іншому випадку викликається подія OnLivesLost для сповіщення про втрату життя, скидається м'яч,
     * зупиняється гра та завантажується поточний рівень.*/
    private void OnBallDeath(Ball obj)
    {
        if (BallsManager.Instance.Balls.Count <= 0)
        {
            this.Lives--;

            if (this.Lives < 1)
            {
                gameOverScreen.SetActive(true);
            }
            else
            {
                OnLivesLost?.Invoke(this.Lives);
                BallsManager.Instance.ResetBall();
                IsGameStarted = false;
                BricksManager.Instance.LoadLevel(BricksManager.Instance.CurrentLevel);
            }
        }
    }

    /*Цей метод викликається, коли компонент вимикається*/
    private void OnDisable()
    {
        Ball.OnBallDeath -= OnBallDeath;    
    }

    //показує переможний екран, у випадку, коли користувач пройшов всі рівні
    internal void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true); 
    }
}
