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


    /*��� ����� � �������� �������� ����� Unity �� ����������� �� ��� ������� �������.
     * � ����� �������������� ��������� GameManager �� ������� (Singleton). ���� ��������� 
     * ��� ����, �� �������� ��������� ���������, ������ �� �������� ���� �� ������ ���������.*/
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


    /*��� ����� ����� � �������� �������� ����� Unity � ����������� ����� ������ ������. 
     * � ����� �������������� �������� �������� ���, ��� �� ������� �����, �������� �������� ������. 
     * ����� ���������� ������� �� ��䳿 ��������� �'��� �� �������� ������.*/
    private void Start()
    {
        this.Lives = this.AvailableLives;
        Screen.SetResolution(540, 960, false);
        Ball.OnBallDeath += OnBallDeath;
        Brick.OnBrickDestruction += OnBrickDestruction;
    }


    /*��� ����� ����������� ��� ������� �������. ³� ��������, �� �� ���������� ����� ������ � BricksManager.
     * ���� �, �� ���������� �������� �'���, ������� ��� �� ������������ ���������� ����.*/
    private void OnBrickDestruction(Brick obj)
    {
        if (BricksManager.Instance.RemainingBricks.Count <= 0)
        {
            BallsManager.Instance.ResetBall();
            GameManager.Instance.IsGameStarted = false;
            BricksManager.Instance.LoadNextLevel();
        }
    }

    /*��� ����� ����������� ��� ����������� ��� ������ ���������������� ������� ����� �� ��������� SceneManager.*/
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /*��� ����� ����������� ��� ����� �'���. ³� ������ ������� ����� � ��������, �� ���������� �� �'���� �
     * BallsManager. ���� ����, ������������ ������� �����. ���� ������� ����� ����� 1, �� ���������� �����
     * ��� ���������. � ������ ������� ����������� ���� OnLivesLost ��� ��������� ��� ������ �����, ��������� �'��,
     * ����������� ��� �� ������������� �������� �����.*/
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

    /*��� ����� �����������, ���� ��������� ����������*/
    private void OnDisable()
    {
        Ball.OnBallDeath -= OnBallDeath;    
    }

    //������ ���������� �����, � �������, ���� ���������� ������� �� ���
    internal void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true); 
    }
}
