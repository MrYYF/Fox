using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Character
{

    //进入伤害判定范围受到伤害
    void OnTriggerStay2D(Collider2D collision)
    {
        collision.GetComponent<Character>()?.TakeDamage(this);
    }
}
