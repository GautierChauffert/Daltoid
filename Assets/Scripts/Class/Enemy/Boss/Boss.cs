using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
	public enum BossMode {
		Punch = 0,
		Colour,
		Fire,
		Follow,
		Death
	}

	[Header("Mode")]
	public BossMode mode = BossMode.Follow;
	public float interval;

	[Header("Follow")]
	public Vector2 followOffset;
	[Range(0.01f, 3f)] public float followSmooth = 1f;

	[Header("Aim")]
	public Vector2 aimHorizontalRangeOffset;
	public Vector2 aimVerticalRangeOffset;
	public float aimSmooth;
	public float aimDuration;

	[Header("Fire")]
	public float loadDuration;
	public float fireDuration;

	[Header("Punch")]
	public AnimationCurve punchSpeedCurve = new AnimationCurve();
	[Range(0f, 100f)] public float punchDamages = 10f;

	[Header("Reset")]
	public float resetSpeed;

	[Header("Hand")]
	[Range(0f, 5f)] public float handShakeMagnitude;
	[Range(0f, 50f)] public float handShakeSpeed;
	[Range(0.51f, 1f)] public float handShakeFadeAwayRatio = 0.575f;
	[Range(0, 100)] public int doubleAttackProbability = 50;

	[Header("Death")]
	[SerializeField] private float deathSmoothDeparture = 10f;

	[HideInInspector] public float initialLife;

	private float clock = 0f;

	private BossAI bossBehaviour = null;

	protected override void Awake()
	{
		base.Awake();
		
		this.initialLife = this.life;
	}

	protected override void Start()
	{
		base.Start();

		// Initialize AI behaviour (this will launch the AI)
		this.behaviour = BossAI.Initialize(this);
		this.bossBehaviour = this.behaviour as BossAI;
	}

	void Update()
	{
		if(this.isDying) {
			return;
		}

		if(Time.time > clock + this.interval) {
			this.SwitchMode();
			clock = Time.time;
		}
	}

	public void SwitchMode()
	{
		int old = (int)this.mode;

		this.mode = (BossMode)((old + Random.Range(1, 4)) % 4);
	}

	protected override void Death()
	{		
		if(this.isDying) {
			return;
		}

		this.mode = BossMode.Death;

		HUDManager.instance.DisplayBossHealthBar(false);

		this.behaviour.Cancel(this);
        AudioManager.instance.PlaySound("BossDeath");
        StartCoroutine(this.DeathCoroutine());
	}

	private IEnumerator DeathCoroutine()
	{
		yield return new WaitUntil(() => this.bossBehaviour.isDead);

		yield return new WaitForSeconds(2f);

		this.behaviour.Remove(this);

		StartCoroutine(this.ReachTheSkyCoroutine());
	}

	private IEnumerator ReachTheSkyCoroutine()
	{
		// reach the sky
		Vector3 target = new Vector3(this.myTransform.position.x, 10000f, 0f);
		Vector3 reference = Vector3.zero;
		while(Vector3.Distance(this.myTransform.position, target) > 1f) {
			this.myTransform.position = Vector3.SmoothDamp(this.myTransform.position, target, ref reference, this.deathSmoothDeparture);
			yield return null;
		}
	}
}
