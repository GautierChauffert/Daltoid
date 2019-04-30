using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class CameraController : Singleton<CameraController>
{
	private Transform myTransform;
	private Transform player;
	private Rigidbody2D playerRigidbody;
	private float _smoothOverride = -1f;
	public float smoothOverride {
		get { return this._smoothOverride; }
		set {
			this._smoothOverride = value;
			if(smoothOverrideCoroutine == null) { this.StartAndStopCoroutine(ref smoothOverrideCoroutine, SmoothOverrideCoroutine()); }
		}
	}
	private IEnumerator smoothOverrideCoroutine = null;
	private float smooth;
	private Vector3 horizontalReference = Vector3.zero;
	private Vector3 verticalReference = Vector3.zero;


	[Header("Parameters")]
	[Range(0f, 5f)] public float height = 0.25f;
	[SerializeField, Range(0f, 10f)] private float offset = 0.25f;

	[Header("Smooth")]
	[SerializeField] private AnimationCurve smoothHorizontalCurve = new AnimationCurve();
	[SerializeField] private AnimationCurve smoothVerticalCurve = new AnimationCurve();

	[Header("Player")]
	public PlayerControllerData playerData;



	protected override void Awake()
	{
		base.Awake();
		
		myTransform = transform;
	}

	void Start()
	{
		player = PlayerManager.instance.playerObject.transform;
		playerRigidbody = player.GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		float smoothVertical, smoothHorizontal;
		Vector3 horizontal, vertical;
		Vector3 targetHorizontal, targetVertical;

		targetHorizontal = this.player.position + (Vector3)this.playerData.direction * this.offset - myTransform.forward * 10f;
		targetVertical = this.player.position + this.player.up * this.height - myTransform.forward * 10f;

		smoothHorizontal = (this.smoothOverride > 0f) ? this.smoothOverride : this.smoothHorizontalCurve.Evaluate(Mathf.Abs(this.playerRigidbody.velocity.x));
		smoothVertical = (this.smoothOverride > 0f) ? this.smoothOverride : this.smoothVerticalCurve.Evaluate(Mathf.Abs(this.playerRigidbody.velocity.y));

		horizontal = Vector3.SmoothDamp(this.myTransform.position, targetHorizontal, ref this.horizontalReference, smoothHorizontal);
		vertical = Vector3.SmoothDamp(this.myTransform.position, targetVertical, ref this.verticalReference, smoothVertical);

		this.myTransform.position = (horizontal + vertical) / 2f;
	}

	private IEnumerator SmoothOverrideCoroutine()
	{
		float dist;

		do {

			// dist = Vector3.Distance(myTransform.position, this.targetPosition);
			dist = 0f;
			yield return null;
		}
		while(dist > 0.05f);

		this.smoothOverride = -1f;
		this.smoothOverrideCoroutine = null;
	}
}
