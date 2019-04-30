using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class Checkpoint : MonoBehaviour
{
	private ParticleSystem explosion;

	void Awake()
	{
		this.explosion = transform.GetComponentInChildrenWithName<ParticleSystem>("Explosion");
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Player"))
		{
			PlayerPrefs.SetFloat("CheckpointPositionX", transform.position.x);
			PlayerPrefs.SetFloat("CheckpointPositionY", transform.position.y);

			this.explosion.Play();
			AudioManager.instance.PlaySound("CheckPoint");
		}
	}
}
