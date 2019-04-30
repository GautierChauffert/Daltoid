using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class LightManager : Singleton<LightManager>
{
	[Header("Parameters")]
	[SerializeField] private Transform lightFolder = null;
	[SerializeField, Range(0f, 500f)] private float margin = 5f;

	[Header("Effect")]
	[SerializeField, Range(0, 100)] private int probability = 50;
	[SerializeField, Range(0f, 20f)] private float range = 5f;
	[SerializeField, Range(0f, 50f)] private float speed = 5f;
	[SerializeField] private AnimationCurve interval = new AnimationCurve();

	private Transform player;
	private float radius;
	private Light[] lights;
	private Vector3 pos;


	protected override void Awake()
	{
		base.Awake();

		this.lights = this.lightFolder.GetComponentsInChildren<Light>(true);
		foreach(Light l in this.lights) {
			l.enabled = false;
		}
	}

	void Start()
	{
		this.player = PlayerManager.instance.playerObject.transform;
	}

	void Update()
	{
		foreach(Light light in this.lights)
		{
			this.radius = Mathf.Abs(light.transform.position.z) * Mathf.Tan(light.spotAngle);
			this.pos.Set(light.transform.position.x, light.transform.position.y, 0f);

			if(Vector3.Distance(this.player.position, this.pos) < this.radius + this.margin)
			{
				if(!light.enabled) {
					light.enabled = true;
					this.TryBrokenLightEffect(light);
				}
			}
			else if(light.enabled)
			{
				light.enabled = false;
			}
		}
	}

	private void TryBrokenLightEffect(Light light)
	{
		if(Random.Range(0, 100) >= this.probability || !light.enabled) {
			return;
		}

		this.StartCoroutine(this.BrokenLightEffectCoroutine(light));
	}

	private IEnumerator BrokenLightEffectCoroutine(Light light)
	{
		float step;
		float initial, from, to;

		initial = light.intensity;

		while(light.enabled)
		{	
			step = 0f;
			from = light.intensity;
			to = initial - Random.value * this.range;
			while(step < 1f) {

				step += this.speed * Time.deltaTime;
				light.intensity = Mathf.MoveTowards(from, to, step);
				yield return null;
			}

			yield return new WaitForSeconds(this.interval.Evaluate(Random.value));
		}

		light.intensity = initial;
	}
}
