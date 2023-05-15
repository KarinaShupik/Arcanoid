using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallsManager : MonoBehaviour
{
    #region Singleton

    private static BallsManager _instance;
    public static BallsManager Instance => _instance;

    /*Цей метод є частиною життєвого циклу Unity і викликається при створенні об'єкта.
     * В ньому встановлюється екземпляр BallsManager як сінглтон (Singleton). 
     * Якщо екземпляр вже існує, то поточний екземпляр знищується, інакше він призначає себе як єдиний екземпляр.*/
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

    [SerializeField]
    private Ball ballPrefab;
    private Ball initialBall;
    private Rigidbody2D initialBallRb;
    public float initialBallSpeed = 250;

    public List<Ball> Balls { get; set; }

    /*Цей метод також є частиною життєвого циклу Unity і викликається перед першим кадром. В ньому викликається метод InitBall(), який ініціалізує м'яч.*/
    private void Start()
    {
        InitBall();
    }

    /*Цей метод також належить до життєвого циклу Unity і викликається кожен кадр. Якщо гра не розпочалася (IsGameStarted = false),
     * то позиція м'яча вирівнюється за позицією платформи. Якщо користувач натискає ліву кнопку миші (Input.GetMouseButtonDown(0)),
     * то м'яч розпочинає рухатися, установлюючи isKinematic = false і додаючи силу (Force) по напрямку вгору. Змінна IsGameStarted
     * встановлюється в true, щоб позначити початок гри.*/
    private void Update()
    {
        if (!GameManager.Instance.IsGameStarted)
        {
            //align ball position to the paddle position
            Vector3 paddlePosition = Paddle.Instance.gameObject.transform.position;
            Vector3 ballPosition = new Vector3(paddlePosition.x, paddlePosition.y + .27f, 0);
            initialBall.transform.position = ballPosition;

            if(Input.GetMouseButtonDown(0))
            {
                initialBallRb.isKinematic = false;   
                initialBallRb.AddForce(new Vector2(0, initialBallSpeed));   
                GameManager.Instance.IsGameStarted = true;
            }
        }
    }

    /*Цей метод викликається для ініціалізації м'яча. Встановлюється початкова позиція м'яча поруч з платформою. 
     * Створюється екземпляр м'яча (initialBall) з використанням префабу ballPrefab, і отримується посилання 
     * на його компонент Rigidbody2D (initialBallRb). Список Balls ініціалізується з початковим м'ячем.*/
    private void InitBall()
    {
        Vector3 paddlePosition = Paddle.Instance.gameObject.transform.position;
        Vector3 startingPosition = new Vector3(paddlePosition.x, paddlePosition.y + .27f, 0);
        initialBall = Instantiate(ballPrefab, startingPosition, Quaternion.identity);
        initialBallRb = initialBall.GetComponent<Rigidbody2D>();

        this.Balls = new List<Ball>
        {
            initialBall
        };

    }

    /*Цей метод викликається для скидання м'яча. Він знищує всі існуючі м'ячі зі списку Balls, 
     * використовуючи метод Destroy(), а потім викликає метод InitBall() для створення нового 
     * початкового м'яча.*/

    public void ResetBall()
    {
        foreach (var ball in this.Balls.ToList()) 
        {
            Destroy(ball.gameObject);
        }

        InitBall();
    }
}

