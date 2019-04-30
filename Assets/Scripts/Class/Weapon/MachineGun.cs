using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Damageable;

public class MachineGun : Weapon
{
	[SerializeField, Range(0.01f, 0.1f)]
	private float shotRate = 0.05f;
	private float lastShotTime = 0f;

	public override void Shoot()
	{
		// the pistol shoot at shotRate
		if(Time.time - lastShotTime < shotRate) {
			return;
		}

		CameraShake2D.instance.Shake(0.3f);
		AudioManager.instance.PlaySound("Shoot");

		lastShotTime = Time.time;

		this.SpawnBullet();
	}
}
