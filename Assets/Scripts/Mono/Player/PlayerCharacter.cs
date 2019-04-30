using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Damageable;
using Tools;

[RequireComponent(typeof(PlayerInput))]
public class PlayerCharacter : LivingEntity, IDamageableEntity
{
	private Transform shoulder;

	public PlayerInput inputs { get; private set; }
	private PlayerWeaponController playerWeaponController;
	private PlayerDashController playerDashController;
	private HUDManager hudManager;

	private ParticleSystem landingSmoke;
	private ParticleSystem jumpingSmoke;

	private Rigidbody2D rb;
	private Collider2D cl;

	private Animator _anim = null;
	public Animator anim {
		get { return this._anim; }
		private set { this._anim = value; }
	}

	[Header("Curve")]
	public AnimationCurve fallCameraShakeCurve = new AnimationCurve();
	public AnimationCurve landTemporizationCurve = new AnimationCurve();

	[Header("Shoulder")]
	[SerializeField] private Vector3 shoulderLandPosition = Vector3.zero;
	[SerializeField] private Vector3 shoulderCrouchPosition = Vector3.zero;
	private Vector3 shoulderStandardPosition = Vector3.zero;

	private const float MINIMUM_INTERVAL_BETWEEN_EFFECTS = 0.2f;
	public static readonly float PLAYER_MAXIMUM_LIFE = 100f;
	
	private float fallStartTime = 0f;
	private float lastTimeGrounded = 0f;

	private IEnumerator impactCoroutine = null;
	private IEnumerator temporizationCoroutine = null;

	[HideInInspector] public UnityEvent onDying = new UnityEvent();

	protected override void Awake()
	{
		base.Awake();

		this.rb = GetComponent<Rigidbody2D>();
		this.cl = GetComponent<Collider2D>();
		this.inputs = GetComponent<PlayerInput>();
		this.anim = GetComponentInChildren<Animator>();
		this.landingSmoke = myTransform.GetComponentInChildrenWithName<ParticleSystem>("Landing Smoke");
		this.jumpingSmoke = myTransform.GetComponentInChildrenWithName<ParticleSystem>("Jumping Smoke");

		this.shoulder = transform.DeepFind("Shoulder");
		this.shoulderStandardPosition = this.shoulder.localPosition;

		this.onLifeUpdate.AddListener(this.DamageEffect);

		this.inputs.onSetJumping.AddListener(OnSetJump);
		this.inputs.onSetGrounded.AddListener(OnSetGround);
		this.inputs.onSetFalling.AddListener(OnSetFall);
		this.inputs.onSetCrouching.AddListener(OnSetCrouch);
	}

	protected override void Start()
	{
		base.Start();

		this.playerWeaponController = GetComponent<PlayerWeaponController>();
		this.playerDashController = GetComponent<PlayerDashController>();
		this.hudManager = HUDManager.instance;

		this.anim.SetFloat("Move", 0.5f);
	}

	void Update()
	{
		float target;

		if(inputs.isWalking)
		{
			target = ((inputs.horizontalInputRaw == 1) == playerWeaponController.toRight) ? 0f : 1f;
		}
		else if(inputs.isDashing)
		{
			if (inputs.horizontalInputRaw != 0) {
				target = ((inputs.horizontalInputRaw > 0) == playerWeaponController.toRight) ? 0f : 1f;
			} else {
				target = 0f;
			}
		}
		else
		{
			target = 0.5f;
		}
		
		anim.SetFloat("Move", target, 0.1f, Time.deltaTime);
	}

	// Add force + Cancel dash + Cancel inputs
	public void Impact(Vector2 impact)
	{
		this.Impulse(impact);
		this.inputs.cancelInput = true;
		this.StartAndStopCoroutine(ref this.impactCoroutine, this.ImpactCoroutine());
	}

	// Add force + Cancel dash
	public void Impulse(Vector2 impulse)
	{
		this.playerDashController?.Cancel();
		this.rb.AddForce(impulse, ForceMode2D.Impulse);
	}

	private IEnumerator ImpactCoroutine()
	{
		yield return new WaitUntil(() => this.rb.velocity.magnitude < 2f);
		this.inputs.cancelInput = false;
	}




	protected override void Death()
	{
		this.onDying.Invoke();

		this.inputs.cancelInput = true;

		this.cl.enabled = false;
		this.rb.bodyType = RigidbodyType2D.Static;
		
		this.anim.SetTrigger("Death");
		this.enabled = false;

		AudioManager.instance.PlaySound("Death2");

		StartCoroutine(DeathCoroutine());
	}

	private IEnumerator DeathCoroutine()
	{
		yield return new WaitForSeconds(1f);
		hudManager.GameOver();
	}


	private void DamageEffect(float newLife)
	{
		float diff;

		diff = this.life - newLife;

		CameraShake2D.instance.Shake(1.25f);
		Glitch.instance.Launch(0.35f);
		AudioManager.instance.Damage(0.2f, 0.5f);
	}





	private void OnSetFall(bool value)
	{
		// only does that
		anim.SetBool("Fall", value);

		if(value) {
			this.fallStartTime = Time.time;
		}
	}

	private void OnSetJump(bool value)
	{
		// only does that
		anim.SetBool("Jump", value);
		
		if(value) {
			this.jumpingSmoke.Play();
		}
	}

	private void OnSetGround(bool value)
	{
		if(!value) {
			this.inputs.isCrouching = false;
			return;
		}

		if(Time.time - this.lastTimeGrounded > MINIMUM_INTERVAL_BETWEEN_EFFECTS && landingSmoke.isStopped) {
			float interval = Time.time - this.fallStartTime;
			CameraShake2D.instance.Shake(this.fallCameraShakeCurve.Evaluate(interval));
			landingSmoke.Play();

			this.StartAndStopCoroutine(ref temporizationCoroutine, this.LandingTemporizationCoroutine(this.landTemporizationCurve.Evaluate(interval)));
		}

		this.lastTimeGrounded = Time.time;

		anim.SetBool("Fall", false);
	}

	private void OnSetCrouch(bool value)
	{
		if(value) {
			this.anim.SetBool("Crouch", true);
			this.shoulder.localPosition = this.shoulderCrouchPosition;
		} else {
			this.anim.SetBool("Crouch", false);
			this.shoulder.localPosition = this.shoulderStandardPosition;
		}
	}

	private IEnumerator LandingTemporizationCoroutine(float duration)
	{
		this.inputs.cancelInput = true;
		this.anim.SetBool("Land", true);
		this.shoulder.localPosition = this.shoulderLandPosition;

		yield return new WaitForSeconds(duration);

		this.inputs.cancelInput = false;
		this.anim.SetBool("Land", false);
		this.shoulder.localPosition = this.shoulderStandardPosition;
	}
}
