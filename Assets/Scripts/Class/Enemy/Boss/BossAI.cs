using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class BossAI : EnemyAI
{
	private static BossAI instance = null;

	public IEnumerator followCoroutine = null;
	public bool isDead = false;

	public Boss boss;


	public static UtilityBehaviourAI Initialize(Boss enemy)
	{
		BossController btr = new BossController(enemy);
		instance.AddController(btr);

		instance.StartAndStopCoroutine(ref instance.followCoroutine, btr.Follow());
		instance.boss = enemy;

		return instance;
	}

	protected override void Awake()
	{
		base.Awake();

		instance = this;
	}



	public bool IsInPunchMode(MovementController ctr) => (ctr.entity as Boss).mode == Boss.BossMode.Punch;
	public bool IsInColourMode(MovementController ctr) => (ctr.entity as Boss).mode == Boss.BossMode.Colour;
	public bool IsInFireMode(MovementController ctr) => (ctr.entity as Boss).mode == Boss.BossMode.Fire;
	public bool IsInFollowMode(MovementController ctr) => (ctr.entity as Boss).mode == Boss.BossMode.Follow;
	public bool IsInDeathMode(MovementController ctr) => (ctr.entity as Boss).mode == Boss.BossMode.Death;





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

		BossController btr = ctr as BossController;
		BossHandController hand = btr.SelectOneHand();

		hand.anim.SetBool("Close", true);
		Coroutine co = StartCoroutine(this.HandPunch(btr, hand));

		if(Random.Range(0, 100) < this.boss.doubleAttackProbability) {
			BossHandController hand2 = btr.SelectOtherHand(hand);

			hand2.anim.SetBool("Close", true);
			yield return btr.Aim(hand2);
			yield return btr.Punch(hand2);
			yield return btr.Reset(hand2);
		}

		yield return co;

		act.isRunning = false;
	}

	private IEnumerator HandPunch(BossController btr, BossHandController hand)
	{
		yield return btr.Aim(hand);
		yield return btr.Punch(hand);
		yield return btr.Reset(hand);
	}

	public IEnumerator Colour(MovementController ctr, UtilityAction act)
	{
		act.isStoppable = false;

		BossController btr = ctr as BossController;
		yield return btr.Colour();

		act.isRunning = false;
	}

	public IEnumerator Fire(MovementController ctr, UtilityAction act)
	{
		act.isStoppable = false;

		BossController btr = ctr as BossController;
		BossHandController hand = btr.SelectOneHand();

		hand.anim.SetBool("Aim", true);
		Coroutine co = StartCoroutine(this.HandFire(btr, hand));

		if(Random.Range(0, 100) < this.boss.doubleAttackProbability) {
			BossHandController hand2 = btr.SelectOtherHand(hand);

			hand2.anim.SetBool("Aim", true);
			yield return btr.Aim(hand2);
			yield return btr.Fire(hand2);
			yield return btr.Reset(hand2);
		}

		yield return co;

		act.isRunning = false;
	}

	private IEnumerator HandFire(BossController btr, BossHandController hand)
	{
		yield return btr.Aim(hand);
		yield return btr.Fire(hand);
		yield return btr.Reset(hand);
	}

	public void Follow(MovementController ctr)
	{
		BossController btr = ctr as BossController;
		btr.TriggerFollowAnimation();
	}

	public IEnumerator Death(MovementController ctr, UtilityAction act)
	{
		act.isStoppable = false;

		if(this.isDead) {
			yield break;
		}

		BossController btr = ctr as BossController;
		BossHandController hand1 = btr.SelectOneHand();
		BossHandController hand2 = btr.SelectOtherHand(hand1);

		this.TryStopCoroutine(ref this.followCoroutine);

		hand1.anim.SetTrigger("Death");
		hand2.anim.SetTrigger("Death");
		Coroutine c1 = StartCoroutine(btr.Reset(hand1));
		yield return btr.Reset(hand2);
		yield return c1;

		this.isDead = true;
		act.isRunning = false;
	}
}
