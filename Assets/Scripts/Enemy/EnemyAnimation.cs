using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    EnemyController enemyController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyController = GetComponent<EnemyController>();
    }

    void Update()
    {
        animator.SetFloat("velocityX", rb.velocity.x);
        animator.SetBool("Dead", enemyController.isDead);
    }
}
