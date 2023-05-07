using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWall : MonoBehaviour
{
    /*Цей метод викликається, коли інший коллайдер потрапляє в тригер, до якого прикріплений цей скрипт.
    У методі перевіряється, чи тег коллайдера, що потрапив, є "Ball". Якщо це так, то отримується посилання 
    на компонент Ball з об'єкта, що стикнувся. Далі видаляється цей Ball зі списку Balls у BallsManager.Instance,
    використовуючи метод Remove(). Нарешті, викликається метод Die() на об'єкті ball, щоб виконати логіку смерті для цього Ball.*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ball")
        {
            Ball ball = collision.GetComponent<Ball>();
            BallsManager.Instance.Balls.Remove(ball);
            ball.Die();
        }
    }
}
