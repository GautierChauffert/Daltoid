using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class PlayerInput : Singleton<PlayerInput>
{
	private InputManager inputManager;

	public float horizontalInput { get; private set; } = 0f;
	public float verticalInput { get; private set; } = 0f;
	public int horizontalInputRaw { get; private set; } = 0;
	public int verticalInputRaw { get; private set; } = 0;

	private IEnumerator jumpInputCoroutine = null;

	[HideInInspector] public bool isCeiled = false;
	[HideInInspector] public bool cancelInput = false;
	[SerializeField, Range(0f, 1f)] private float jumpInputTimeRange = 0.1f;


	private bool _isGrounded = false;
	public bool isGrounded {
		get { return _isGrounded; }
		set {
			onSetGrounded.Invoke(value);
			_isGrounded = value;
		}
	}

	private bool _isWalking = false;
	public bool isWalking {
		get { return _isWalking; }
		private set {
			onSetWalking.Invoke(value);
			_isWalking = value;
		}
	}

	private bool _isMoving = false;
	public bool isMoving {
		get { return _isMoving; }
		private set {
			onSetMoving.Invoke(value);
			_isMoving = value;
		}
	}

	private bool _isJumping = false;
	public bool isJumping {
		get { return _isJumping; }
		set {
			onSetJumping.Invoke(value);
			_isJumping = value;
		}
	}

	private bool _isHoldingJump = false;
	public bool isHoldingJump {
		get { return _isHoldingJump; }
		private set {
			_isHoldingJump = value;
			onStartHoldingJump.Invoke(value);
		}
	}

	private bool _isFalling = false;
	public bool isFalling {
		get { return _isFalling; }
		private set {
			onSetFalling.Invoke(value);
			_isFalling = value;
		}
	}

	private bool _isDashing = false;
	public bool isDashing {
		get { return _isDashing; }
		set {
			_isDashing = value;
			onSetDashing.Invoke(value);
		}
	}

	private bool _isFiring = false;
	public bool isFiring {
		get { return _isFiring; }
		private set {
			onSetFiring.Invoke(value);
			_isFiring = value;
		}
	}

	private bool _isCrouching = false;
	public bool isCrouching {
		get { return _isCrouching; }
		set {
			onSetCrouching.Invoke(value);
			_isCrouching = value;
		}
	}


	[HideInInspector] public OnActionEvent onSetJumping = new OnActionEvent();
	[HideInInspector] public OnActionEvent onSetGrounded = new OnActionEvent();
	[HideInInspector] public OnActionEvent onSetDashing = new OnActionEvent();
	[HideInInspector] public OnActionEvent onSetWalking = new OnActionEvent();
	[HideInInspector] public OnActionEvent onSetMoving = new OnActionEvent();
	[HideInInspector] public OnActionEvent onStartHoldingJump = new OnActionEvent();
	[HideInInspector] public OnActionEvent onSetFalling = new OnActionEvent();
	[HideInInspector] public OnActionEvent onSetFiring = new OnActionEvent();
	[HideInInspector] public OnActionEvent onSetCrouching = new OnActionEvent();





	void Start()
	{
		inputManager = InputManager.instance;
		this.onSetGrounded.AddListener(OnSetGround);
		this.onSetMoving.AddListener(OnSetMove);
	}

	void Update()
	{
		if(this.cancelInput) {
			return;
		}

		// AXIS
		horizontalInput = inputManager.GetHorizontalAxis();
		verticalInput = inputManager.GetVerticalAxis();
		horizontalInputRaw = (int)inputManager.GetHorizontalAxisRaw();
		verticalInputRaw = (int)inputManager.GetVerticalAxisRaw();


		// INPUTS
		if(inputManager.GetHorizontalAxisHold()) {
			if(!isMoving) {
				isMoving = true;
			}
			if(!isWalking && !isJumping && isGrounded) {
				isWalking = true;
			}
		} else {
			if(isMoving) {
				isMoving = false;
			}
			if(isWalking) {
				isWalking = false;
			}
		}
		if(Input.GetButtonDown("Vertical"))
		{
			if(Input.GetAxisRaw("Vertical") == 1f)
			{
				this.StartAndStopCoroutine(ref this.jumpInputCoroutine, this.JumpInputCoroutine());
				if(!isHoldingJump) {
					isHoldingJump = true;
				}
			}
		}
		if(Input.GetAxisRaw("Vertical") == -1f && isGrounded && !isCrouching && !isMoving) {
			this.isCrouching = true;
		}
		if(Input.GetButtonUp("Vertical")) {
			if(isHoldingJump) {
				isHoldingJump = false;
			}
			if(isCrouching) {
				isCrouching = false;
			}
		}
		if(inputManager.GetDashDown() && !isDashing) {
			isDashing = true;
		}
		if(inputManager.GetFire()) {
			isFiring = true;
		}
		if(inputManager.GetFireUp() && isFiring) {
			isFiring = false;
		}


		// LOGICS
		if(!isJumping && !isGrounded && !isFalling) {
			isFalling = true;
		}
	}

	private void OnSetGround(bool value)
	{
		if(value) {
			this.isFalling = false;
		}
	}

	private void OnSetMove(bool value)
	{
		if(value && this.isCrouching) {
			this.isCrouching = false;
		}
	}

	private IEnumerator JumpInputCoroutine()
	{
		float clock = 0f;

		while(clock < this.jumpInputTimeRange) {
			clock += Time.deltaTime;

			if(isGrounded && !isJumping) {
				isJumping = true;
				isWalking = false;
				break;
			}

			yield return null;
		}
	}
}
