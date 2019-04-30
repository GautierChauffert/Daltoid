using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class CameraShake2D : Singleton<CameraShake2D>
{
	private Transform cam;
	private float currentMagnitude = 0f;
	private IEnumerator shakeCoroutine = null;

	[Header("Parameters")]
	[SerializeField, Range(0.1f, 0.9f)] private float fadeAwayRatio = 0.575f;
	[SerializeField, Range(0f, 50f)] private float shakeSpeed = 25f;

	protected override void Awake()
	{
		base.Awake();

		cam = Camera.main.transform;
	}

	public void Shake(float magnitude)
	{
		if(magnitude >= this.currentMagnitude) {
			this.StartAndStopCoroutine(ref this.shakeCoroutine, this.ShakeCoroutine(magnitude));
		}
	}

	private IEnumerator ShakeCoroutine(float magnitude)
	{
		float step;
		Vector3 target, from;

		this.currentMagnitude = magnitude;

		while(this.currentMagnitude > 0.05f)
		{
			from = this.cam.localPosition;
			target = Random.insideUnitSphere * this.currentMagnitude;
			target.Set(target.x, target.y, 0f);
			step = 0f;

			while(step < 1f) {
				step += this.shakeSpeed * Time.deltaTime;
				this.cam.localPosition = Vector3.Lerp(from, target, step);
				yield return null;
			}

			// fade away
			this.currentMagnitude *= fadeAwayRatio;
		}

		// set camera back to 0
		from = this.cam.localPosition;
		target = Vector3.zero;
		step = 0f;

		while(step < 1f) {
			step += this.shakeSpeed * Time.deltaTime;
			this.cam.localPosition = Vector3.Lerp(from, target, step);
			yield return null;
		}

		this.shakeCoroutine = null;
		this.currentMagnitude = 0f;
		this.cam.localPosition = target;
	}
}
