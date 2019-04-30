using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Damageable;
using Tools;

public class BossHandController : MonoBehaviour, IDamageableEntity
{
	private Boss boss;
	private PlayerCharacter player;
	private Transform handler;
	private IEnumerator shakeCoroutine = null;

	[HideInInspector] public bool isRightHand;
	[HideInInspector] public Vector3 baseLocalPosition;
	[HideInInspector] public Animator anim;
	[HideInInspector] public bool isPunching = false;
	[HideInInspector] public Color color = Color.white;
	[HideInInspector] public Collider2D myCollider;

	private Weapon[] weapons;

	void Awake()
	{
		this.boss = GetComponentInParent<Boss>();

		this.isRightHand = this.gameObject.name.Contains("Right");
		this.baseLocalPosition = transform.localPosition;
		this.anim = GetComponentInChildren<Animator>();
		this.myCollider = GetComponent<Collider2D>();

		this.handler = transform.DeepFind("Handler");

		this.weapons = GetComponentsInChildren<Weapon>();
		foreach(Weapon w in this.weapons) {
			w.Owned(this.boss);
		}
	}

	void Start()
	{
		this.player = PlayerManager.instance.playerCharacter;
	}

	void OnCollisionEnter2D(Collision2D other)
	{		
		if(!other.gameObject.CompareTag("Player") || !this.isPunching) {
			return;
		}

		AudioManager.instance.PlaySound("BossPunch");
		this.player.GetDamagedBy(this.color, this.boss.punchDamages);
	}

	public Weapon GetOneWeapon() => this.weapons[Random.Range(0, this.weapons.Length)];

	public void GetDamagedBy(Color c, float damage)
	{
		float value;

		value = ColorableEntity.ColorCompare(this.color, c) * damage;

		this.boss.life -= value;

		if(this.boss.life <= 0f) {
			this.myCollider.enabled = false;
		}

		HUDManager.instance.UpdateBossHealthBar(this.boss, this.boss.life);

		this.StartAndStopCoroutine(ref this.shakeCoroutine, this.DamageCoroutine());
	}

	private IEnumerator DamageCoroutine()
	{
		float step, current;
		Vector3 target, from;

		current = this.boss.handShakeMagnitude;

		while(current > 0.05f)
		{
			from = this.handler.localPosition;
			target = Random.insideUnitSphere * current;
			target.Set(target.x, target.y, 0f);
			step = 0f;

			while(step < 1f) {
				step += this.boss.handShakeSpeed * Time.deltaTime;
				this.handler.localPosition = Vector3.Lerp(from, target, step);
				yield return null;
			}

			// fade away
			current *= this.boss.handShakeFadeAwayRatio;
		}

		// set camera back to 0
		from = this.handler.localPosition;
		target = Vector3.zero;
		step = 0f;

		while(step < 1f) {
			step += this.boss.handShakeSpeed * Time.deltaTime;
			this.handler.localPosition = Vector3.Lerp(from, target, step);
			yield return null;
		}

		this.shakeCoroutine = null;
		this.handler.localPosition = target;
	}
}
