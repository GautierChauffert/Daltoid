using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

// [ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class Glitch : Singleton<Glitch>
{
	[Header("Shader Glitch")]
	[SerializeField] private Shader shader = null;
	private Material material;

	[Header("Displacement Glitch")]
	[SerializeField] private Texture2D displacementMap = null;
	[Range(0.0f, 5.0f)] public float intensity;
	private float glitchup = 0f;
	private float glitchdown = 0f;
	private float flicker = 0f;
	private float glitchupTime = 0.05f;
	private float glitchdownTime = 0.05f;
	private float flickerTime = 0.5f;

	[Header("Analog Glitch")]
	// [SerializeField, Range(0, 1)] private float scanLineJitter = 0f;
	[Range(0, 1)] public float scanLineJitter = 0f;
	[SerializeField, Range(0, 1)] private float verticalJump = 0f;
	[SerializeField, Range(0, 1)] private float horizontalShake = 0f;
	[SerializeField, Range(0, 1)] private float colorDrift = 0f;
	private float verticalJumpTime = 0f;


	private IEnumerator glitchCoroutine = null;
	


	protected override void Awake()
	{
		base.Awake();

		if(this.shader == null) {
			this.shader = Shader.Find("Hidden/GlitchShader");
		}
		this.material = new Material(this.shader);
		this.material.hideFlags = HideFlags.DontSave;
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if(this.material == null) {
			this.material = new Material(this.shader);
			// this.material.hideFlags = HideFlags.DontSave;
		}

		this.AnalogGlitch(this.material);
		this.DisplacementGlitch(this.material);

		Graphics.Blit(source, destination, this.material);
	}

	public void Launch(float duration)
	{
		this.enabled = true;

		this.StartAndStopCoroutine(ref this.glitchCoroutine, this.GlitchCoroutine(duration));
	}

	private IEnumerator GlitchCoroutine(float duration)
	{
		yield return new WaitForSeconds(duration);

		this.enabled = false;
	}

	private void DisplacementGlitch(Material mat)
	{
		if(this.intensity == 0) {
			mat.SetFloat("flip_up", 0);
			mat.SetFloat("flip_down", 1);
			return;
		}

		mat.SetFloat("_Intensity", this.intensity);
		mat.SetTexture("_DispTex", this.displacementMap);
		
		this.glitchup += Time.deltaTime * this.intensity;
		this.glitchdown += Time.deltaTime * this.intensity;
		this.flicker += Time.deltaTime * this.intensity;
		
		if(this.flicker > this.flickerTime) {
			mat.SetFloat("filter_radius", Random.Range(-3f, 3f) * this.intensity);
			this.flicker = 0;
			this.flickerTime = Random.value;
		}
		
		if(this.glitchup > this.glitchupTime)
		{
			if(Random.value < 0.1f * this.intensity) {
				mat.SetFloat("flip_up", Random.Range(0, 1f) * this.intensity);
			} else {
				mat.SetFloat("flip_up", 0);
			}
			
			this.glitchup = 0;
			this.glitchupTime = Random.value/10f;
		}
		
		if(this.glitchdown > this.glitchdownTime)
		{
			if(Random.value < 0.1f * this.intensity) {
				mat.SetFloat("flip_down", 1-Random.Range(0, 1f) * this.intensity);
			} else {
				mat.SetFloat("flip_down", 1);
			}
			
			this.glitchdown = 0;
			this.glitchdownTime = Random.value/10f;
		}
		
		if(Random.value < 0.05 * this.intensity) {
			mat.SetFloat("displace", Random.value * this.intensity);
			mat.SetFloat("scale", 1-Random.value * this.intensity);
		} else {
			mat.SetFloat("displace", 0);
		}
	}

	private void AnalogGlitch(Material mat)
	{
		float slthresh, sldisp;
		Vector2 vj, cd;

		this.verticalJumpTime += Time.deltaTime * this.verticalJump * 11.3f;

		slthresh = Mathf.Clamp01(1.0f - this.scanLineJitter * 1.2f);
		sldisp = 0.002f + Mathf.Pow(this.scanLineJitter, 3) * 0.05f;
		this.material.SetVector("_ScanLineJitter", new Vector2(sldisp, slthresh));

		vj = new Vector2(this.verticalJump, this.verticalJumpTime);
		this.material.SetVector("_VerticalJump", vj);

		this.material.SetFloat("_HorizontalShake", this.horizontalShake * 0.2f);

		cd = new Vector2(this.colorDrift * 0.04f, Time.time * 606.11f);
		this.material.SetVector("_ColorDrift", cd);
	}
}
