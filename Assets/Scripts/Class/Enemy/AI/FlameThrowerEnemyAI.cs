using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerEnemyAI : EnemyAI
{
	private static FlameThrowerEnemyAI instance = null;

	public static UtilityBehaviourAI Initialize(FlameThrowerEnemy enemy)
	{
		MovementController ctr = new FlameThrowerEnemyController(enemy);
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

		FlameThrowerEnemyController enemy = ctr as FlameThrowerEnemyController;
		yield return StartCoroutine(enemy.Fire());

		act.isRunning = false;
	}

	public IEnumerator StandBy(MovementController ctr, UtilityAction act)
	{
		act.isStoppable = true;

		// yield return enemy.WatchOut();
		yield return new WaitForSeconds(99999f);

		act.isRunning = false;
	}
}
