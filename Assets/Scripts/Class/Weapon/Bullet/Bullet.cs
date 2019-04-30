using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Damageable;


public abstract class Bullet : PoolableEntity
{
	private LivingEntity owner;
	protected Transform myTransform;
	protected Transform body;

	private readonly Quaternion ROTATION_AJUST = Quaternion.Euler(0, 270, 0);
	private Vector3 _forward = Vector3.zero;
	protected Vector3 forward {
		get { return this._forward; }
		set {
			this._forward = value;
			if(this._forward.magnitude > 0) {
				this.myTransform.rotation = Quaternion.LookRotation(forward) * ROTATION_AJUST;
			}
		}
	}

	[SerializeField]
	protected bool bounceOnGround = false;
	private bool firstBounce = true;

	protected int groundLayer;


	protected override void Awake()
	{
		base.Awake();
		
		this.myTransform = transform;
		this.body = myTransform.Find("Body");
		this.groundLayer = LayerMask.NameToLayer("Ground");

		gameObject.layer = LayerMask.NameToLayer("Bullet");
	}

	public virtual void Initialize(LivingEntity o, DamageData d, Vector3 f)
	{
		this.owner = o;
		this.color = d.color;
		this.forward = f;
	}

	protected virtual void OnCollisionEnter2D(Collision2D other)
	{
		IDamageableEntity damageable = other.transform.GetComponent<IDamageableEntity>();
		if(damageable != null) {
			this.Damage(damageable);
			this.Disappear();
		}
	}

	protected virtual void Bounce(Collision2D other)
	{
		if(firstBounce) {
			Physics2D.IgnoreCollision(GetComponent<Collider2D>(), this.owner.GetComponent<Collider2D>(), false);
			firstBounce = false;
		}
	}

	protected abstract void Disappear();

	protected IEnumerator DisappearCoroutine(float delay)
	{
		yield return new WaitForSeconds(delay);

		this.poolingManager.Stow(this);
	}

	protected abstract void Damage(IDamageableEntity target);

	public override void Reset()
	{
		owner = null;
		forward = Vector3.zero;
		firstBounce = true;
		this.body.gameObject.SetActive(true);

		base.Reset();
	}
}
