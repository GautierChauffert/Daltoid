using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class EventZoneController : MonoBehaviour
{
	public UnityEvent _event = new UnityEvent();

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Player")) {
			this._event.Invoke();
		}
	}
}
