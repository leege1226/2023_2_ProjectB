using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{

    public int totalHealth = 50;        //ü�� �ʱ� ����

    public void TakeDamage(int damagedAmount)
    {
        totalHealth -= damagedAmount;       //�Ķ���ͷ� ���� ���� ü�¿��� �Ҹ�

        if (totalHealth <= 0)               //0���ϸ� ��� ó��
        {
            totalHealth = 0;
            Destroy(gameObject);
        }
    }

}
