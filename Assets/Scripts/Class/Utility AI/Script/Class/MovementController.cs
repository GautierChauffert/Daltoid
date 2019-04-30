using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementController
{
	public LivingEntity entity { get; protected set; }
	public Transform transform { get; protected set; }

	private bool previousFlip = false;
	private const float GROUND_DETECTION_RADIUS = 2.5f;
	private Collider2D[] colliderResults = new Collider2D[5];
	private Collider2D myCollider;
	private LayerMask groundLayerMask;

	public MovementController(LivingEntity e)
	{
		this.entity = e;

		this.groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
		this.myCollider = e.GetComponent<Collider2D>();
	}

	protected void UpdateRotationTowards(LivingEntity ent, Vector3 towards)
	{
		Vector3 ground;
		Transform tr;
		float rot;
		bool flip;

		tr = ent.transform;

		ground = this.FindGroundAt(tr.position);
		tr.rotation = Quaternion.LookRotation(towards, ground) * Quaternion.Euler(0,270,0);

		rot = Mathf.Abs(tr.eulerAngles.y);
		flip = (rot > 90 && rot <= 270);
		if(flip != this.previousFlip) {
			this.previousFlip = flip;
			this.entity.SwitchAllSpriteScaleForLight(flip);
		}
	}

	private Vector3 FindGroundAt(Vector2 position)
	{
		int count;
		float min;
		Vector2 point;
		ColliderDistance2D dist2D;
		RaycastHit2D hit;

		min = float.MaxValue;
		point = Vector2.zero;
		count = Physics2D.OverlapCircleNonAlloc(position, GROUND_DETECTION_RADIUS, this.colliderResults, this.groundLayerMask);

		if(count == 0) {
			return Vector3.up;
		}

		foreach(Collider2D c in this.colliderResults) {
			if(c == null) {
				continue;
			}

			dist2D = Physics2D.Distance(c, this.myCollider);

			if(dist2D.distance < min) {
				min = dist2D.distance;
				point = dist2D.pointA;
			}
		}

		hit = Physics2D.Raycast(position, point - position, min + 5f, this.groundLayerMask);

		return hit.normal;
	}
}
