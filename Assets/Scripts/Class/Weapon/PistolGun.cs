using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Damageable;

public class PistolGun : Weapon
{
	public override void Shoot()
	{
		// the pistol is one shot at each time
		if(owner.isFiring) {
			return;
		}

		CameraShake2D.instance.Shake(0.2f);
		AudioManager.instance.PlaySound("Pistol");

		this.SpawnBullet();
	}
}
