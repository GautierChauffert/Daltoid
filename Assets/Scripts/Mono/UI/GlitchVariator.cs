using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Glitch))]
public class GlitchVariator : MonoBehaviour
{
	[Header("Parameters")]
	[SerializeField, Range(0f, 20f)] private float intensity = 0.1f;
	[SerializeField, Range(0.01f, 20f)] private float speed = 0.1f;
	[SerializeField] private Vector2 interval = Vector2.zero;

	private Glitch glitch;

	void Awake()
	{
		this.glitch = GetComponent<Glitch>();
	}

	void Start()
	{
		StartCoroutine(this.EffectCoroutine());
	}

	private IEnumerator EffectCoroutine()
	{
		float initial, from, to;
		float step;

		initial = this.glitch.scanLineJitter;

		while(true) {

			from = this.glitch.scanLineJitter;
			to = initial + Random.Range(0.1f, 1f) * this.intensity;
			step = 0f;
			while(step < 1f) {
				step += Time.deltaTime * this.speed;
				this.glitch.scanLineJitter = Mathf.Lerp(from, to, step);
				yield return null;
			}

			from = this.glitch.scanLineJitter;
			to = initial;
			step = 0f;
			while(step < 1f) {
				step += Time.deltaTime * this.speed;
				this.glitch.scanLineJitter = Mathf.Lerp(from, to, step);
				yield return null;
			}

			yield return new WaitForSeconds(Random.Range(this.interval[0], this.interval[1]));
		}
	}
}
