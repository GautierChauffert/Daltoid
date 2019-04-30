using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
	[HideInInspector] public GameObject playerObject;

	[HideInInspector] public Rigidbody2D playerRigidbody;

	[HideInInspector] public PlayerCharacter playerCharacter;

	[HideInInspector] public PlayerMoveController playerController;

	[HideInInspector] public int playerLayerMask;


	protected override void Awake()
	{
		base.Awake();

		playerObject = GameObject.FindWithTag("Player");
		playerLayerMask = LayerMask.NameToLayer("Player");
		playerRigidbody = playerObject.GetComponent<Rigidbody2D>();
		playerCharacter = playerObject.GetComponent<PlayerCharacter>();
		playerController = playerObject.GetComponent<PlayerMoveController>();
	}
}
