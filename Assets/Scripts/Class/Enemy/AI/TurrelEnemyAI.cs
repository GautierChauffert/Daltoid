using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrelEnemyAI : EnemyAI
{
	private static TurrelEnemyAI instance = null;

	public static UtilityBehaviourAI Initialize(TurrelEnemy enemy)
	{
		MovementController ctr = new TurrelEnemyController(enemy);
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

		TurrelEnemyController enemy = ctr as TurrelEnemyController;
		Vector3 target = player.position + player.up * 0.5f;
		yield return StartCoroutine(enemy.ShootAt(target));

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
