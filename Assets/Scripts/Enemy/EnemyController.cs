using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Character
{

    //�����˺��ж���Χ�ܵ��˺�
    void OnTriggerStay2D(Collider2D collision)
    {
        collision.GetComponent<Character>()?.TakeDamage(this);
    }
}
