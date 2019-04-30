using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Damageable;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ProjectileBullet : Bullet
{
	protected Rigidbody2D rb;
	private Collider2D cl;

	protected ParticleSystem tail;
	protected ParticleSystem explosion;
	protected ParticleSystem bounce;
	protected ParticleSystem awake;
	protected TrailRenderer trail;

	private const float LIFETIME = 2.0f;
	private float startLifeTime;

	private int bounceCount = 0;
	[SerializeField, Range(1, 5)] private int maxBounceCount = 5;

	protected float damage;

	[Header("Parameters")]
	[SerializeField]
	protected float speed = 50f;

	private bool isDisappearing = false;


	protected override void Awake()
	{
		base.Awake();
		rb = GetComponent<Rigidbody2D>();
		cl = GetComponent<Collider2D>();

		ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
		foreach(ParticleSystem ps in particles) {
			if(ps.name.Equals("Tail")) {
				tail = ps;
			} else if(ps.name.Equals("Explosion")) {
				explosion = ps;
			} else if(ps.name.Equals("Bounce")) {
				bounce = ps;
			} else if(ps.name.Equals("Awake")) {
				awake = ps;
			}
		}

		trail = GetComponentInChildren<TrailRenderer>();
	}

	public override void Launch()
	{
		base.Launch();

		startLifeTime = Time.time;
		rb.velocity = forward * speed;
	}

	void Update()
	{
		if(Time.time - startLifeTime > LIFETIME && !isDisappearing) {
			Disappear();
		}
	}

	protected override void OnCollisionEnter2D(Collision2D other)
	{
		base.OnCollisionEnter2D(other);

		// collide with ground
		if(((1 << other.gameObject.layer) & groundLayer) == 0)
		{
			if(bounceOnGround && bounceCount < maxBounceCount) {
				this.Bounce(other);
			} else {
				this.Disappear();
			}
		}
	}

	public override void Initialize(LivingEntity o, DamageData d, Vector3 f)
	{
		base.Initialize(o, d, f);

		this.damage = d.damage;

		// body initialization
		SpriteRenderer[] sr = body.GetComponentsInChildren<SpriteRenderer>();
		foreach(SpriteRenderer s in sr) {
			s.color = d.color;
		}

		// particle initialization
		ParticleSystem.MainModule m;
		if(tail != null) {
			m = tail.main;
			m.startColor = d.color;
		}
		if(explosion != null) {
			m = explosion.main;
			m.startColor = d.color;
		}
		if(bounce != null) {
			m = bounce.main;
			m.startColor = d.color;
		}
		if(awake != null) {
			m = awake.main;
			m.startColor = d.color;
		}
		if(trail != null) {
			trail.startColor = d.color;
			trail.endColor = d.color;
		}
	}

	protected override void Damage(IDamageableEntity target)
	{
		// only does that
		target.GetDamagedBy(color, damage);
	}

	protected override void Bounce(Collision2D other)
	{
		base.Bounce(other);

		Vector2 normal;

		normal = other.GetContact(0).normal;
		forward = Vector2.Reflect(forward, normal);
		rb.velocity = forward * speed;

		bounceCount ++;

		if(bounce != null) {
			Vector3 target = myTransform.position + new Vector3(normal.x, normal.y, 0);
			bounce.transform.LookAt(target);
			bounce.Play();
		}
	}

	protected override void Disappear()
	{
		float delay;

		forward = Vector2.zero;
		rb.velocity = Vector2.zero;
		isDisappearing = true;

		cl.enabled = false;
		body.gameObject.SetActive(false);
		tail?.Stop();
		delay = 0.5f;

		if(trail != null) {
			trail.emitting = false;
		}
		
		if(explosion != null) {
			explosion.Play();

			ParticleSystem.MainModule m;
			m = explosion.main;
			delay = m.duration + 0.5f;
		}

		StartCoroutine(this.DisappearCoroutine(delay));
	}

	public override void Reset()
	{
		tail?.Stop();
		explosion?.Stop();
		bounce?.Stop();
		trail?.Clear();

		if(trail != null) {
			trail.emitting = true;
		}

		cl.enabled = true;
		bounceCount = 0;

		isDisappearing = false;

		base.Reset();
	}
}
