using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Damageable;

public class MiteEnemyController : MovementController
{
	private PatrolController patrolController;
	private MiteEnemy enemy;
	private Rigidbody2D rb;
	private Animator enemyAnimator;

	private Transform playerTransform;
	private PlayerCharacter playerCharacter;
	private IDamageableEntity playerDamageable;

	public MiteEnemyController(MiteEnemy e) : base(e)
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

		this.rb.velocity = Vector2.zero;
		this.enemyAnimator.SetBool("Walk", false);

		recul = (playerTransform.position - transform.position).normalized;
		this.playerDamageable.GetDamagedBy(enemy.color, enemy.damage);
		this.playerCharacter.Impact(recul * 1000f);
        AudioManager.instance.PlaySound("Enemy5");
    }

	public IEnumerator Patrol()
	{
		while (true) {
			this.rb.velocity = patrolController.GetNextVelocity();
			this.UpdateRotationTowards(enemy, this.rb.velocity);
			yield return null;
		}
	}
}
