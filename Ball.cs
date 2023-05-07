using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static event Action<Ball> OnBallDeath;

    /*��� ����� �����������, ���� �'�� "������" ��� ����������� � ���. � ����� ��������� 
     * ���� OnBallDeath, ��� ������ ��� ������ �'���. ���� ����������� ����� Destroy ��� 
     * ��������� ��� � ��'����� �'��� � �����. ������ ���������� ������ Destroy � ���, ����� ���� 
     * ��'��� ���� �������� (� ������ �������, 1 �������).*/
    public void Die()
    {
        OnBallDeath.Invoke(this);
        Destroy(gameObject, 1);
    }

}
