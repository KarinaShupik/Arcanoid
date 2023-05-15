using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Brick : MonoBehaviour
{
    public int hitPoints = 1;
    public static event Action<Brick> OnBrickDestruction;
    public ParticleSystem DestroyEffect;

    private SpriteRenderer sr;

    /*Цей метод є частиною життєвого циклу Unity і викликається при створенні об'єкта.
     * В ньому отримується посилання на компонент SpriteRenderer, прикріплений до об'єкта.*/
    private void Awake()
    {
        this.sr = this.GetComponent<SpriteRenderer>();
   
    }

    /*Цей метод викликається при зіткненні об'єкта з іншим коллайдером у двох вимірах. 
     * В методі отримується посилання на м'яч, що зіткнувся з цеглою, і викликається 
     * метод ApplyCollisionLogic(ball) для обробки логіки зіткнення.*/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        ApplyCollisionLogic(ball);
    }

    /*Цей метод застосовує логіку зіткнення м'яча з цеглою. Зменшується кількість залишкових 
     * хітпоінтів цегли на одиницю. Якщо кількість хітпоінтів стане менше або дорівнює нулю,
     * цегла видаляється зі списку залишкових цегли BricksManager.Instance.RemainingBricks,
     * викликається подія OnBrickDestruction, створюється ефект руйнування (DestroyEffect) за
     * допомогою методу SpawnDestroyEffect(), і оригінальний об'єкт цегли знищується за допомогою методу Destroy().*/
    private void ApplyCollisionLogic(Ball ball)
    {
        this.hitPoints--;

        if(this.hitPoints <= 0) 
        {
            BricksManager.Instance.RemainingBricks.Remove(this);    
            OnBrickDestruction?.Invoke(this);
            SpawnDestroyEffect();
            Destroy(this.gameObject);
        }
        else
        {
            this.sr.sprite = BricksManager.Instance.Sprites[this.hitPoints - 1];
        }
    }

    /*Цей метод створює ефект руйнування при знищенні цегли. Він створює екземпляр ефекту руйнування (DestroyEffect)
     * в позиції цегли, задає йому початковий колір, збігаючийся з кольором цегли, і встановлює час життя ефекту на
     * основі значення startLifetime ефекту DestroyEffect. Ефект знищується після закінчення свого життєвого циклу.*/
    private void SpawnDestroyEffect()
    {
        Vector3 brickPos = gameObject.transform.position;
        Vector3 spawnPosition = new Vector3(brickPos.x, brickPos.y, brickPos.z = 0.2f);
        GameObject effect = Instantiate(DestroyEffect.gameObject, spawnPosition, Quaternion.identity); 

        MainModule mm = effect.GetComponent<ParticleSystem>().main;
        mm.startColor = this.sr.color;

        Destroy(effect, DestroyEffect.main.startLifetime.constant);
    }

    /*Цей метод викликається для ініціалізації цегли. Встановлюється батьківський контейнер (containerTransform), спрайт цегли (sprite), колір цегли (color) та початков*/
    public void Init(Transform containerTransform, Sprite sprite, Color color, int hitpoints)
    {
        this.transform.SetParent(containerTransform);
        this.sr.sprite = sprite;
        this.sr.color = color;
        this.hitPoints = hitpoints;
    }
}

