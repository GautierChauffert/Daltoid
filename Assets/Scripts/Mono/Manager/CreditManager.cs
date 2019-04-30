using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditManager : Singleton<CreditManager>
{
	[Header("Dark")]
	[SerializeField] private Image dark = null;
	[SerializeField, Range(0.01f, 10f)] private float fadeSpeed = 2f;

	[Header("Background")]
	[SerializeField] private Transform background1 = null;
	[SerializeField] private Transform background2 = null;

	[Header("Parameters")]
	[SerializeField, Range(0f, 50f)] private float scrollSpeed = 10f;
	[SerializeField, Range(0f, 50f)] private float threshold = 10f;
	[SerializeField, Range(0f, 50f)] private float offset = 24f;

	protected override void Awake()
	{
		base.Awake();
		
		this.dark.color = new Color(dark.color.r, dark.color.g, dark.color.b, 1f);
	}

	void Start()
	{
		this.StartCoroutine(this.FadeAwayCoroutine());
	}

	void Update()
	{
		if(Input.GetButtonDown("Quit")) {
			Application.Quit();
		}
		
		this.background1.Translate(Vector3.up * this.scrollSpeed * Time.deltaTime);
		this.background2.Translate(Vector3.up * this.scrollSpeed * Time.deltaTime);

		if(this.background1.position.y >= this.threshold) {
			this.background1.position = this.background2.position - Vector3.up * offset;
		}

		if(this.background2.position.y >= this.threshold) {
			this.background2.position = this.background1.position - Vector3.up * offset;
		}
	}

	private IEnumerator FadeAwayCoroutine()
	{
		Color from, to;
		float step;

		from = this.dark.color;
		to = new Color(dark.color.r, dark.color.g, dark.color.b, 0f);
		step = 0f;

		while(step < 1f) {
			step += this.fadeSpeed * Time.deltaTime;
			this.dark.color = Color.Lerp(from, to, step);
			yield return null;
		}

		this.dark.color = to;
	}
}
