using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tools;

public class HUDManager : Singleton<HUDManager>
{
	private PlayerCharacter playerCharacter;

	public Slider health;
	// public Image weapon;
	public Outline weaponOutline;
	public GameObject gameover;
	public Transform inventory;
	public GameObject pause;
	public GameObject option;
	public RectTransform bossHealthBar;

	[HideInInspector] public bool isPaused = false;
	private bool optionOpened = false;

	private Image[] slots;
	private RectTransform cursor;

	private float bossMaxHealthLife;

	private const int CURSOR_INTERVAL = 63;

	protected override void Awake()
	{
		base.Awake();

		if(this.inventory) {
			this.slots = this.inventory.GetComponentsInChildrenWithName<Image>("Icon");
			this.cursor = this.inventory.DeepFind("Cursor").GetComponent<RectTransform>();
			this.ResetInventory();
		}

		if(bossHealthBar) {
			this.bossMaxHealthLife = bossHealthBar.sizeDelta.x;
		}
	}

	void Start()
	{
		playerCharacter = PlayerManager.instance.playerCharacter;
		playerCharacter.onLifeUpdate.AddListener(UpdateHealth);
	}

	void Update()
	{
		if(Input.GetKeyDown("p"))
		{
			if(this.isPaused)
			{
				playerCharacter.inputs.cancelInput = false;
				GameManager.instance.ClosePauseMenu();
				this.isPaused = false;
				option.SetActive(false);
				pause.SetActive(false);
				this.optionOpened = false;
			}
			else if(!this.optionOpened)
			{
				playerCharacter.inputs.cancelInput = true;
                AudioManager.instance.PlaySound("Pause");
                this.isPaused = true;
				Time.timeScale = 0.0001f;
				this.Pause();
			}
		}
	}



	public void DisplayBossHealthBar(bool value)
	{
		this.bossHealthBar.gameObject.SetActive(value);
	}

	public void UpdateBossHealthBar(Boss boss, float current)
	{
		float value = this.bossMaxHealthLife * current / boss.initialLife;
		if(value < 0f) {
			value = 0f;
		}
		this.bossHealthBar.sizeDelta = new Vector2(value, this.bossHealthBar.sizeDelta.y);
	}

	public void SetOptionOpen(bool o)
	{
		this.optionOpened = o;
	}

	private void UpdateHealth(float current)
	{
		health.value = current / PlayerCharacter.PLAYER_MAXIMUM_LIFE;
	}

	public void UpdateWeapon(float fill, Color max)
	{
		if(weaponOutline == null) {
			return;
		}
		weaponOutline.effectColor = max;

		// weapon.fillAmount = fill;
	}

	private void ResetInventory()
	{
		if(this.slots == null) {
			return;
		}

		foreach(Image s in this.slots) {
			s.gameObject.SetActive(false);
		}
	}

	public void UpdateInventory(List<Weapon> weapons, int current)
	{
		if(this.slots == null) {
			return;
		}

		this.ResetInventory();

		for(int i = 0; i < weapons.Count; i++) {
			this.slots[i].gameObject.SetActive(true);
			this.slots[i].sprite = weapons[i].icon;
			this.slots[i].color = weapons[i].color;
		}

		this.cursor.anchoredPosition = new Vector2(current * CURSOR_INTERVAL, this.cursor.anchoredPosition.y);
	}

	public void GameOver()
	{
		gameover.SetActive(true);
	}

	public void Pause()
	{
		pause.SetActive(true);
	}
}
