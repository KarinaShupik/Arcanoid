using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWall : MonoBehaviour
{
    /*��� ����� �����������, ���� ����� ��������� ��������� � ������, �� ����� ����������� ��� ������.
    � ����� ������������, �� ��� ����������, �� ��������, � "Ball". ���� �� ���, �� ���������� ��������� 
    �� ��������� Ball � ��'����, �� ���������. ��� ����������� ��� Ball � ������ Balls � BallsManager.Instance,
    �������������� ����� Remove(). ������, ����������� ����� Die() �� ��'��� ball, ��� �������� ����� ����� ��� ����� Ball.*/
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
