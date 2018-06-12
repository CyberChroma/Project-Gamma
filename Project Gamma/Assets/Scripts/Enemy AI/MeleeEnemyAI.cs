using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAI : MonoBehaviour {

    // This enemy's attack pattern is to run at the player and attack them

	public float moveSensitivity = 0.1f; // Used to make the enemy movement less snappy
    public float radius = 20;

	private Animator anim; // Reference to the animator
    private Health health;
	private MoveByForce enemyMove; // Reference to the move script
	private Transform player; // Reference to the player
    private float moveSpeed;
    private DamageByTouchCollision damageByTouchCollision;

	// Use this for initialization
	void Awake () {
        enemyMove = GetComponent<MoveByForce> (); // Getting the reference
        anim = GetComponentInChildren<Animator> (); // Getting the reference
        health = GetComponent<Health> ();
        damageByTouchCollision = GetComponent<DamageByTouchCollision>();
		player = GameObject.Find ("Cube Character").transform; // Getting the reference
	}

	void OnEnable () {
        if (anim) {
            anim.speed = Random.Range(0.9f, 1.1f);
        }
	}

	// Update is called once per frame
	void FixedUpdate () {
        if (player && Vector3.Distance(player.position, transform.position) <= radius) { 
            Vector3 dir = player.position - transform.position; // Sets its direction
            dir = new Vector3(dir.x, 0, dir.z); // Elimintates y value
            enemyMove.dir = Vector3.Lerp (enemyMove.dir, dir.normalized, moveSensitivity); // Sets the magnitude to 1
            if (dir.magnitude >= 0.1f) {
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.position - transform.position), 0.1f); // Looks at the player
                transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0); // Ignores x and z values
            }
        } else {
            enemyMove.dir = Vector3.Lerp (enemyMove.dir, Vector3.zero, moveSensitivity);
        }
        if (health.currentHealth <= 0) {
            enemyMove.dir = Vector3.zero;
            damageByTouchCollision.canDamage = false;
            enabled = false;
            health.ChangeHealth();
        }
	}

	void OnCollisionEnter (Collision other) { // If the enemy collided with something, change its move target.
        if (other.collider.CompareTag ("Player") && anim) {
            anim.SetTrigger("Attack");
        }
	}
}
