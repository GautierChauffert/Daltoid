using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PatrolController : MonoBehaviour
{
	[Header("Parameters")]
	[Range(0.01f, 25f)] public float speed = 5f;
	[Range(0.01f, 2.5f)] public float threshold = 0.1f;
	[Range(0.01f, 25f)] public float smooth = 5f;

	[Header("Points")]
	public List<Vector2> points = new List<Vector2>();

	private Transform tr;

	private int index = 0;

	private Vector2 target;
	private Vector2 aim;
	private Vector2 direction;
	private Vector2 align;

	
	void Awake()
	{
		this.tr = transform;
	}

	void Start()
	{
		this.index = 0;
		this.target = this.points[this.index];

		this.aim = (this.target - (Vector2)this.tr.position).normalized;
		this.direction = this.aim;
		this.align = this.aim;
	}

	public void OnValidate()
	{
		if(this.points == null || this.points.Count == 0) {
			return;
		}
		
		this.target = this.points[0];
	}

	public Vector2 GetNextVelocity()
	{
		if(this.tr == null) {
			return Vector2.zero;
		}

		// Reach target -> switch to next target
		if(Vector2.Distance(this.tr.position, this.target) < this.threshold) {

			this.index = (this.index + 1) % this.points.Count;
			this.target = this.points[this.index];

			if(this.points.Count == 2) {
				this.align = Vector3.RotateTowards(this.align, Vector2.Perpendicular(this.aim), this.smooth * Time.deltaTime, 0f).normalized;
			}
		}

		this.aim = (this.target - (Vector2)this.tr.position).normalized;
		this.align = Vector3.RotateTowards(this.align, this.aim, this.smooth * Time.deltaTime, 0f).normalized;

		if(this.points.Count == 2) {
			this.direction = Vector3.Project(this.align, this.points[0] - this.points[1]);
		} else {
			this.direction = this.align;
		}

		// Debug.DrawRay(tr.position, aim.normalized * 5f, Color.yellow);
		// Debug.DrawRay(tr.position, direction.normalized * 5f, Color.red);
		// Debug.DrawRay(tr.position, align.normalized * 5f, Color.green);

		return this.direction * this.speed;
	}
















	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(this.points[0], 0.25f);

		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(this.points[0], this.points[this.points.Count - 1]);

		for(int i = 1; i < this.points.Count; i++) {

			Gizmos.color = Color.red;
			Gizmos.DrawSphere(this.points[i], 0.25f);

			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(this.points[i - 1], this.points[i]);
		}
	}
}
