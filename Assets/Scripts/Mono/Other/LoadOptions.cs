using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LoadOptions : MonoBehaviour
{
	public AudioMixer mixer;

    void Start()
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume", 0.75f)) * 20); 
    }
}
