using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerEnemyController : MovementController
{
	private FlameThrowerEnemy shooter;

	public FlameThrowerEnemyController(FlameThrowerEnemy e) : base(e)
	{
		this.shooter = e;
		this.transform = e.transform;
	}

	public IEnumerator Fire()
	{
		float duration = 0.2f;
        
        do {
			this.shooter.weapon_1.Shoot();
			this.shooter.weapon_2.Shoot();
			this.shooter.isFiring = true;
			duration -= Time.deltaTime;

			yield return null;
		}
		while(duration > 0f);

		this.shooter.isFiring = false;
	}
}
