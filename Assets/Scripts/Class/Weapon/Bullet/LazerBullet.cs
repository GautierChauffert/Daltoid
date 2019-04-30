using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Damageable;

public class LazerBullet : Bullet
{
	protected float damagePerSeconds;

	public override void Initialize(LivingEntity o, DamageData d, Vector3 f)
	{
		base.Initialize(o, d, f);

		this.damagePerSeconds = d.damage;
	}

	protected override void OnCollisionEnter2D(Collision2D other)
	{
		base.OnCollisionEnter2D(other);
	}

	protected override void Damage(IDamageableEntity target)
	{
		// only does that
		target.GetDamagedBy(color, damagePerSeconds * Time.deltaTime);
	}

	protected override void Bounce(Collision2D other)
	{
		base.Bounce(other);
	}

	protected override void Disappear()
	{
		// empty yet
	}
}
