using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PatrolController), typeof(Rigidbody2D))]
public class MiteEnemy : Enemy
{
    public float damage = 10f;
    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
    }

    protected override void Start()
    {
        base.Start();

        // Initialize AI behaviour (this will launch the AI)
        this.behaviour = MiteEnemyAI.Initialize(this);
    }

    protected override void Death()
    {
        animator.SetTrigger("Death");
        base.Death();
    }
}