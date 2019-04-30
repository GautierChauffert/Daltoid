using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiteEnemyAI : EnemyAI
{
	private static MiteEnemyAI instance = null;

	public static UtilityBehaviourAI Initialize(MiteEnemy enemy)
	{
		MovementController ctr = new MiteEnemyController(enemy);
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

		MiteEnemyController mtr = ctr as MiteEnemyController;
		mtr.Punch();
		yield return new WaitForSeconds(0.5f);

		act.isRunning = false;
	}

	public IEnumerator Patrol(MovementController ctr, UtilityAction act)
	{
		act.isStoppable = true;

		MiteEnemyController mrec = ctr as MiteEnemyController;
		yield return mrec.Patrol();

		act.isRunning = false;
	}
}
