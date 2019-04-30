using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallEvent : MonoBehaviour, IEventEntity
{
	private PlayerCharacter playerCharacter;

	void Start()
	{
		this.playerCharacter = PlayerManager.instance.playerCharacter;
	}

	public void Invoke()
	{
		this.playerCharacter.GetDamagedBy(Color.white, 1000000f);
	}
}
