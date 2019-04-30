using UnityEngine;
using Damageable;

public class SawEnemy : MonoBehaviour
{
	[SerializeField, Range(0f, 100f)] private float damagePerSeconds = 50f;
	[SerializeField, Range(0f, 100f)] private float rotationSpeed = 10f;
	[SerializeField] private bool isMoving = true;
	[SerializeField] private float moveSpeed = 10f;

	private Transform saw;
	private PlayerMoveController playerController;


	protected void Start()
	{
		saw = GetComponentInChildren<SpriteRenderer>().transform;
	}

	public void Update()
	{
		saw.Rotate(Vector3.forward, this.rotationSpeed);

		if(isMoving) {
			this.MoveSaw();
		}
	}

	public void OnCollisionStay2D(Collision2D collision)
	{
		IDamageableEntity ent = collision.transform.GetComponent<IDamageableEntity>();

		if(ent != null)
		{
			this.GiveDamage(ent, collision.transform);
			AudioManager.instance.PlaySound("SawHit");
		}
	}

	public void GiveDamage(IDamageableEntity ent, Transform enty)
	{
		ent.GetDamagedBy(Color.white, this.damagePerSeconds * Time.deltaTime);
	}

	public void MoveSaw()
	{
		transform.localPosition = new Vector3(0, -1 *  Mathf.PingPong(Time.time * moveSpeed, 1), 0);
	}
}
