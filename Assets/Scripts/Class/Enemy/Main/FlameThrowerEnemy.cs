using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class FlameThrowerEnemy : Enemy
{
	[HideInInspector] public Weapon weapon_1;
	[HideInInspector] public Weapon weapon_2;

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
		this.behaviour = FlameThrowerEnemyAI.Initialize(this);

		Weapon[] weapons = GetComponentsInChildren<Weapon>();
		weapon_1 = weapons[0];
		weapon_2 = weapons[1];
		weapon_1.Owned(this);
		weapon_2.Owned(this);
	}

	protected override void Death()
	{
		base.Death();

		AudioManager.instance.PlaySound("Gravitron 2");

		this.death.Play();

		this.body.gameObject.SetActive(false);

		Destroy(gameObject, 5f);
	}
}