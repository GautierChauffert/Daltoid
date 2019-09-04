using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Damageable;

public class MeleeRangerEnemyController : MovementController
{
	private PatrolController patrolController;
	public MeleeRangerEnemy enemy;
	private Rigidbody2D rb;
	private Animator enemyAnimator;

	private Transform playerTransform;
	private PlayerCharacter playerCharacter;
	private IDamageableEntity playerDamageable;

	public MeleeRangerEnemyController(MeleeRangerEnemy e) : base(e)
	{
		this.enemy = e;
		this.enemyAnimator = e.GetComponentInChildren<Animator>();
		this.transform = e.transform;
		this.rb = e.GetComponent<Rigidbody2D>();
		this.patrolController = e.GetComponent<PatrolController>();

		this.playerTransform = PlayerManager.instance.playerObject.transform;
		this.playerCharacter = PlayerManager.instance.playerCharacter;
		this.playerDamageable = this.playerTransform.GetComponent<IDamageableEntity>();
	}

	public void Punch()
	{
		Vector2 recul;
		int i = Random.Range(1, 4);

		this.rb.velocity = Vector2.zero;
        
		this.enemyAnimator.Play("Shoot");

		recul = (playerTransform.position - transform.position).normalized;
		this.playerDamageable.GetDamagedBy(enemy.color, enemy.damage);
		this.playerCharacter.Impact(recul * 1000f);

		AudioManager.instance.PlaySound("EnemyHitting");
        AudioManager.instance.PlaySound("Enemy" + i);
      


    }

	public IEnumerator Patrol()
	{
		this.enemyAnimator.SetBool("Walk", true);
		while (true) {

			this.rb.velocity = patrolController.GetNextVelocity();
			this.UpdateRotationTowards(enemy, this.rb.velocity);
			yield return null;
		}
	}
}