using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static event Action<Ball> OnBallDeath;

    /*Цей метод викликається, коли м'яч "помирає" або видаляється з гри. У методі спрацьовує 
     * подія OnBallDeath, яка сповіщає про втрату м'яча. Потім викликається метод Destroy для 
     * видалення гри з об'єктом м'яча з сцени. Другим параметром методу Destroy є час, через який 
     * об'єкт буде видалено (у даному випадку, 1 секунда).*/
    public void Die()
    {
        OnBallDeath.Invoke(this);
        Destroy(gameObject, 1);
    }

}
