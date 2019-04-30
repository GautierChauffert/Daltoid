using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Tools;

public class PlayerGroundController : PlayerController<PlayerGroundController>
{
	private ContactFilter2D contactFilter;
	private ContactPoint2D[] contactBuffer = new ContactPoint2D[5];


	protected override void Awake()
	{
		base.Awake();

		this.contactFilter.layerMask = this.playerData.grounds;
		this.contactFilter.useLayerMask = true;
		this.contactFilter.useTriggers = false;

		this.playerData.groundNormal = Vector2.up;

		Physics2D.queriesStartInColliders = false;
	}

	void LateUpdate()
	{
		// check if grounded after movement
		this.CheckIfGrounded();
		
		// check if ceiled after movement
		this.CheckIfCeiled();
	}

	// This updates the state of pc.isGrounded, it should be called in FixedUpdate
	private void CheckIfGrounded()
	{
		int count;
		float radius, dot;
		Vector2 point;
		Collider2D coll;
		bool ground;

		radius = cl.size.x * 0.475f;
		point = rb.position + Vector2.up * (cl.offset.y + cl.size.x * 0.5f - (cl.size.y * 0.5f + 0.1f));
		count = cl.GetContacts(contactFilter, contactBuffer);
		coll = Physics2D.OverlapCircle(point, radius, contactFilter.layerMask);

		// calculate ground normal
		this.playerData.groundNormal = Vector2.zero;
		for(int i = 0; i < count; i++) {
			dot = Vector2.Dot(contactBuffer[i].normal, Vector2.up);

			if(dot >= this.playerData.slopeThreshold) {
				this.playerData.groundNormal += contactBuffer[i].normal;
			}
		}

		this.playerData.groundNormal.Normalize();

		if(coll != null && this.playerData.groundNormal == Vector2.zero) {
			this.playerData.groundNormal = tr.up;
		}

		ground = (coll != null && this.playerData.groundNormal != Vector2.zero);
		if(ground != pc.inputs.isGrounded) {
			pc.inputs.isGrounded = ground;
		}

		this.playerData.horizontal = (ground) ? -Vector2.Perpendicular(this.playerData.groundNormal).normalized : Vector2.right;
	}

	private void CheckIfCeiled()
	{
		float radius;
		Vector2 point;
		Collider2D coll;
		bool ceil;

		radius = cl.size.x * 0.45f;
		point = rb.position + Vector2.up * (cl.size.x * 0.5f + cl.size.y * 0.5f - cl.offset.y + this.playerData.ceilOffset);
		coll = Physics2D.OverlapCircle(point, radius, contactFilter.layerMask);

		ceil = (coll != null);
		if(ceil != pc.inputs.isCeiled) {
			pc.inputs.isCeiled = ceil;
		}
	}

	void OnDrawGizmos()
	{
		if(rb == null) {
			return;
		}
		
		float radius;
		Vector2 point;

		radius = cl.size.x * 0.45f;
		point = rb.position + Vector2.up * (cl.size.x * 0.5f + cl.size.y * 0.5f - cl.offset.y + this.playerData.ceilOffset);
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(point, radius);

		radius = cl.size.x * 0.475f;
		point = rb.position + Vector2.up * (cl.offset.y + cl.size.x * 0.5f - (cl.size.y * 0.5f + this.playerData.groundOffset));
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(point, radius);
	}
}