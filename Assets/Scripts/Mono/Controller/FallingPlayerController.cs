using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlayerController : MonoBehaviour
{
	private Transform myTransform;

	private Vector3 center;
	private Vector3 target;
	private Vector3 reference = Vector3.zero;

	[Header("Parameters")]
	[SerializeField, Range(0.01f, 2f)] private float smooth = 1f;
	[SerializeField, Range(0.01f, 20f)] private float amplitude = 5f;
	[SerializeField, Range(0.01f, 20f)] private float freq = 5f;

	void Awake()
	{
		this.myTransform = transform;
		this.center = this.myTransform.position;
	}

	void Update()
	{
		this.target = this.center + Vector3.up * (2f * Mathf.PingPong(Time.time * this.freq, this.amplitude) - this.amplitude);
		this.myTransform.position = Vector3.SmoothDamp(this.myTransform.position, this.target, ref this.reference, this.smooth);
	}
}
