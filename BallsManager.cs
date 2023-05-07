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

    /*��� ����� � �������� �������� ����� Unity � ����������� ��� �������� ��'����.
     * � ����� �������������� ��������� BallsManager �� ������� (Singleton). 
     * ���� ��������� ��� ����, �� �������� ��������� ���������, ������ �� �������� ���� �� ������ ���������.*/
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

    /*��� ����� ����� � �������� �������� ����� Unity � ����������� ����� ������ ������. � ����� ����������� ����� InitBall(), ���� �������� �'��.*/
    private void Start()
    {
        InitBall();
    }

    /*��� ����� ����� �������� �� �������� ����� Unity � ����������� ����� ����. ���� ��� �� ����������� (IsGameStarted = false),
     * �� ������� �'��� ����������� �� �������� ���������. ���� ���������� ������� ��� ������ ���� (Input.GetMouseButtonDown(0)),
     * �� �'�� ��������� ��������, ������������ isKinematic = false � ������� ���� (Force) �� �������� �����. ����� IsGameStarted
     * �������������� � true, ��� ��������� ������� ���.*/
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

    /*��� ����� ����������� ��� ����������� �'���. �������������� ��������� ������� �'��� ����� � ����������. 
     * ����������� ��������� �'��� (initialBall) � ������������� ������� ballPrefab, � ���������� ��������� 
     * �� ���� ��������� Rigidbody2D (initialBallRb). ������ Balls ������������ � ���������� �'����.*/
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

    /*��� ����� ����������� ��� �������� �'���. ³� ����� �� ������� �'��� � ������ Balls, 
     * �������������� ����� Destroy(), � ���� ������� ����� InitBall() ��� ��������� ������ 
     * ����������� �'���.*/

    public void ResetBall()
    {
        foreach (var ball in this.Balls.ToList()) 
        {
            Destroy(ball.gameObject);
        }

        InitBall();
    }
}
