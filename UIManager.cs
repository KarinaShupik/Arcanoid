using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text Target;
    public Text ScoreText;
    public Text LivesText;

    public int Score { get; set; }

    /*��� ����� ����������� �� ��� ������� �������. 
     * ³� ���������� ��������� ���� ��� ���� OnBrickDestruction, 
     * OnLevelLoaded �� OnLivesLost, �������������� += ��� ��������� ������-��������� �� ����.*/
    private void Awake()
    {
        Brick.OnBrickDestruction += OnBrickDestruction;
        BricksManager.OnLevelLoaded += OnLevelLoaded;
        GameManager.OnLivesLost += OnLivesLost;
    }

    /*��� ����� ����������� ����� ������ ������ ���������. ³� ������� ����� OnLivesLost() � 
     * ��������� �������� GameManager.Instance.AvailableLives ��� ����������� ������� ��������� �����.*/
    private void Start()
    {
        OnLivesLost(GameManager.Instance.AvailableLives);
    }

    /*��� ����� ����������� ��� ����� ����� � ��. ³� ������� ����� LivesText, ������������ 
     * ���� �������� �� ����� remainingLives, ��� �������� ������� ��������� �����.*/
    private void OnLivesLost(int remainingLives)
    {
        LivesText.text = $"LIVES: {remainingLives}";
    }

    /*��� ����� ����������� ���� ������������ ����. ³� ������� ������ Target �� ScoreText.
     * ������� ����� UpdateRemainingBricksText() ��� ��������� ������ Target, ���� �������� 
     * ������� ��������� ��������. ������� ����� UpdateScoreText(0) ��� ��������� ������ ScoreText,
     * ���� �������� �������� �������, ��������� 0 � ����� �������� �������.*/
    private void OnLevelLoaded()
    {
        UpdateRemainingBricksText();
        UpdateScoreText(0);
    }

    /*��� ����� ������� ����� ScoreText �� ����� �������� increment. �������� increment �������� 
     * �� ��������� ������� Score, � ���� �������������� �� �����, �������������� ToString(). 
     * ����� ������� ������������ ������ ��������� '0', ���� ���� ������� ����� 5 �������. 
     * ������, ��������� ����� ������� �������������� �� ����� ScoreText.*/
    private void UpdateScoreText(int increment)
    {
        this.Score += increment;
        string scoreString = this.Score.ToString().PadLeft(5, '0');
        ScoreText.text = $"SCORE:{Environment.NewLine}{scoreString}";
    }

    /*��� ����� ����������� ��� ������� �������. ³� ������� ������ Target �� ScoreText. ������� ����� */
    private void OnBrickDestruction(Brick obj)
    {
        UpdateRemainingBricksText();
        UpdateScoreText(10);
    }

    /**/
    private void UpdateRemainingBricksText()
    {
        Target.text = $"TARGET:{Environment.NewLine}{BricksManager.Instance.RemainingBricks.Count} / {BricksManager.Instance.initialBricksCount}";
    }

    /*UpdateRemainingBricksText() ��� ��������� ������ Target, ��� ���������� ������� ��������� �������� ���� ��������.
     * ������� ����� UpdateScoreText(10) ��� ��������� ������ ScoreText*/
    private void OnDisable()
    {
        Brick.OnBrickDestruction -= OnBrickDestruction;
        BricksManager.OnLevelLoaded -= OnLevelLoaded;
    }
}
