using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Damageable;

[DisallowMultipleComponent]
public abstract class Enemy : LivingEntity
{
	protected Collider2D cl;

	protected override void Awake()
	{
		base.Awake();

		cl = GetComponent<Collider2D>();
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, this.rangeOfView);
	}

	protected override void Death()
	{
		if(this.isDying) {
			return;
		}

		this.behaviour.Remove(this);
		this.cl.enabled = false;
	}
}
