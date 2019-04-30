using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeRangerEnemyAI : EnemyAI
{
	private static MeleeRangerEnemyAI instance = null;

	public static UtilityBehaviourAI Initialize(MeleeRangerEnemy enemy)
	{
		MovementController ctr = new MeleeRangerEnemyController(enemy);
		instance.AddController(ctr);

		return instance;
	}

	protected override void Awake()
	{
		base.Awake();

		instance = this;
	}





	/* --------------------------------------------------------------------------------------------*/
	/* --------------------------------------------------------------------------------------------*/
	/* --------------------------------------------------------------------------------------------*/
	/* ------------------------------------ ACTIONS FUNCTIONS -------------------------------------*/
	/* --------------------------------------------------------------------------------------------*/
	/* --------------------------------------------------------------------------------------------*/
	/* --------------------------------------------------------------------------------------------*/
	public override IEnumerator Attack(MovementController ctr, UtilityAction act)
	{
		act.isStoppable = false;

		MeleeRangerEnemyController enemy = ctr as MeleeRangerEnemyController;

		enemy.Punch();
		yield return new WaitForSeconds(0.5f);

		act.isRunning = false;
	}

	public IEnumerator Patrol(MovementController ctr, UtilityAction act)
	{
		act.isStoppable = true;

		MeleeRangerEnemyController mrec = ctr as MeleeRangerEnemyController;
		mrec.enemy.animator.SetBool("Walk", true);
		yield return mrec.Patrol();

		act.isRunning = false;
	}

	public IEnumerator StandBy(MovementController ctr, UtilityAction act)
	{
		act.isStoppable = true;

		MeleeRangerEnemyController mrec = ctr as MeleeRangerEnemyController;
		mrec.enemy.animator.SetBool("Walk", false);
		yield return new WaitForSeconds(99999f);

		act.isRunning = false;
	}
}
