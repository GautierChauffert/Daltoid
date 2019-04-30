using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Shadow))]
public class ShadowUIColor : MonoBehaviour
{
	[Header("Parameters")]
	[SerializeField, Range(0.01f, 20f)] private float speed = 0.1f;
	[SerializeField, Range(0f, 1f)] private float alpha = 0.1f;
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
		float step;
		Color to, from;

		while(true)
		{
			from = this.shadow.effectColor;
			to = Random.ColorHSV(0f, 1f, 1f, 1f, 0.85f, 1f, this.alpha, 1f);
			step = 0f;

			while(step < 1f) {
				step += Time.deltaTime * this.speed;
				this.shadow.effectColor = Color.Lerp(from, to, step);
				yield return null;
			}

			yield return new WaitForSeconds(Random.Range(this.interval[0], this.interval[1]));
		}
	}
}
