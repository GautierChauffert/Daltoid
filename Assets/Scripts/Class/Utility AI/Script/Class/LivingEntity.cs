using UnityEngine;
using UnityEngine.Events;
using Damageable;

public class OnActionEvent : UnityEvent<bool> {}

[RequireComponent(typeof(Collider2D))]
public abstract class LivingEntity : ColorableEntity, IDamageableEntity
{
	public class OnLifeEvent : UnityEvent<float> {}

	// useful for inspector
	[HideInInspector] public UtilityBehaviourAI behaviour;

	[HideInInspector] public Transform myTransform;
	protected GameObject body;

	[HideInInspector] public OnActionEvent onSetDying = new OnActionEvent();

	[HideInInspector] public OnLifeEvent onLifeUpdate = new OnLifeEvent();

	public UnityEvent onDeath = new UnityEvent();

	[SerializeField] private float _life = 100f;
	public float life {
		get { return this._life; }
		set {
			onLifeUpdate.Invoke(value);
			this._life = value;
			if(this._life <= 0f) {
				this.Death();
				this.onDeath.Invoke();
				this.isDying = true;
			}
		}
	}
	public float rangeOfView = 5f;

	private bool _isDying = false;
	public bool isDying {
		get { return _isDying; }
		set {
			_isDying = value;
			onSetDying.Invoke(value);
		}
	}

	[HideInInspector] public bool isFiring = false;


	protected override void Awake()
	{
		base.Awake();

		myTransform = transform;
		body = myTransform.Find("Body").gameObject;
		// myCollider = GetComponent<Collider2D>();
	}

	protected virtual void Death()
	{
		// Debug.LogWarning("TO DO !");
	}

	public void GetDamagedBy(Color c, float damage)
	{
		float value;

		value = ColorableEntity.ColorCompare(this.color, c) * damage;
		this.life -= value;
	}

	public void SwitchAllSpriteScaleForLight(bool flipped)
	{
		int value = (flipped) ? -1 : 1;
		foreach(Transform t in this.body.transform) {
			t.localScale = new Vector3(t.localScale.x, t.localScale.y, value);
		}
	}
}
