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

    /*÷ей метод Ї частиною життЇвого циклу Unity ≥ викликаЇтьс€ при створенн≥ об'Їкта.
     * ¬ ньому встановлюЇтьс€ екземпл€р BallsManager €к с≥нглтон (Singleton). 
     * якщо екземпл€р вже ≥снуЇ, то поточний екземпл€р знищуЇтьс€, ≥накше в≥н призначаЇ себе €к Їдиний екземпл€р.*/
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

    /*÷ей метод також Ї частиною життЇвого циклу Unity ≥ викликаЇтьс€ перед першим кадром. ¬ ньому викликаЇтьс€ метод InitBall(), €кий ≥н≥ц≥ал≥зуЇ м'€ч.*/
    private void Start()
    {
        InitBall();
    }

    /*÷ей метод також належить до життЇвого циклу Unity ≥ викликаЇтьс€ кожен кадр. якщо гра не розпочалас€ (IsGameStarted = false),
     * то позиц≥€ м'€ча вир≥внюЇтьс€ за позиц≥Їю платформи. якщо користувач натискаЇ л≥ву кнопку миш≥ (Input.GetMouseButtonDown(0)),
     * то м'€ч розпочинаЇ рухатис€, установлюючи isKinematic = false ≥ додаючи силу (Force) по напр€мку вгору. «м≥нна IsGameStarted
     * встановлюЇтьс€ в true, щоб позначити початок гри.*/
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

    /*÷ей метод викликаЇтьс€ дл€ ≥н≥ц≥ал≥зац≥њ м'€ча. ¬становлюЇтьс€ початкова позиц≥€ м'€ча поруч з платформою. 
     * —творюЇтьс€ екземпл€р м'€ча (initialBall) з використанн€м префабу ballPrefab, ≥ отримуЇтьс€ посиланн€ 
     * на його компонент Rigidbody2D (initialBallRb). —писок Balls ≥н≥ц≥ал≥зуЇтьс€ з початковим м'€чем.*/
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

    /*÷ей метод викликаЇтьс€ дл€ скиданн€ м'€ча. ¬≥н знищуЇ вс≥ ≥снуюч≥ м'€ч≥ з≥ списку Balls, 
     * використовуючи метод Destroy(), а пот≥м викликаЇ метод InitBall() дл€ створенн€ нового 
     * початкового м'€ча.*/

    public void ResetBall()
    {
        foreach (var ball in this.Balls.ToList()) 
        {
            Destroy(ball.gameObject);
        }

        InitBall();
    }
}
