using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Tools;

[RequireComponent(typeof(Text))]
public class TextUIReader : MonoBehaviour
{
	[Header("Parameters")]
	[SerializeField, Range(0.01f, 2f)] private float characterDuration = 0.1f;
	[SerializeField, Range(0.01f, 2f)] private float punctuationDuration = 0.1f;

	[Header("Text")]
	[SerializeField, TextArea] private string text = null;

	[Header("Events")]
	[SerializeField] private UnityEvent onEndReading = new UnityEvent();

	private Text textComponent;

	private WaitForSeconds waitForCharacter;
	private WaitForSeconds waitForPunctuation;

	private IEnumerator readCoroutine = null;


	void Awake()
	{
		this.textComponent = GetComponent<Text>();
		this.SetDelay();
		this.textComponent.text = "";
	}

	public void Read()
	{
		this.textComponent.text = "";
		this.StartAndStopCoroutine(ref this.readCoroutine, this.EffectCoroutine());
	}

	private IEnumerator EffectCoroutine()
	{
		string word;
		string[] words = this.text.Split(' ');
		string[] duet;

		for(int i = 0; i < words.Length - 1; i++) {
			word = words[i];
			duet = word.Split('\n');

			if(duet.Length > 1)
			{
				for(int j = 0; j < duet.Length - 1; j++) {
					word = duet[j];
					this.textComponent.text += word;
					this.textComponent.text += '\n';
					yield return this.GetDelay(word);
				}

				word = duet[duet.Length - 1];
				this.textComponent.text += word;
				this.textComponent.text += ' ';
				yield return this.GetDelay(word);
			}
			else
			{
				this.textComponent.text += word;
				this.textComponent.text += ' ';
				yield return this.GetDelay(word);
			}
		}

		word = words[words.Length - 1];
		this.textComponent.text += word;
		yield return this.GetDelay(word);

		this.onEndReading.Invoke();
	}


	private void SetDelay()
	{
		this.waitForPunctuation = new WaitForSeconds(this.punctuationDuration);
		this.waitForCharacter = new WaitForSeconds(this.characterDuration);
	}

	private WaitForSeconds GetDelay(string word)
	{
		char c = word[word.Length - 1];

		switch(c) {

			case '!':
			case '?':
			case '.':
				return this.waitForPunctuation;

			default:
				return this.waitForCharacter;
		}
	}
}
