using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Damageable;
using Tools;

public abstract class Weapon : MonoBehaviour
{
	protected Transform myTransform;
	protected Transform parent;
	protected Transform tip;
	private Transform weaponFolder;

	[HideInInspector] public Vector2 handle;

	protected LivingEntity owner;

	private const float MAX_ACCURACY = 100f;
	private const float MAX_LOW_ACCURACY_ANGLE = 45f;
	private const float FALL_SPEED = 5f;
	private const float JIGGLE_MAGNITUDE = 0.25f;

	private int groundLayerMask;

	private IEnumerator fallCoroutine = null;
	
	private Sprite _icon = null;
	public Sprite icon {
		get {
			if(this._icon == null) { this._icon = GetComponentInChildren<SpriteRenderer>().sprite; }
			return this._icon;
		}
	}

	protected DamageData damageData;


	[Header("Bullet")]
	[SerializeField] protected GameObject bulletPrefab;

	[Header("Parameters")]
	public Color color = Color.white;
	[SerializeField] protected float damage = 10f;
	[SerializeField, Range(0f, MAX_ACCURACY)] private float accuracy = 90f;


	protected virtual void Awake()
	{
		this.myTransform = transform;
		
		this.damageData = new DamageData(this.color, this.damage);
		this.groundLayerMask = (1 << LayerMask.NameToLayer("Ground"));
		this.tip = this.myTransform.DeepFind("Tip");
	}

	protected virtual void Start()
	{
		weaponFolder = WeaponManager.instance.folder;

		if(this.owner == null) {
			this.myTransform.position = this.GetGroundPosition() + Vector2.up;
		}
	}

	public virtual void Disowned()
	{
		this.owner = null;
	}

	public virtual void Owned(LivingEntity o)
	{
		this.owner = o;
		this.parent = this.owner?.transform.DeepFind("Weapon") ?? transform.parent;
		this.TryStopCoroutine(ref this.fallCoroutine);
	}

	public void DropOnGround()
	{
		this.Disowned();

		gameObject.SetActive(true);
		this.myTransform.parent = this.weaponFolder;
		this.myTransform.rotation = Quaternion.identity;

		this.StartAndStopCoroutine(ref this.fallCoroutine, this.FallCoroutine());
	}

	protected void SpawnBullet()
	{
		Bullet bullet = PoolingManager.instance.Get<Bullet>(bulletPrefab);

		if(bullet == null) {
			return;
		}

		damageData.color = this.color;
		damageData.damage = this.damage;
		Vector2 forward = this.GetBulletDirectionWithAccuracy();
		Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), this.owner.GetComponent<Collider2D>(), true);
		bullet.transform.position = tip.position;

		bullet.Initialize(this.owner, this.damageData, forward);

		bullet.Launch();
	}

	public abstract void Shoot();


	




	/* -------------------------------------------------------------------------------------------*/
	/* -------------------------------------------------------------------------------------------*/
	/* -------------------------------------------------------------------------------------------*/
	/* --------------------------------- PRIVATE UTIL FUNCTIONS ----------------------------------*/
	/* -------------------------------------------------------------------------------------------*/
	/* -------------------------------------------------------------------------------------------*/
	/* -------------------------------------------------------------------------------------------*/
	private Vector3 GetBulletDirectionWithAccuracy()
	{
		Quaternion rotation;
		Vector3 forward;
		float random, angle;

		angle = ((MAX_ACCURACY - accuracy) / MAX_ACCURACY) * MAX_LOW_ACCURACY_ANGLE;
		random = Random.Range(-angle, angle);

		rotation = Quaternion.Euler(0, 0, random);
		forward = rotation * this.myTransform.right;
		

		return forward;
	}

	private Vector2 GetGroundPosition()
	{
		RaycastHit2D hit = Physics2D.Raycast(myTransform.position, -Vector2.up, Mathf.Infinity, this.groundLayerMask);
		return hit.point;
	}

	private IEnumerator FallCoroutine()
	{
		Vector2 ground, from, to;
		Vector2 up;
		float step;

		up = Vector2.up;
		ground = this.GetGroundPosition();
		from = this.myTransform.position;
		to = (ground.magnitude > 0f) ? ground + Vector2.up : new Vector2(this.myTransform.position.x, -10000f);
		step = 0f;

		// FALL
		while(step < 1f) {
			step += FALL_SPEED * Time.deltaTime;
			this.myTransform.position = Vector2.Lerp(from, to, step);
			yield return null;
		}

		// JIGGLE
		from = this.myTransform.position;
		step = 0f;
		while(step < 1f) {
			step += FALL_SPEED * Time.deltaTime;
			this.myTransform.position = Vector2.Lerp(from, to - up * JIGGLE_MAGNITUDE, step);
			yield return null;
		}

		// STABILIZE
		from = this.myTransform.position;
		step = 0f;
		while(step < 1f) {
			step += FALL_SPEED * Time.deltaTime;
			this.myTransform.position = Vector2.Lerp(from, to, step);
			yield return null;
		}

		this.myTransform.position = to;
		this.fallCoroutine = null;
	}
}
