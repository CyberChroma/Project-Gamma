using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAI : MonoBehaviour {

    // This enemy's attack pattern is to run at the player and attack them

    public float moveSpeed = 4;
	public float moveSensitivity = 1; // Used to make the enemy movement less snappy
    public float turnSpeed = 5;
    public float radius = 10;

	private Animator anim; // Reference to the animator
    private Health health;
	private Transform player; // Reference to the player
    private Vector3 moveVector = Vector3.zero;
    private Rigidbody rb;

	// Use this for initialization
	void Awake () {
        anim = GetComponentInChildren<Animator> (); // Getting the reference
        health = GetComponent<Health> ();
		player = GameObject.Find ("Cube Character").transform; // Getting the reference
        rb = GetComponent<Rigidbody>();
	}

	void OnEnable () {
        if (anim) {
            anim.speed = Random.Range(0.9f, 1.1f);
        }
	}

	// Update is called once per frame
	void FixedUpdate () {
        if (player && Vector3.Distance(player.position, transform.position) <= radius && player.position.y > (transform.position.y - 0.1f)) {
            moveVector = Vector3.Lerp(moveVector, transform.forward * moveSpeed * Time.deltaTime, moveSensitivity / 10);
            Quaternion targetRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(player.position - transform.position), turnSpeed); // Looks at the player
            transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0); // Ignores x and z values
        } else {
            moveVector = Vector3.Lerp (moveVector, Vector3.zero, moveSensitivity / 10);
        }
        if (health.currentHealth <= 0) {
            moveVector = Vector3.zero;
            enabled = false;
            health.Damage();
        }
        rb.MovePosition (rb.position + moveVector); // Applying the movement
	}

	void OnCollisionEnter (Collision other) { // If the enemy collided with something, change its move target.
        if (other.collider.CompareTag ("Player") && anim) {
            anim.SetTrigger("Attack");
        }
	}
}
