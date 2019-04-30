using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerEndLevel : MonoBehaviour, IEventEntity
{
    [Header("Dark")]
	[SerializeField] private Image dark = null;
	[SerializeField, Range(0.01f, 10f)] private float fadeSpeed = 2f;

	void Awake()
	{
		this.dark.color = new Color(dark.color.r, dark.color.g, dark.color.b, 0f);
	}

	public void Invoke()
	{
		this.StartCoroutine(this.EndLevelCoroutine());
	}

	private IEnumerator EndLevelCoroutine()
	{
		yield return this.FadeAwayCoroutine();

		SceneManager.LoadScene("Credit");
	}

	private IEnumerator FadeAwayCoroutine()
	{
		Color from, to;
		float step;

		from = this.dark.color;
		to = new Color(dark.color.r, dark.color.g, dark.color.b, 1f);
		step = 0f;

		while(step < 1f) {
			step += this.fadeSpeed * Time.deltaTime;
			this.dark.color = Color.Lerp(from, to, step);
			yield return null;
		}

		this.dark.color = to;
	}
}
