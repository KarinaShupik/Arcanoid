using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BricksManager : MonoBehaviour
{

    #region Singleton

    private static BricksManager _instance;
    public static BricksManager Instance => _instance;

    /*��� ����� � �������� �������� ����� Unity � ����������� ��� �������� ��'����. 
     * � ����� ���������� ������ Singleton, ��� ����������� �������� ����� ������ ���������� 
     * ��'���� BricksManager � ����. ���� ��� ���� ���������, �� ���������, 
     * ������ �������������� ��������� �� �������� ���������.*/
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    private int maxRows = 17;
    private int maxCols = 12;
    private GameObject bricksContainer;
    private float initialBrickSpawnPositionX = -1.96f;
    private float initialBrickSpawnPositionY = 3.325f;
    private float shiftAmount = 0.365f;

    public Sprite[] Sprites;

    public Color[] BrickColors;

    public List<Brick> RemainingBricks { get; set; }

    public List<int[,]> LevelsData {  get; set; }

    public int initialBricksCount { get; set; }

    public int CurrentLevel;

    public Brick brickPrefab;

    public static event Action OnLevelLoaded;



    /*��� ����� ����������� ����� ������ ������ ��������� �����.
     * � ����� ������������ ��������� ��� ������ (bricksContainer),
     * ������������� ������������ ���� ��� (LevelsData), � ����������� 
     * ����� �� ��������� ������ GenerateBricks().*/
    private void Start()
    {
        this.bricksContainer = new GameObject("BricksContainer");
        this.LevelsData = this.LoadLevelData();
        this.GenerateBricks();
         
    }

    /*��� ����� ������ ����� ��� ��������� ���� ���. ³� ������� ������ ������ RemainingBricks,
     * ���� ��������������� ��� ��������� �������� �� �������� �����. � ���� ����������� ����� 
     * ����� ������� ������� ���� (��������� maxRows � maxCols), ���������� ��� ����� � ����, � 
     * ���� �� ����� 0, ����������� ���� ����� � ������������� prefab (brickPrefab). ����� 
     * ������������, ������� ����������� ���������, ������, ���� � ������� ��������. 
     * ϳ��� ����� ����� �������� �� ������ RemainingBricks � �������� ������� ��������� ����� 
     * (currentSpawnX � currentSpawnY). ������� �� �� Z (zShift) ��������������� ��� ������������
     * ����������� ������� ����������� ����� � ����������� �������. ϳ��� ���������� ��������� 
     * ���������� ����, �������� ������� currentSpawnY �������� ����, � ������ ������������ ��� 
     * ���������� ���� ���������� ����.*/
    private void GenerateBricks()
    {
        this.RemainingBricks = new List<Brick>();
        int[,] currentLevelData = this.LevelsData[this.CurrentLevel];
        float currentSpawnX = initialBrickSpawnPositionX;
        float currentSpawnY = initialBrickSpawnPositionY;
        float zShift = 0;

        for (int row = 0; row < this.maxRows; row++)
        {
            for (int col = 0; col < this.maxCols; col++)
            {
                int brickType = currentLevelData[row, col];

                if (brickType > 0)
                {
                    Brick newBrick = Instantiate(brickPrefab, new Vector3(currentSpawnX, currentSpawnY, 0.0f - zShift), Quaternion.identity) as Brick;
                    newBrick.Init(bricksContainer.transform, this.Sprites[brickType - 1], this.BrickColors[brickType], brickType);

                    this.RemainingBricks.Add(newBrick); 
                    zShift += 0.0001f;
                }

                currentSpawnX += shiftAmount;
                if (col + 1 == this.maxCols)
                {
                    currentSpawnX = initialBrickSpawnPositionX;
                }
            }

            currentSpawnY -= shiftAmount;   
        }
        this.initialBricksCount= this.RemainingBricks.Count;
        OnLevelLoaded?.Invoke();
    }

    /*��� ����� ��������� ��� ���� ��� � ���������� �����. ³� ����������� ����� Resources.Load() ��� ������������ ���������� ������� "levels". 
     * ��������� ������ ������ ������������ ���� � ������, �� ����� ��� ����������� ���� �����, � �������� ��������� ����� ������� ������.
       � ���� �� ������ ���������� ������� ����������� ����� ����� �� ����� ����� ��������� ������� �� ��������� ������ Split(). ��� ������� ����� ������� ���������� ��������:
       ���� �������� ��������� "--", �� ������ ����� ��������� ����. ��� ���������� �������� ����� currentLevel �������� �� ������ levelsData, � currentLevel ������������ �� ����� ���������� ����� ������� maxRows �� maxCols.
       � ������ �������, ������ �������� ����� �������������� � ������������� ��� �� ��������� int.Parse(), � �� �������� ���������� � ������� ������� currentLevel[currentRow, col].
       ϳ��� ���������� ������� ��� ����� ���������� �������, ������ levelsData �������� �������� ������ � ������������� ������� ���� ���. ����� ������� levelsData.*/
    private List<int[,]> LoadLevelData()
    {
        TextAsset text = Resources.Load("levels") as TextAsset;

        string[] rows = text.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        List<int[,]> levelsData = new List<int[,]>();
        int[,] currentLevel = new int[maxRows, maxCols];
        int currentRow = 0;

        for (int row = 0; row < rows.Length; row++)
        {
            string line = rows[row];

            if (line.IndexOf("--") == -1)
            {
                string[] bricks = line.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
                for (int col = 0; col < bricks.Length; col++)
                {
                    currentLevel[currentRow, col] = int.Parse(bricks[col]);
                }
                currentRow++;
            }
            else
            {
                currentRow = 0;
                levelsData.Add(currentLevel);
                currentLevel = new int[maxRows, maxCols];
            }
        }
        return levelsData;  
    }

    /*��� ����� ��������� �������� ����� ���. ³� ������ �������� level, ���� ������� ����� ����, �� �� ���� ������������.*/
    public void LoadLevel(int level)
    {
        this.CurrentLevel = level;
        this.ClearRemainingBricks();
        this.GenerateBricks();  
    }

    /*��� ��������� ����� ����� ������ RemainingBricks, �������� �� ������ ��'���� Brick, �� ���������� � �� � ������������ ����.
     * �������������� ���� foreach, ��� ������� ��'���� Brick � ������ RemainingBricks ����������� ����� Destroy(), ��� ������� ��'���
     * gameObject �����. ����������� ToList(), ��� �������� ���� ������ ��������, ������� ����������� ������ ���� ������������� �� ��� ��������.*/
    private void ClearRemainingBricks()
    {
        foreach (Brick brick in this.RemainingBricks.ToList())
        {
            Destroy(brick.gameObject);
        }
    }

    /*��� ����� ��������� ��������� ����� ��� ���� �������� ���������� ��������� ����.*/
    public void LoadNextLevel()
    {
        this.CurrentLevel++;

        if (this.CurrentLevel >= this.LevelsData.Count)
        {
            GameManager.Instance.ShowVictoryScreen();
        }
        else
        {
            this.LoadLevel(this.CurrentLevel);
        }
    }

    
}
