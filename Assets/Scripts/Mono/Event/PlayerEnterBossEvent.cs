using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnterBossEvent : MonoBehaviour, IEventEntity
{
	[Header("Boss")]
	[SerializeField] private Boss boss = null;

	[Header("Parameters")]
	[SerializeField, Range(0.01f, 10f)] private float bossArrivalSmooth = 1f;

	private Transform player;

	private CameraController cameraController;
	private Camera[] cams;
	private bool done = false;

	void Start()
	{
		this.player = PlayerManager.instance.playerObject.transform;
		this.cameraController = CameraController.instance;
		this.cams = this.cameraController.GetComponentsInChildren<Camera>();
	}

	public void Invoke()
	{
		if(this.done) {
			return;
		}

		this.cameraController.height = 8f;
		AudioManager.instance.PlayMusic("Boss");

		boss.gameObject.SetActive(true);

		this.StartCoroutine(this.CameraCoroutine());
		this.StartCoroutine(this.ReachPlayerCoroutine());
		
		this.done = true;
	}

	private IEnumerator CameraCoroutine()
	{
		float step;
		float from;

		from = this.cams[0].orthographicSize;
		step = 0f;

		while(step < 1f) {
			step += 12.5f * Time.deltaTime;
			foreach(Camera c in this.cams) {
				c.orthographicSize = Mathf.Lerp(from, 10f, step);
			}
			yield return null;
		}
	}

	private IEnumerator ReachPlayerCoroutine()
	{
		Transform tr;
		Vector3 target, reference;

		reference = Vector3.zero;
		tr = this.boss.transform;

		do {
			target = this.player.position + Vector3.up * 8.5f;
			tr.position = Vector3.SmoothDamp(tr.position, target, ref reference, this.bossArrivalSmooth);
			yield return null;
		}
		while(Vector3.Distance(tr.position, target) > 0.5f);

		boss.enabled = true;
		boss.interval = Time.time + 3f;
		boss.mode = Boss.BossMode.Follow;
	}
}
