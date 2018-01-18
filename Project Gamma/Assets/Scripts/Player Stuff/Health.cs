using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

	public float startHealth = 5; // The health the object starts with
	public float tempStopHitsTime = 2; // Time between hits

	[HideInInspector] public bool healthChanged; // UI updates when true
	[HideInInspector] public float currentHealth; // The current health
	[HideInInspector] public bool canBeHit = true;


	// Use this for initialization
	void Awake () {
		// Setting start values
		currentHealth = startHealth;
	}

	public IEnumerator TempStopHits () { // Temporarily stops the object from taking more damage
		canBeHit = false;
		yield return new WaitForSeconds (tempStopHitsTime);
		canBeHit = true;
	}
}
