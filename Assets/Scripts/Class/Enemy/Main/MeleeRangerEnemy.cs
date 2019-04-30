using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MeleeRangerEnemy : Enemy
{
	public float damage = 10f;
	[HideInInspector] public Animator animator;

	protected override void Awake()
	{
		base.Awake();
		animator = GetComponentInChildren<Animator>();
	}

	protected override void Start()
	{
		base.Start();

		// Initialize AI behaviour (this will launch the AI)
		this.behaviour = MeleeRangerEnemyAI.Initialize(this);
	}

	protected override void Death()
	{
		base.Death();

		AudioManager.instance.PlaySound("Death1");

		animator.SetTrigger("Death");
		Destroy(GetComponent<PatrolController>());
		GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

		Destroy(gameObject, 5f);
	}
}