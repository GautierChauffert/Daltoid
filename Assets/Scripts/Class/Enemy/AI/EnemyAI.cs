using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAI : UtilityBehaviourAI
{
	protected Transform player;

	private int playerLayerMask;

	private Collider2D[] overlapResults;


	protected virtual void Start()
	{
		this.player = PlayerManager.instance.playerObject.transform;
		this.playerLayerMask = (1 << PlayerManager.instance.playerLayerMask);

		this.overlapResults = new Collider2D[20];
	}









	/* --------------------------------------------------------------------------------------------*/
	/* --------------------------------------------------------------------------------------------*/
	/* --------------------------------------------------------------------------------------------*/
	/* ------------------------------------ ACTIONS FUNCTIONS -------------------------------------*/
	/* --------------------------------------------------------------------------------------------*/
	/* --------------------------------------------------------------------------------------------*/
	/* --------------------------------------------------------------------------------------------*/
	public virtual IEnumerator Attack(MovementController ctr, UtilityAction act) { yield break; }








	/* --------------------------------------------------------------------------------------------*/
	/* --------------------------------------------------------------------------------------------*/
	/* --------------------------------------------------------------------------------------------*/
	/* ------------------------------------ SCORERS FUNCTIONS -------------------------------------*/
	/* --------------------------------------------------------------------------------------------*/
	/* --------------------------------------------------------------------------------------------*/
	/* --------------------------------------------------------------------------------------------*/
	public bool PlayerIsInRangeOfView(MovementController ctr)
	{
		Vector2 head, direction;
		float radius, distance;
		RaycastHit2D hit;
		int viewCount, castLayerMask;

		head = ctr.transform.position + Vector3.up * (1.5f);
		viewCount = Physics2D.OverlapCircleNonAlloc(head, ctr.entity.rangeOfView, this.overlapResults, this.playerLayerMask);

		if(viewCount == 0) {
			return false;
		}

		direction = ((Vector2)(player.position + player.up * 0.5f)) - head;
		radius = 0.25f;
		distance = ctr.entity.rangeOfView;
		castLayerMask = (1 << LayerMask.NameToLayer("Ground")) | this.playerLayerMask;
		hit = Physics2D.CircleCast(head, radius, direction, distance, castLayerMask);

		return (hit.transform == player);
	}

	public float DistanceFromPlayer(MovementController ctr)
	{
		float dist, range, res;

		range = ctr.entity.rangeOfView;
		dist = Vector2.Distance(player.position, ctr.transform.position);

		// map :
		// dist = 0 -> res = 1
		// dist > range -> res = 0
		res = (-1f / range) * dist + 1f;

		return res;
	}
}
