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

    /*��� ����� � �������� �������� ����� Unity � ����������� ��� �������� ��'����.
     * � ����� ���������� ��������� �� ��������� SpriteRenderer, ����������� �� ��'����.*/
    private void Awake()
    {
        this.sr = this.GetComponent<SpriteRenderer>();
   
    }

    /*��� ����� ����������� ��� ������� ��'���� � ����� ����������� � ���� ������. 
     * � ����� ���������� ��������� �� �'��, �� �������� � ������, � ����������� 
     * ����� ApplyCollisionLogic(ball) ��� ������� ����� ��������.*/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        ApplyCollisionLogic(ball);
    }

    /*��� ����� ��������� ����� �������� �'��� � ������. ���������� ������� ���������� 
     * �������� ����� �� �������. ���� ������� �������� ����� ����� ��� ������� ����,
     * ����� ����������� � ������ ���������� ����� BricksManager.Instance.RemainingBricks,
     * ����������� ���� OnBrickDestruction, ����������� ����� ���������� (DestroyEffect) ��
     * ��������� ������ SpawnDestroyEffect(), � ����������� ��'��� ����� ��������� �� ��������� ������ Destroy().*/
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

    /*��� ����� ������� ����� ���������� ��� ������� �����. ³� ������� ��������� ������ ���������� (DestroyEffect)
     * � ������� �����, ���� ���� ���������� ����, ���������� � �������� �����, � ���������� ��� ����� ������ ��
     * ����� �������� startLifetime ������ DestroyEffect. ����� ��������� ���� ��������� ����� �������� �����.*/
    private void SpawnDestroyEffect()
    {
        Vector3 brickPos = gameObject.transform.position;
        Vector3 spawnPosition = new Vector3(brickPos.x, brickPos.y, brickPos.z = 0.2f);
        GameObject effect = Instantiate(DestroyEffect.gameObject, spawnPosition, Quaternion.identity); 

        MainModule mm = effect.GetComponent<ParticleSystem>().main;
        mm.startColor = this.sr.color;

        Destroy(effect, DestroyEffect.main.startLifetime.constant);
    }

    /*��� ����� ����������� ��� ����������� �����. �������������� ����������� ��������� (containerTransform), ������ ����� (sprite), ���� ����� (color) �� ��������*/
    public void Init(Transform containerTransform, Sprite sprite, Color color, int hitpoints)
    {
        this.transform.SetParent(containerTransform);
        this.sr.sprite = sprite;
        this.sr.color = color;
        this.hitPoints = hitpoints;
    }
}
