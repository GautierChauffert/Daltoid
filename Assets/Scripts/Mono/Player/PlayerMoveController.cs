using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Tools;

public class PlayerMoveController : PlayerController<PlayerMoveController>
{
	void Start()
	{
		this.pc.inputs.onSetMoving.AddListener(OnSetMove);
	}

	void FixedUpdate()
	{
		if(pc.inputs.isDashing || pc.inputs.cancelInput) {
			return;
		}

		this.playerData.direction = Vector3.right * pc.inputs.horizontalInputRaw;

		Vector2 move = this.playerData.horizontal * pc.inputs.horizontalInputRaw * this.playerData.moveSpeed;
		this.rb.AddForce(move, ForceMode2D.Force);
	}

	private void OnSetMove(bool value)
	{
		if(pc.inputs.isDashing || pc.inputs.cancelInput) {
			return;
		}

		if(!value) {
			this.rb.velocity = new Vector2(0f, this.rb.velocity.y);
		}
	}
}