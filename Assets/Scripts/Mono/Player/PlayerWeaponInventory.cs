using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class PlayerWeaponInventory : MonoBehaviour
{
	private Transform folder;
	[HideInInspector] public Transform shoulder;

	private HUDManager hudManager;
	private InputManager inputManager;
	private PlayerCharacter playerCharacter;
	private SingleColorEffect singleColorEffect;

	private const int INVENTORY_SIZE = 5;
	private int weaponLayerMask;

	public Weapon current { get; private set; }
	private List<Weapon> weapons;

	private Collider2D[] results = new Collider2D[5];


	void Awake()
	{
		this.weaponLayerMask = (1 << LayerMask.NameToLayer("Weapon"));

		this.InitializeFolders();
		
		// weapon
		this.weapons = new List<Weapon>();
		this.current = GetComponentInChildren<Weapon>();
		this.current.Owned(playerCharacter);
		this.weapons.Add(this.current);
	}

	void Start()
	{
		// scripts
		inputManager = InputManager.instance;
		hudManager = HUDManager.instance;
		playerCharacter = PlayerManager.instance.playerCharacter;
		singleColorEffect = SingleColorEffect.instance;

		// weapon
		this.SwitchWeapon(0, false);
		this.current.gameObject.SetLayerWithChildren("Player");
	}

	void Update()
	{
		// PICK
		if(Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") == -1) {
			this.TryPickWeapon();
		}

		// SCROLL SWITCH
		int scroll = (int)Input.mouseScrollDelta.y;
		if(scroll != 0) {
			this.ScrollWeapon(scroll);
		}

		// SWITCH
		if(Input.GetKeyDown("1")) {
			this.TrySwitchWeapon(0);
		} else if(Input.GetKeyDown("2")) {
			this.TrySwitchWeapon(1);
		} else if(Input.GetKeyDown("3")) {
			this.TrySwitchWeapon(2);
		} else if(Input.GetKeyDown("4")) {
			this.TrySwitchWeapon(3);
		} else if(Input.GetKeyDown("5")) {
			this.TrySwitchWeapon(4);
		}

		// DROP
		if(Input.GetKeyDown("c")) {
			this.TryDropWeapon();
		}
	}

	



	/* -------------------------------------------------------------------------------------------------- */
	/* -------------------------------------------------------------------------------------------------- */
	/* -------------------------------------------------------------------------------------------------- */
	/* ----- WEAPON METHODS ----------------------------------------------------------------------------- */
	/* -------------------------------------------------------------------------------------------------- */
	/* -------------------------------------------------------------------------------------------------- */
	/* -------------------------------------------------------------------------------------------------- */
	private void TryPickWeapon()
	{
		Weapon found;

		// search weapon on the ground
		found = this.SearchForWeapon();

		if(found == null) {
			return;
		}
			
		if(this.weapons.Count >= INVENTORY_SIZE) {
			// Debug.Log("HERE A WARNING MESSAGE.");
			return;
		}

		this.PickWeapon(found);
	}
	private void PickWeapon(Weapon w)
	{
		int currentIndex;

		// add
		this.weapons.Add(w);

		// update ui
		currentIndex = this.weapons.IndexOf(this.current);
		hudManager.UpdateInventory(this.weapons, currentIndex);

		// sound
		AudioManager.instance.PlaySound("WeaponPickUp3");

		// stow weapon
		w.transform.parent = folder;
		w.gameObject.SetActive(false);
		w.gameObject.SetLayerWithChildren("Player");
		w.Owned(playerCharacter);
	}


	private void TrySwitchWeapon(int index)
	{
		if(index >= this.weapons.Count || index == this.weapons.IndexOf(this.current)) {
			// Debug.Log("HERE A WARNING.");
			return;
		}
		
		this.SwitchWeapon(index);
	}
	private void SwitchWeapon(int index, bool sound = true)
	{
		// stow previous weapon
		if(this.current != null) {
			this.current.transform.parent = folder;
			this.current.gameObject.SetActive(false);
		}
		
		// switch
		this.current = this.weapons[index];
		this.current.gameObject.SetActive(true);
		this.current.Owned(playerCharacter);

		// position
		this.current.transform.parent = shoulder;
		this.current.transform.localPosition = new Vector3(0, -this.current.handle.y, -this.current.handle.x);
		this.current.transform.localRotation = Quaternion.Euler(0, -90, 0);

		// update hud
		this.hudManager.UpdateWeapon(1f, this.current.color);
		this.hudManager.UpdateInventory(this.weapons, index);
		this.singleColorEffect.color = this.current.color;

		// sound
		if(sound) {
			AudioManager.instance.PlaySound("Switch");
		}
	}


	private void ScrollWeapon(int scroll)
	{
		int currentIndex, newIndex;

		currentIndex = this.weapons.IndexOf(this.current);
		newIndex = (currentIndex + scroll) % this.weapons.Count;
		if(newIndex < 0) {
			newIndex += this.weapons.Count;
		}

		this.TrySwitchWeapon(newIndex);
	}


	private void TryDropWeapon()
	{
		if(this.weapons.Count <= 1) {
			// Debug.Log("HERE ANOTHER WARNING YOU BUTT.");
			return;
		}

		this.DropWeapon();
	}
	private void DropWeapon()
	{
		int dropIndex, currentIndex;

		// calculate index
		dropIndex = this.weapons.IndexOf(this.current);
		currentIndex = (dropIndex == this.weapons.Count - 1) ? this.weapons.Count - 2 : dropIndex;

		// drop current
		this.current.DropOnGround();
		this.weapons.RemoveAt(dropIndex);
		this.current.gameObject.SetLayerWithChildren("Weapon");
		this.current = null;

		// sound
		AudioManager.instance.PlaySound("Drop");

		// set new weapon
		this.SwitchWeapon(currentIndex);
	}


	public void HideWeapon()
	{
		shoulder.gameObject.SetActive(false);
	}








	/* -------------------------------------------------------------------------------------------------- */
	/* -------------------------------------------------------------------------------------------------- */
	/* -------------------------------------------------------------------------------------------------- */
	/* ----- UTIL METHODS ------------------------------------------------------------------------------- */
	/* -------------------------------------------------------------------------------------------------- */
	/* -------------------------------------------------------------------------------------------------- */
	/* -------------------------------------------------------------------------------------------------- */
	private void InitializeFolders()
	{
		GameObject folder;

		folder = new GameObject();
		folder.name = "Player Weapon Folder";

		this.folder = folder.transform;
		this.shoulder = transform.DeepFind("Shoulder");
	}

	private Weapon SearchForWeapon()
	{
		int count;
		float radius;
		Vector2 point;
		Weapon w;

		point = transform.position;
		radius = 1.5f;
		count = Physics2D.OverlapCircleNonAlloc(point, radius, this.results, weaponLayerMask);

		if(count == 0) {
			return null;
		}

		foreach(Collider2D c in this.results)
		{
			if(c == null) {
				continue;
			}

			w = c.gameObject.GetComponent<Weapon>();
			if(w != null && !this.weapons.Contains(w)) {
				return w;
			}
		}
	
		return null;
	}
}
