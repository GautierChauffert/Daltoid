using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Damageable;

public class ShotGun : Weapon
{
	[SerializeField] private int nbrOfShots = 5;
	[SerializeField] private float startTimeShots = 0;
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
			CameraShake2D.instance.Shake(1.5f);
			this.StartCoroutine(this.CreateNbr(nbrOfShots));
			timeBtwShots = startTimeShots;
			AudioManager.instance.PlaySound("ShotGun");
		}
	}

	private IEnumerator CreateNbr(int nbr)
	{
		for(int i =0; i < nbr; i++) {
			this.SpawnBullet();
			yield return null;
		}
	}
}

