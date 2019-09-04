using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class TurrelEnemy : Enemy
{
    public ParticleSystem deathParticle;
    [HideInInspector] public Weapon weapon;
	private ParticleSystem death;

	protected override void Awake()
	{
		base.Awake();

		this.death = this.GetComponentInChildrenWithName<ParticleSystem>("Explosion");
	}

	protected override void Start()
	{
		base.Start();

		// Initialize AI behaviour (this will launch the AI)
		this.behaviour = TurrelEnemyAI.Initialize(this);

		weapon = GetComponentInChildren<Weapon>();
		weapon.Owned(this);
	}

	protected override void Death()
	{
		base.Death();

		AudioManager.instance.PlaySound("Gravitron 2");

        deathParticle.Play();

		this.death.Play();

		this.body.gameObject.SetActive(false);

		Destroy(gameObject, 5f);
	}
}