using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpController : PlayerController<PlayerJumpController>
{
	private float jumpTime = 0f;
	public const float JUMP_TIME_MAX = 0.2f;

	private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
	private WaitForSeconds waitForJump = new WaitForSeconds(0.03f);

	private IEnumerator jumpCoroutine = null;

	void Start()
	{
		this.pc.inputs.onSetJumping.AddListener(OnSetJump);
		this.pc.inputs.onSetGrounded.AddListener(OnSetGround);
	}

	// LANDING
	private void OnSetGround(bool value)
	{
		if(value) {
			if(this.jumpCoroutine != null) {
				this.StopCoroutine(this.jumpCoroutine);
			}
			this.pc.inputs.isJumping = false;
			this.jumpTime = 0f;
			this.jumpCoroutine = null;
		}
	}

	// INITIALIZE
	private void OnSetJump(bool value)
	{
		if(!value) {
			return;
		}

		// stop jump coroutine
		if(this.jumpCoroutine != null) {
			this.StopCoroutine(this.jumpCoroutine);
		}

		// start jump
		this.jumpTime = this.playerData.jumpTimeMax;
		this.rb.AddForce(Vector2.up * this.playerData.jumpSpeed, ForceMode2D.Impulse);

		// jump coroutine
		this.jumpCoroutine = this.JumpCoroutine();
		this.StartCoroutine(this.jumpCoroutine);
	}

	private IEnumerator JumpCoroutine()
	{
		yield return waitForJump;

		while(this.pc.inputs.isHoldingJump && this.jumpTime > 0f && !this.pc.inputs.isGrounded && !this.pc.inputs.isCeiled) {

			// add force jump
			this.rb.AddForce(this.playerData.jumpSpeed * 0.6f * this.playerData.jumpHoldCoeff * Vector2.up, ForceMode2D.Force);

			this.jumpTime -= Time.fixedDeltaTime;

			yield return waitForFixedUpdate;
		}

		this.pc.inputs.isJumping = false;
		this.jumpTime = 0f;
		this.jumpCoroutine = null;
	}
}
