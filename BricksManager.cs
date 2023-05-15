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

    /*Цей метод є частиною життєвого циклу Unity і викликається при створенні об'єкта. 
     * В ньому реалізовано патерн Singleton, щоб забезпечити наявність тільки одного екземпляра 
     * об'єкта BricksManager у сцені. Якщо вже існує екземпляр, він знищується, 
     * інакше встановлюється посилання на поточний екземпляр.*/
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



    /*Цей метод викликається перед першим кадром оновлення сцени.
     * У ньому ініціалізується контейнер для цеглин (bricksContainer),
     * завантажується конфігурація рівнів гри (LevelsData), і генеруються 
     * цегли за допомогою методу GenerateBricks().*/
    private void Start()
    {
        this.bricksContainer = new GameObject("BricksContainer");
        this.LevelsData = this.LoadLevelData();
        this.GenerateBricks();
         
    }

    /*Цей метод генерує цегли для поточного рівня гри. Він створює пустий список RemainingBricks,
     * який використовується для зберігання посилань на залишкові цегли. В циклі проходиться через 
     * кожну позицію решітки рівня (визначену maxRows і maxCols), отримується тип цегли з рівня, і 
     * якщо він більше 0, створюється нова цегла з використанням prefab (brickPrefab). Цегла 
     * ініціалізується, задаючи батьківський контейнер, спрайт, колір і кількість хітпоінтів. 
     * Після цього цегла додається до списку RemainingBricks і зміщується позиція створення цегли 
     * (currentSpawnX і currentSpawnY). Зміщення по осі Z (zShift) використовується для забезпечення
     * правильного порядку відображення цегли в тривимірному просторі. Після завершення генерації 
     * цегельного ряду, значення позиції currentSpawnY зміщується вниз, і процес повторюється для 
     * наступного ряду цегельного рівня.*/
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

    /*Цей метод завантажує дані рівнів гри з текстового файлу. Він використовує метод Resources.Load() для завантаження текстового ресурсу "levels". 
     * Текстовий ресурс містить конфігурацію рівнів у форматі, де кожна лінія представляє один рівень, а значення цегельних блоків розділені комами.
       У циклі по рядках текстового ресурсу розбивається кожен рядок на окремі блоки цегельних значень за допомогою методу Split(). Для кожного блоку значень виконується наступне:
       Якщо знайдено роздільник "--", це означає кінець поточного рівня. Тоді збережений поточний рівень currentLevel додається до список levelsData, а currentLevel ініціалізується як новий двовимірний масив розміром maxRows на maxCols.
       В іншому випадку, розбиті значення цегли перетворюються в цілочисельний тип за допомогою int.Parse(), і це значення зберігається у поточній позиції currentLevel[currentRow, col].
       Після завершення обробки всіх рядків текстового ресурсу, список levelsData міститиме двовимірні масиви з конфігурацією кожного рівня гри. Метод повертає levelsData.*/
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

    /*Цей метод завантажує вказаний рівень гри. Він отримує параметр level, який визначає номер рівня, що має бути завантажений.*/
    public void LoadLevel(int level)
    {
        this.CurrentLevel = level;
        this.ClearRemainingBricks();
        this.GenerateBricks();  
    }

    /*Цей приватний метод очищає список RemainingBricks, знищуючи всі гральні об'єкти Brick, які залишилися в грі з попереднього рівня.
     * Використовуючи цикл foreach, для кожного об'єкту Brick у списку RemainingBricks викликається метод Destroy(), щоб знищити об'єкт
     * gameObject цегли. Використано ToList(), щоб створити копію списку ітерацій, оскільки оригінальний список буде модифікований під час ітерації.*/
    private void ClearRemainingBricks()
    {
        foreach (Brick brick in this.RemainingBricks.ToList())
        {
            Destroy(brick.gameObject);
        }
    }

    /*Цей метод завантажує наступний рівень гри після успішного завершення поточного рівня.*/
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
