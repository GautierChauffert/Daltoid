using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

[RequireComponent(typeof(PlayerWeaponInventory))]
public class PlayerWeaponController : PlayerController<PlayerWeaponController>
{
	private PlayerWeaponInventory playerWeaponInventory;
	private Transform shoulder;
	private Transform body;
	private Vector3 target;
	private Camera cam;
	private Transform[] spriteTransforms;

	private bool previousFlip;
	public bool toRight { get; private set; }

	protected override void Awake()
	{
		base.Awake();

		this.playerWeaponInventory = GetComponent<PlayerWeaponInventory>();
		this.cam = Camera.main;
		this.body = tr.Find("Body");
	}

	void Start()
	{
		this.shoulder = playerWeaponInventory.shoulder;

		this.pc.inputs.onSetFiring.AddListener(OnSetFire);
		this.pc.onDying.AddListener(OnDying);

		this.previousFlip = !(Input.mousePosition.x > cam.WorldToScreenPoint(tr.position).x);
	}

	void Update()
	{
		if(this.pc.inputs.cancelInput) {
			return;
		}

		this.OrientateWeapon();

		if(!this.pc.inputs.isDashing) {
			this.OrientatePlayer();
		}
	}

	// orientate weapong according to mouse position
	private void OrientateWeapon()
	{
		if(shoulder == null) {
			return;
		}

		// calculate target
		target = cam.ScreenToWorldPoint(Input.mousePosition);
		target.Set(target.x, target.y, 0f);

		// look at mouse
		shoulder.LookAt(target);
	}

	private void OrientatePlayer()
	{
		float playerX = cam.WorldToScreenPoint(tr.position).x;

		this.toRight = (Input.mousePosition.x > playerX);
		if(this.previousFlip == this.toRight) {
			return;
		}
		this.previousFlip = this.toRight;

		if(this.toRight)
		{
			body.eulerAngles = Vector3.zero;
			this.SwitchAllSpriteScaleForLight(false);
			body.localScale = new Vector3(body.localScale.x, body.localScale.y, 1f);
		}
		else
		{
			body.eulerAngles = new Vector3(0, 180, 0);
			this.SwitchAllSpriteScaleForLight(true);
		}
	}

	private void SwitchAllSpriteScaleForLight(bool flipped)
	{
		SpriteRenderer[] rends = this.GetComponentsInChildren<SpriteRenderer>();
		Transform tr;
		int value = (flipped) ? -1 : 1;
		foreach(SpriteRenderer r in rends) {
			tr = r.transform;
			tr.localScale = new Vector3(tr.localScale.x, tr.localScale.y, value);
		}
	}

	private void OnSetFire(bool value)
	{
		if(value) {
			this.playerWeaponInventory.current.Shoot();
		}

		this.pc.isFiring = value;
	}

	private void OnDying()
	{
		this.playerWeaponInventory.HideWeapon();
	}
}
