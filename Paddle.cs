using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    #region Singleton

    private static Paddle _instance;
    public static Paddle Instance => _instance;


    /*Цей метод є частиною життєвого циклу Unity і викликається при створенні об'єкта. 
     * В ньому встановлюється екземпляр Paddle як сінглтон (Singleton). Якщо екземпляр 
     * вже існує, то поточний екземпляр знищується, інакше він призначає себе як єдиний екземпляр.*/
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

    private Camera mainCamera;
    private float paddleInitialY;
    private float defaultPaddleWidthInPixel = 200;
    private float defaultLeftClamp = 135;
    private float defaultRightClamp = 410;
    private SpriteRenderer sr;

    /*Цей метод також є частиною життєвого циклу Unity і викликається перед першим кадром. 
     * В ньому ініціалізуються змінні, такі як основна камера, початкова позиція платформи, компонент SpriteRenderer і т.д.*/
    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        paddleInitialY = this.transform.position.y;
        sr = GetComponent<SpriteRenderer>();    
    }

    /*Цей метод також належить до життєвого циклу Unity і викликається кожен кадр. 
     * В ньому викликається метод PaddleMovement(), який відповідає за рух платформи.*/
    private void Update()
    {
        PaddleMovement();
    }

    /*Цей метод відповідає за рух платформи відповідно до позиції миші. Він обчислює обмежувальні 
     * значення для руху платформи на основі розміру платформи і позиції миші, і потім змінює 
     * позицію платформи відповідно до позиції миші.*/
    private void PaddleMovement()
    {
        float paddleShift = (defaultPaddleWidthInPixel - ((defaultPaddleWidthInPixel / 2) * this.sr.size.x)) / 2;
        float leftClamp = defaultLeftClamp - paddleShift;
        float rightClamp = defaultRightClamp + paddleShift;
        float mousePositionPixels = Mathf.Clamp(Input.mousePosition.x, leftClamp, rightClamp);
        float mousePositionWorldX = mainCamera.ScreenToWorldPoint(new Vector3(mousePositionPixels, 0, 0)).x;
        this.transform.position = new Vector3(mousePositionWorldX, paddleInitialY, 0);

    }

    /*Цей метод викликається, коли платформа зіштовхується з іншим об'єктом, в даному випадку - м'ячем.
     * Він перевіряє, чи тег об'єкта, з яким відбулось зіткнення, є "Ball". Якщо так, то відбувається 
     * обробка зіткнення: зупиняється рух м'яча, визначається точка зіткнення і центр платформи, 
     * розраховується різниця між цими точками*/
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ball")
        {

            Rigidbody2D ballRb = coll.gameObject.GetComponent<Rigidbody2D>();
            Vector3 hitPoint = coll.contacts[0].point;
            Vector3 paddleCenter = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
            

            ballRb.velocity = Vector2.zero; 

            float difference = paddleCenter.x - hitPoint.x;

            if(hitPoint.x < paddleCenter.x)
            {
                ballRb.AddForce(new Vector2(-(Mathf.Abs(difference * 200)), BallsManager.Instance.initialBallSpeed));
            }
            else
            {
                ballRb.AddForce(new Vector2((Mathf.Abs(difference * 200)), BallsManager.Instance.initialBallSpeed));
            }
        }
    }
}
