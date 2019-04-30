using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : Singleton<WeaponManager>
{
	public Transform folder;

	protected override void Awake()
	{
		base.Awake();

		if(folder == null) {
			folder = GameObject.Find("Weapons")?.transform;
		}
		if(folder == null) {
			this.InitializeFolder();
		}
	}

	private void InitializeFolder()
	{
		GameObject g = new GameObject();

		g.name = "Weapons";

		this.folder = g.transform;
		this.folder.position = Vector3.zero;
	}
}
