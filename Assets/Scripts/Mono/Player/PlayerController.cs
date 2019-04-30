using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PlayerCharacter))]
public abstract class PlayerController<T> : MonoBehaviour where T : class
{
	private static T _instance = null;
	public static T instance {
		get {
			if(_instance == null) {
				// Debug.LogError($"ERROR : Instance of {typeof(T)} is null, either you tried to access it from the Awake function or it has not been initialized in its own Awake function");
			}

			return _instance;
		}
		set {
			if(value != null && _instance != null) {
				// Debug.LogWarning("WARNING : Several instance of {typeof(T)} has been set ! Check it out.");
				return;
			}

			_instance = value;
		}
	}
	

	protected Transform tr;
	protected PlayerCharacter pc;
	protected Rigidbody2D rb;
	protected CapsuleCollider2D cl;

	[Header("Data")]
	public PlayerControllerData playerData;


	protected virtual void Awake()
	{
		if(_instance == null) {
			instance = this as T;
		} else {
			Destroy(gameObject);
		}

		this.tr = transform;
		this.pc = GetComponent<PlayerCharacter>();
		this.rb = GetComponent<Rigidbody2D>();
		this.cl = GetComponent<CapsuleCollider2D>();
	}

	protected virtual void OnDisable()
	{
		instance = null;
	}
}
