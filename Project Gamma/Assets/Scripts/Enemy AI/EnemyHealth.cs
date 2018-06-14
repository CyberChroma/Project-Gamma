using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    private Health health;

	// Use this for initialization
	void Awake () {
        health = GetComponent<Health> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
        if (health.currentHealth <= 0) {
            //enemyMove.dir = Vector3.zero;
            enabled = false;
            health.Damage();
        }
	}
}
