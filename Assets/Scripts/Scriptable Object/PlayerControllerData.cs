using UnityEngine;
using System;



[CreateAssetMenu(fileName = "PlayerControllerData", menuName = "Scriptable Object/Data/PlayerControllerData", order = 3)]
public class PlayerControllerData : ScriptableObject
{
	[NonSerialized] public Vector2 horizontal;
	[NonSerialized] public Vector2 direction;
	[NonSerialized] public Vector2 groundNormal;

	[Header("Global")]
	[Range(0f, 5f)] public float slopeThreshold = 0.75f;
	[Range(0f, 2f)] public float ceilOffset = 1.5f;
	[Range(0f, 2f)] public float groundOffset = 1.5f;

	[Header("Move")]
	[Range(0f, 200f)] public float moveSpeed = 10f;

	[Header("Jump")]
	[Range(0f, 1000f)] public float jumpSpeed = 600f;
	[Range(0f, 50f)] public float jumpHoldCoeff = 17.5f;
	[Range(0f, 2f)] public float jumpTimeMax = 0.2f;

	[Header("Dash")]
	[Range(0f, 30000f)] public float dashSpeed = 10f;
	[Range(0.01f, 2f)] public float dashDuration = 10f;
	[Range(0f, 2f)] public float dashAfterDelay = 0.175f;

	[Header("Ground")]
	public LayerMask grounds;
}