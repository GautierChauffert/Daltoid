using UnityEngine;
using System.Collections;

// [ExecuteInEditMode]
public class SingleColorEffect : Singleton<SingleColorEffect>
{
	[Header("Target Color")]
	[SerializeField] private Color _color = Color.red;
	public Color color {
		get { return this._color; }
		set {
			this._color = value;
			this.UpdateMaterial();
		}
	}

	[Header("Parameters")]
	[Range(0f, 1f), SerializeField] private float _threshold = 0.25f;
	public float threshold {
		get { return this._threshold; }
		set {
			this._threshold = value;
			this.UpdateMaterial();
		}
	}
	

	private Material material;

	public Shader shader;


	protected override void Awake()
	{
		base.Awake();
		
		// Creates a private material used to the effect
		this.material = new Material(shader);
		this.UpdateMaterial();
	}

	void OnValidate()
	{
		this.UpdateMaterial();
	}
	
	// Postprocess the image
	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if(this.threshold >= 1f) {
			Graphics.Blit(source, destination);
			return;
		}

		if(this.material != null) {
			Graphics.Blit(source, destination, this.material);
		}
	}

	private void UpdateMaterial()
	{
		if(this.material == null) {
			return;
		}

		this.material.SetFloat("_threshold", this.threshold);
		this.material.SetColor("_targetColor", this.color);
	}
}