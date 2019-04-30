using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Tools;

public class AudioManager : Singleton<AudioManager>
{
	[System.Serializable]
	public struct AudioCouple
	{
		public string name;
		public AudioClip clip;
	}

	[SerializeField] private AudioSource musicSourceReference = null;
	[SerializeField] private AudioSource soundSourceReference = null;

	[Header("Musics")]
	[SerializeField] private List<AudioCouple> musics = new List<AudioCouple>();

	[Header("Sounds")]
	[SerializeField] private List<AudioCouple> sounds = new List<AudioCouple>();
	[SerializeField, Range(1, 10)] private int maxSoundSource = 5;

	[Header("Parameters")]
	[SerializeField, Range(0.01f, 10f)] private float musicTransitionSpeed = 1f; 


	private IEnumerator damageCoroutine = null;
	private IEnumerator musicCoroutine = null;

	private AudioSource[] soundSources;
	private AudioSource[] musicSources;
	private AudioSource currentMusicSource = null;


	protected override void Awake()
	{
		base.Awake();

		AudioSource src;

		this.soundSources = this.soundSourceReference.GetComponents<AudioSource>();
		for(int i = 0; i < this.maxSoundSource - this.soundSources.Length; i++) {
			src = this.soundSourceReference.gameObject.AddComponent<AudioSource>();

			src.bypassEffects = this.soundSourceReference.bypassEffects;
			src.outputAudioMixerGroup = this.soundSourceReference.outputAudioMixerGroup;
			src.priority = this.soundSourceReference.priority;
			src.volume = this.soundSourceReference.volume;
		}
		this.soundSources = this.soundSourceReference.GetComponents<AudioSource>();

		this.musicSources = this.musicSourceReference.GetComponents<AudioSource>();
		this.currentMusicSource = this.musicSources[0];
	}

	public void PlaySound(string name)
	{
		AudioClip clip = this.sounds.Find(x => x.name == name).clip;
		AudioSource selected = null;
		
		foreach(AudioSource s in this.soundSources) {
			if(!s.isPlaying || s.clip == clip) {
				selected = s;
				break;
			}
		}

		if(selected == null) {
			return;
		}

		selected.clip = clip;
		selected.Play();
	}

	public void PlayMusic(string name)
	{
		AudioClip clip;
		AudioSource from, to;

		clip = this.musics.Find(x => x.name == name).clip;
		if(this.musicSources[0].isPlaying) {
			from = this.musicSources[0];
			to = this.musicSources[1];
		} else {
			from = this.musicSources[1];
			to = this.musicSources[0];
		}

		this.currentMusicSource = to;

		if(!from.isPlaying) {
			from.clip = clip;
			from.Play();
			return;
		}

		to.enabled = true;
		to.volume = 0f;
		to.clip = clip;
		to.Play();

		this.StartAndStopCoroutine(ref this.musicCoroutine, this.MusicTransition(from, to));
	}

	public void StopMusic()
	{

	}

	private IEnumerator MusicTransition(AudioSource from, AudioSource to)
	{
		float step;
		float fromSourceValue, toSourceValue;

		fromSourceValue = from.volume;
		toSourceValue = to.volume;
		step = 0f;

		while(step < 1f) {
			step += this.musicTransitionSpeed * Time.deltaTime;
			from.volume = Mathf.Lerp(fromSourceValue, 0f, step);
			to.volume = Mathf.Lerp(toSourceValue, 1f, step);

			yield return null;
		}

		from.volume = 0f;
		to.volume = 1f;
		from.enabled = false;
	}


	public void Damage(float duration, float low = 0.8f)
	{
		// only does that
		this.StartAndStopCoroutine(ref this.damageCoroutine, this.DamageCoroutine(duration, low));
	}
	private IEnumerator DamageCoroutine(float duration, float low)
	{
		float from, to;
		float speed, step;

		from = this.currentMusicSource.pitch;
		to = low;
		speed = Mathf.Abs(1f - to) * 2f / duration;

		step = 0f;
		while(step < 1f) {

			step += speed * Time.deltaTime;
			this.currentMusicSource.pitch = Mathf.Lerp(from, to, step);
			yield return null;
		}

		from = this.currentMusicSource.pitch;
		to = 1f;
		step = 0f;
		while(step < 1f) {

			step += speed * Time.deltaTime;
			this.currentMusicSource.pitch = Mathf.Lerp(from, to, step);
			yield return null;
		}

		this.currentMusicSource.pitch = to;
		this.damageCoroutine = null;
	}
}
