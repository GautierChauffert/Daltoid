using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Shadow))]
public class ShadowUIGlitcher : MonoBehaviour
{
	[Header("Range")]
	[SerializeField] private Vector2 xRange = Vector2.zero;
	[SerializeField] private Vector2 yRange = Vector2.zero;

	[Header("Parameters")]
	[SerializeField, Range(0.01f, 2f)] private float smooth = 0.1f;
	[SerializeField] private Vector2 interval = Vector2.zero;

	private Shadow shadow;

	void Awake()
	{
		this.shadow = GetComponent<Shadow>();
	}

	void Start()
	{
		StartCoroutine(this.EffectCoroutine());
	}

	private IEnumerator EffectCoroutine()
	{
		Vector2 from, to;
		float step, speed;

		to = Vector2.zero;

		while(true)
		{
			from = this.shadow.effectDistance;
			to.Set(Random.Range(xRange[0], xRange[1]), Random.Range(yRange[0], yRange[1]));
			speed = Vector2.Distance(from, to) / this.smooth;
			step = 0f;

			while(step < 1f) {
				step += Time.deltaTime * speed;
				this.shadow.effectDistance = Vector2.Lerp(from, to, step);
				yield return null;
			}

			yield return new WaitForSeconds(Random.Range(this.interval[0], this.interval[1]));
		}
	}
}
