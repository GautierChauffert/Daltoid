using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColtGun : Weapon
{
	[SerializeField] private float startTimesShots = 0;
	private float timeBtwShots;

	public void Update()
	{
		if(timeBtwShots > 0) {
			timeBtwShots -= Time.deltaTime;
		}
	}

	public override void Shoot()
	{
		// the pistol is one shot at each time
		if(owner.isFiring) {
			return;
		}

		if(timeBtwShots <= 0) {
			CameraShake2D.instance.Shake(0.5f);
			this.SpawnBullet();
			timeBtwShots = startTimesShots;
			AudioManager.instance.PlaySound("Colt");
		}
	}
}
