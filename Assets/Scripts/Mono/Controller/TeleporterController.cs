using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Tools;

public class TeleporterController : MonoBehaviour
{
	[Header("Teleportation")]
	public TeleporterController destination;
	public UnityEvent onTeleport = new UnityEvent();

	[Header("Parameters")]
	[SerializeField, Range(0.01f, 2f)] private float distanceThreshold = 0.5f;
	[SerializeField, Range(0.01f, 10f)] private float scaleSpeed = 0.5f;
	[SerializeField, Range(0.001f, 0.5f)] private float cameraSmooth = 0.5f;



	private Transform myTransform;
	private Transform playerBody;

	private bool isTeleporting = false;
	private IEnumerator teleportCoroutine = null;
	private IEnumerator inputCoroutine = null;

	private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

	private InputManager inputManager;
	private PlayerInput playerInput;
	private CameraController cameraController;

	private Rigidbody2D playerRigidbody;

	private ParticleSystem soft;
	private ParticleSystem worpIn;
	private ParticleSystem worpOut;


	void Awake()
	{
		this.myTransform = transform;

		this.soft = this.GetComponentInChildrenWithName<ParticleSystem>("Soft");
		this.worpIn = this.GetComponentInChildrenWithName<ParticleSystem>("Worp In");
		this.worpOut = this.GetComponentInChildrenWithName<ParticleSystem>("Worp Out");
	}

	void Start()
	{
		this.cameraController = CameraController.instance;
		this.inputManager = InputManager.instance;
		this.playerInput = PlayerInput.instance;
		this.playerRigidbody = PlayerManager.instance.playerRigidbody;
		this.playerBody = this.playerRigidbody.transform.Find("Body");
	}


	void OnCollisionEnter2D(Collision2D other)
	{
		if(!other.transform.CompareTag("Player") || this.isTeleporting) {
			return;
		}

		this.StartAndStopCoroutine(ref this.inputCoroutine, this.InputCoroutine(other.transform));
	}
	void OnCollisionStay2D(Collision2D other)
	{
		if(!other.transform.CompareTag("Player") || this.isTeleporting) {
			return;
		}

		if(this.inputCoroutine == null) {
			this.StartAndStopCoroutine(ref this.inputCoroutine, this.InputCoroutine(other.transform));
		}
	}
	void OnCollisionExit2D(Collision2D other)
	{
		if(!other.transform.CompareTag("Player") || this.isTeleporting) {
			return;
		}

		this.soft.Stop();
		this.TryStopCoroutine(ref this.inputCoroutine);
	}





	private IEnumerator InputCoroutine(Transform other)
	{
		bool effect = false;

		while(true)
		{
			if(Mathf.Abs(other.position.x - this.myTransform.position.x) < this.distanceThreshold)
			{
				if(!effect) {
					this.soft.Play();
					effect = true;
				}

				if(Input.GetButtonDown("Teleport")) {
					this.Teleport();
					break;
				}
			}
			else if(effect)
			{
				this.soft.Stop();
				effect = false;
			}

			yield return null;
		}

		this.inputCoroutine = null;
	}




	private void Teleport()
	{
		// particle
		this.soft.Stop();
		this.worpIn.Play();

        // sound
        AudioManager.instance.PlaySound("Teleporter");

        // event
        this.onTeleport.Invoke();

		// main
		this.StartAndStopCoroutine(ref this.teleportCoroutine, TeleportCoroutine());
	}

	private IEnumerator TeleportCoroutine()
	{
		this.playerInput.cancelInput = true;
		this.playerInput.isCrouching = false;

		yield return this.TeleportTo(this.destination);
		yield return this.destination.TeleportFrom();

		this.playerInput.cancelInput = false;
		this.playerInput.isCrouching = false;
	}

	private IEnumerator TeleportTo(TeleporterController dest)
	{
		this.isTeleporting = true;

		this.playerRigidbody.velocity = Vector2.zero;
		this.playerRigidbody.interpolation = RigidbodyInterpolation2D.None;

		yield return waitForFixedUpdate;
		
		float step = 0f;
		Vector3 target = new Vector3(0, 1, 1);
		Vector3 from = this.playerBody.localScale;
		while(step < 1f) {
			step += this.scaleSpeed * Time.deltaTime;
			this.playerBody.localScale = Vector3.Lerp(from, target, step);
			yield return null;
		}
		this.playerBody.localScale = target;

		this.isTeleporting = false;
	}

	public IEnumerator TeleportFrom()
	{
		this.isTeleporting = true;

		this.playerRigidbody.position = myTransform.position;
		yield return waitForFixedUpdate;
		this.playerRigidbody.interpolation = RigidbodyInterpolation2D.Interpolate;
		this.cameraController.smoothOverride = this.cameraSmooth;

		// this.PlayParticleSystemBackwards(this.worpOut, 1f, 1.25f);
		this.worpOut.Play();

		float step = 0f;
		Vector3 target = Vector3.one;
		Vector3 from = this.playerBody.localScale;
		while(step < 1f) {
			step += this.scaleSpeed * Time.deltaTime;
			this.playerBody.localScale = Vector3.Lerp(from, target, step);
			yield return null;
		}
		this.playerBody.localScale = target;

		this.isTeleporting = false;
	}






	void OnDrawGizmosSelected()
	{
		if(destination == null) {
			return;
		}

		// DebugExtension.DrawArrowLineGizmo(transform.position + Vector3.up * 0.25f, destination.transform.position + Vector3.up * 0.25f, Color.blue, 0.5f);
	}
}
