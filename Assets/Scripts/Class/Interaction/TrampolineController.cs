using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineController : MonoBehaviour
{
	[SerializeField, Range(0f, 300f)] private float heightForce = 10f;
	private Animator aim;
	private PlayerCharacter playerCharacter;


	public void Awake()
	{
		aim = GetComponentInChildren<Animator>();
	}

	private void Start()
	{
		playerCharacter = PlayerManager.instance.playerCharacter;
	}

	public void OnCollisionEnter2D(Collision2D other)
	{
		if(!other.transform.gameObject.CompareTag("Player")) {
			return;
		}

        AudioManager.instance.PlaySound("Trampoline");
        aim.SetTrigger("Boop");
		playerCharacter.Impulse(Vector2.up * this.heightForce * 100f);
	}
}
