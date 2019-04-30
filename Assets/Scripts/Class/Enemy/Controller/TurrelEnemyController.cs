using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrelEnemyController : MovementController
{
	private TurrelEnemy shooter;
	private Transform parent;

	private const float WEAPON_ROTATION_OMEGA = 100f;

	public TurrelEnemyController(TurrelEnemy e) : base(e)
	{
		this.shooter = e;
		this.transform = e.transform;
	}

	public IEnumerator ShootAt(Vector3 target)
	{
		float duration = 0.2f;

		if(this.parent == null) {
			this.parent = this.shooter.weapon.transform.parent;
		}
		
		do {
			// target = target - this.parent.position;
			// // Debug.Log(target);
			// look = Quaternion.LookRotation(target, this.transform.up);
			// this.parent.rotation = Quaternion.RotateTowards(this.parent.rotation, look, WEAPON_ROTATION_OMEGA * Time.deltaTime);
			this.parent.LookAt(target, Vector3.up);

			this.shooter.weapon.Shoot();
			this.shooter.isFiring = true;
			duration -= Time.deltaTime;

			yield return null;
		}
		while(duration > 0f);

		this.shooter.isFiring = false;
	}
}
