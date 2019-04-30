using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Damageable;

public class BossController : MovementController
{
	private Boss boss;
	private Transform tr;
	private Transform player;
	private BossHandController[] hands;
	private Material leftHandMaterial;
	private Material rightHandMaterial;

	public BossController(Boss e) : base(e)
	{
		this.boss = e;

		this.tr = e.transform;
		this.player = PlayerManager.instance.playerObject.transform;

		this.hands = e.GetComponentsInChildren<BossHandController>();
		foreach(BossHandController ctr in this.hands) {
			if(ctr.gameObject.name == "Left Hand") {
				this.leftHandMaterial = ctr.GetComponentInChildren<SpriteRenderer>().material;
			} else {
				this.rightHandMaterial = ctr.GetComponentInChildren<SpriteRenderer>().material;
			}
		}
	}

	public BossHandController SelectOneHand() => this.hands[Random.Range(0, 2)];
	public BossHandController SelectOtherHand(BossHandController h) => (h == this.hands[0]) ? this.hands[1] : this.hands[0];

	public IEnumerator Punch(BossHandController hand)
	{
		Vector3 dir, from, to;
		Transform tr;
		float step;

		hand.isPunching = true;

		tr = hand.transform;
		step = 0f;
		dir = this.player.position + Vector3.up * 0.5f - tr.position;
		from = tr.position;
		to = from + 2f * dir;

		while(step < 1f) {
			step += Time.deltaTime * this.boss.punchSpeedCurve.Evaluate(step);
			tr.position = Vector3.Slerp(from, to, step);
			yield return null;
		}

		hand.isPunching = false;
		hand.anim.SetBool("Close", false);
	}

	public void TriggerFollowAnimation()
	{
		hands[0].anim.SetTrigger("LoopFront");
		hands[1].anim.SetTrigger("LoopBack");
	}

	public IEnumerator Follow()
	{
		Vector3 target;
		Vector3 reference;

		reference = Vector3.zero;
		this.boss.interval = 4f;

		while(true) {
			target = this.player.position + (Vector3)this.boss.followOffset;
			this.tr.position = Vector3.SmoothDamp(this.tr.position, target, ref reference, this.boss.followSmooth);
			yield return null;
		}
	}

	public IEnumerator Colour()
	{
		float step;
		Color leftColor, rightColor, leftFrom, rightFrom;

		leftFrom = this.leftHandMaterial.color;
		rightFrom = this.rightHandMaterial.color;
		leftColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.75f, 1f);
		rightColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.75f, 1f);
		step = 0f;

		while(step < 1f) {
			step += Time.deltaTime;
			this.leftHandMaterial.color = Color.Lerp(leftFrom, leftColor, step);
			this.hands[0].color = this.leftHandMaterial.color;
			this.rightHandMaterial.color = Color.Lerp(rightFrom, rightColor, step);
			this.hands[1].color = this.rightHandMaterial.color;
			yield return null;
		}

		this.leftHandMaterial.color = leftColor;
		this.hands[0].color = leftColor;
		this.rightHandMaterial.color = rightColor;
		this.hands[1].color = rightColor;

		this.boss.SwitchMode();
	}

	public IEnumerator Aim(BossHandController hand)
	{
		Vector3 targetPosition, reference, align;
		Quaternion ajust;
		Transform tr;
		float clock, horizontal, vertical;
		float delay;
		int dir;

		clock = 0f;
		reference = Vector3.zero;
		tr = hand.transform;
		dir = (hand.isRightHand) ? -1 : 1;
		horizontal = dir * Random.Range(this.boss.aimHorizontalRangeOffset[0], this.boss.aimHorizontalRangeOffset[1]);
		vertical =  Random.Range(this.boss.aimVerticalRangeOffset[0], this.boss.aimVerticalRangeOffset[1]);
		ajust = Quaternion.Euler(0,90,90);

		tr.localScale = new Vector3(tr.localScale.x, tr.localScale.y, -tr.localScale.z);

		delay = Random.Range(this.boss.aimDuration + 0.75f, this.boss.aimDuration - 0.75f);

		while(clock < delay) {
			clock += Time.deltaTime;

			// follow
			targetPosition = this.player.position + Vector3.up * vertical + Vector3.right * horizontal;
			tr.position = Vector3.SmoothDamp(tr.position, targetPosition, ref reference, this.boss.aimSmooth);

			// aim
			align = this.player.position + Vector3.up * 0.5f - tr.position;
			tr.rotation = Quaternion.LookRotation(align, -dir * Vector3.up) * ajust;

			yield return null;
		}
	}

	public IEnumerator Fire(BossHandController hand)
	{
		float clock;

		// load
		clock = 0f;
		// Debug.Log("To do !");
		while(clock < this.boss.loadDuration) {
			clock += Time.deltaTime;
		}

		// fire
		clock = 0f;
		Weapon weapon = hand.GetOneWeapon();

		if(weapon is ShotGun || weapon is ColtGun) {
			weapon.Shoot();
		} else if(weapon is MachineGun) {
			while(clock < this.boss.fireDuration) {
				clock += Time.deltaTime;
				weapon.Shoot();
				yield return null;
			}
		} else {
			WaitForSeconds delay = new WaitForSeconds(0.1f);
			for(int i = 0; i < 5; i++) {
				weapon.Shoot();
				yield return delay;
			}
		}

		hand.anim.SetBool("Aim", false);

		this.boss.SwitchMode();
	}

	public IEnumerator Reset(BossHandController hand)
	{
		Transform tr;
		Vector3 fromPosition, toPosition;
		Quaternion fromRotation, toRotation;
		float step;

		tr = hand.transform;
		fromRotation = tr.rotation;
		fromPosition = tr.localPosition;
		toRotation = Quaternion.identity;
		toPosition = hand.baseLocalPosition;
		tr.localScale = new Vector3(tr.localScale.x, tr.localScale.y, (hand.isRightHand) ? -1 : 1);
		step = 0f;

		while(step < 1f) {
			step += Time.deltaTime * this.boss.resetSpeed;

			tr.localPosition = Vector3.Slerp(fromPosition, toPosition, step);
			tr.rotation = Quaternion.Slerp(fromRotation, toRotation, step);

			yield return null;
		}

		tr.localPosition = toPosition;
		tr.rotation = toRotation;
	}
}
