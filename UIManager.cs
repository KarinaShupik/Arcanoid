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

    /*Цей метод викликається під час запуску скрипту. 
     * Він встановлює обробники подій для подій OnBrickDestruction, 
     * OnLevelLoaded та OnLivesLost, використовуючи += для додавання методів-обробників до подій.*/
    private void Awake()
    {
        Brick.OnBrickDestruction += OnBrickDestruction;
        BricksManager.OnLevelLoaded += OnLevelLoaded;
        GameManager.OnLivesLost += OnLivesLost;
    }

    /*Цей метод викликається перед першим кадром оновлення. Він викликає метод OnLivesLost() з 
     * передачею значення GameManager.Instance.AvailableLives для відображення кількості доступних життів.*/
    private void Start()
    {
        OnLivesLost(GameManager.Instance.AvailableLives);
    }

    /*Цей метод викликається при втраті життів у грі. Він оновлює текст LivesText, встановлюючи 
     * його значення на основі remainingLives, щоб показати кількість залишених життів.*/
    private void OnLivesLost(int remainingLives)
    {
        LivesText.text = $"LIVES: {remainingLives}";
    }

    /*Цей метод викликається після завантаження рівня. Він оновлює тексти Target та ScoreText.
     * Викликає метод UpdateRemainingBricksText() для оновлення тексту Target, який відображає 
     * кількість залишених цеглинок. Викликає метод UpdateScoreText(0) для оновлення тексту ScoreText,
     * який відображає поточний рахунок, передаючи 0 в якості приросту рахунку.*/
    private void OnLevelLoaded()
    {
        UpdateRemainingBricksText();
        UpdateScoreText(0);
    }

    /*Цей метод оновлює текст ScoreText на основі значення increment. Значення increment додається 
     * до поточного рахунку Score, а потім перетворюється на рядок, використовуючи ToString(). 
     * Рядок рахунку доповнюється вперед символами '0', якщо його довжина менше 5 символів. 
     * Нарешті, оновлений рядок рахунку встановлюється як текст ScoreText.*/
    private void UpdateScoreText(int increment)
    {
        this.Score += increment;
        string scoreString = this.Score.ToString().PadLeft(5, '0');
        ScoreText.text = $"SCORE:{Environment.NewLine}{scoreString}";
    }

    /*Цей метод викликається при знищенні цеглини. Він оновлює тексти Target та ScoreText. Викликає метод */
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

    /*UpdateRemainingBricksText() для оновлення тексту Target, щоб відобразити кількість залишених цеглинок після знищення.
     * Викликає метод UpdateScoreText(10) для оновлення тексту ScoreText*/
    private void OnDisable()
    {
        Brick.OnBrickDestruction -= OnBrickDestruction;
        BricksManager.OnLevelLoaded -= OnLevelLoaded;
    }
}
