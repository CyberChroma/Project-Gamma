using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour {

	private Health health; // The health script to track
	private bool canTakeDamage = true;

	// Use this for initialization
	void Awake () {
		health = GetComponent<Health> (); // Getting the reference
	}

	void OnEnable () {
		canTakeDamage = true;
	}

	void OnDisable () {
		canTakeDamage = false;
	}

	public void Damage (float damageAmount) {
		if (health.canBeHit && canTakeDamage) { // If the object can be hit
			health.currentHealth -= damageAmount; // Lose health
			health.healthChanged = true; // Updates the UI
			if (gameObject.activeSelf) {
				StartCoroutine (health.TempStopHits ()); // Temporarily stops the object from taking damage
			}
            health.ChangeHealth();
		}
	}
}
