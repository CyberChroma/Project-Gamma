using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    public float radius = 20;

	private Animator anim; // Reference to the animator

	// Use this for initialization
	void Awake () {
        anim = GetComponentInChildren<Animator> (); // Getting the reference
	}

	void OnEnable () {
        if (anim) {
            anim.speed = Random.Range(0.9f, 1.1f);
        }
	}

	// Update is called once per frame
	void FixedUpdate () {
	}

	void OnCollisionEnter (Collision other) { // If the enemy collided with something, change its move target.
        if (other.collider.CompareTag ("Player")) {
            if (anim) {
                anim.SetTrigger("Attack");
            }
        }
	}
}
