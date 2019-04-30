using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExampleAI : UtilityBehaviourAI
{
	private static ExampleAI instance = null;

	public static UtilityBehaviourAI Initialize(Example example)
	{
		MovementController ctr = new ExampleController(example);
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
	/* ------------------------------------ SCORERS FUNCTIONS -------------------------------------*/
	/* --------------------------------------------------------------------------------------------*/
	/* --------------------------------------------------------------------------------------------*/
	/* --------------------------------------------------------------------------------------------*/
	public float DistanceToDanger(MovementController ctr)
	{
		return 0f;
	}

	public bool ExampleIsScared(MovementController ctr)
	{
		return false;
	}

	public bool ExampleIsHungry(MovementController ctr)
	{
		return false;
	}

	public bool ExampleIsTired(MovementController ctr)
	{
		return false;
	}

	public bool ThereIsFood(MovementController ctr)
	{
		return false;
	}

	public bool CanReachFood(MovementController ctr)
	{
		return false;
	}




	/* --------------------------------------------------------------------------------------------*/
	/* --------------------------------------------------------------------------------------------*/
	/* --------------------------------------------------------------------------------------------*/
	/* ------------------------------------ ACTION FUNCTIONS --------------------------------------*/
	/* --------------------------------------------------------------------------------------------*/
	/* --------------------------------------------------------------------------------------------*/
	/* --------------------------------------------------------------------------------------------*/
	public void EatFood(MovementController ctr)
	{
		// do stuff
	}

	public IEnumerator GoToFood(MovementController ctr, UtilityAction act)
	{
		// set 'isStoppable' to true to allow action cancelling.
		act.isStoppable = false;

		// do stuff with ctr...
		yield return null;
		act.isRunning = false;
	}

	public IEnumerator Wander(MovementController ctr, UtilityAction act)
	{
		// set 'isStoppable' to true to allow action cancelling.
		act.isStoppable = false;

		// do stuff with ctr...
		yield return null;
		act.isRunning = false;
	}

	public IEnumerator RunAway(MovementController ctr, UtilityAction act)
	{
		// set 'isStoppable' to true to allow action cancelling.
		act.isStoppable = false;

		// do stuff with ctr...
		yield return null;
		act.isRunning = false;
	}
}
