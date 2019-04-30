using Damageable;
using UnityEngine;

public class StomperEnemy : MonoBehaviour
{
	[SerializeField]
	private float damage = 100f;
	private Color color = Color.white;
	
	private PlayerMoveController playerController;

	public void OnTriggerEnter2D(Collider2D collision)
	{
		IDamageableEntity ent = collision.transform.GetComponent<IDamageableEntity>();

		if(ent != null) {
			this.GiveDamage(ent, collision.transform);
		}
	}

	public void GiveDamage(IDamageableEntity ent, Transform enty)
	{
		ent.GetDamagedBy(this.color, this.damage);
	}

	public void PlaySound()
	{
		GameObject o = GameObject.Find("StomperTrigger");
		if(o == null) {
			return;
		}

		if(o.GetComponent<PlayerIn>().isIn == true) {
			AudioManager.instance.PlaySound("Stomper");
		}
	}
}
