using UnityEngine;

public class PoolableEntity : ColorableEntity
{
	protected GameObject myGameObject;

	[HideInInspector]
	public bool isActive = false;

	protected PoolingManager poolingManager { get; private set; }


	protected override void Awake()
	{
		base.Awake();

		this.myGameObject = gameObject;
	}

	protected override void Start()
	{
		base.Start();
		
		this.poolingManager = PoolingManager.instance;
	}

	public virtual void Launch()
	{
		this.myGameObject.SetActive(true);

		this.StopAllCoroutines();

		this.isActive = true;
	}

	public virtual void Reset()
	{
		this.isActive = false;

		this.StopAllCoroutines();

		this.myGameObject.SetActive(false);
	}
}
