using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class PlayerDashController : PlayerController<PlayerDashController>
{
	private Transform body;
	private IEnumerator dashCoroutine = null;
	private float previousGravityScale = -1f;
	private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
	private Transform[] spriteTransforms;
	private bool airDashDone = false;


	protected override void Awake()
	{
		base.Awake();

		this.body = tr.Find("Body");
		SpriteRenderer[] rends = this.GetComponentsInChildren<SpriteRenderer>();
		this.spriteTransforms = new Transform[rends.Length];
		for(int i = 0; i < rends.Length; i++) {
			this.spriteTransforms[i] = rends[i].transform;
		}
	}

	void Start()
	{
		this.pc.inputs.onSetDashing.AddListener(OnSetDash);
		this.pc.inputs.onSetGrounded.AddListener(OnSetGround);
	}

	public void OnSetDash(bool value)
	{
		if(!value) {
			this.pc.anim.SetBool("Dash", false);
		}

		if(this.pc.inputs.cancelInput || this.dashCoroutine != null) {
			return;
		}

		if(value) {
			if(!this.pc.inputs.isGrounded) {
				if(this.airDashDone) {
					this.Reset();
					return;
				} else {
					this.airDashDone = true;
				}
			}

			this.pc.anim.SetBool("Dash", true);
            AudioManager.instance.PlaySound("Dash");
            this.StartAndStopCoroutine(ref this.dashCoroutine, this.DashCoroutine());
		}
	}

	public void OnSetGround(bool value)
	{
		if(value) {
			this.airDashDone = false;
		}
	}

	private IEnumerator DashCoroutine()
	{
		int dir;
		float clock;

		clock = this.playerData.dashDuration;
		dir = (this.playerData.direction.magnitude > 0f) ? pc.inputs.horizontalInputRaw : Mathf.RoundToInt(Vector3.Dot(Vector3.right, body.right));
		this.pc.inputs.cancelInput = true;
		this.playerData.direction = this.playerData.horizontal * dir;
		
		this.previousGravityScale = this.rb.gravityScale;
		this.rb.gravityScale = 0f;
		this.rb.velocity = Vector2.zero;

		while(clock > 0f) {
			clock -= Time.fixedDeltaTime;
			this.rb.AddForce(this.playerData.direction * this.playerData.dashSpeed, ForceMode2D.Force);
			yield return waitForFixedUpdate;
		}

		this.rb.velocity = Vector2.zero;
		this.rb.gravityScale = this.previousGravityScale;

		yield return new WaitForSeconds(this.playerData.dashAfterDelay);

		this.Reset();
	}

	public void Cancel()
	{
		this.TryStopCoroutine(ref this.dashCoroutine);
		this.pc.inputs.cancelInput = false;
		this.pc.inputs.isDashing = false;
		if(this.previousGravityScale > 0f) { this.rb.gravityScale = this.previousGravityScale; }
	}

	private void Reset()
	{
		this.pc.inputs.cancelInput = false;
		this.pc.inputs.isDashing = false;
		this.dashCoroutine = null;
	}
}
