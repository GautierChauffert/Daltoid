using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchController : PlayerController<PlayerCrouchController>
{
	private readonly Vector2 STANDARD_OFFSET = new Vector2(0f, 1.175f);
	private readonly Vector2 CROUCH_OFFSET = new Vector2(0f, 0.935f);

	private readonly Vector2 STANDARD_SIZE = new Vector2(1f, 2.35f);
	private readonly Vector2 CROUCH_SIZE = new Vector2(1f, 1.85f);

	void Start()
	{
		this.pc.inputs.onSetCrouching.AddListener(OnSetCrouch);
	}

	private void OnSetCrouch(bool value)
	{
		if(value) {
			cl.offset = CROUCH_OFFSET;
			cl.size = CROUCH_SIZE;
		} else {
			cl.offset = STANDARD_OFFSET;
			cl.size = STANDARD_SIZE;
		}
	}
}
