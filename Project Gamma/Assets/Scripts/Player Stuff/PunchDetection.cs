using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchDetection : MonoBehaviour {
	
	private CubeAbilities cubeAbilities; // Reference to the cube abilities script

	void Start () {
		cubeAbilities = GetComponentInParent<CubeAbilities> (); // Getting the reference
	}

	void OnTriggerStay (Collider other) {
		if (cubeAbilities.isPunching) { // If the cube is punching
			if (other.CompareTag ("Breakable")) { // If the player collided with a breakable wall
				other.GetComponentInChildren<Animator> ().SetTrigger ("Activate"); // Activates the animation
			} else if (other.CompareTag ("Button-Cube")) { // If the player collided with a button
				other.GetComponent<ButtonController> ().Activate (); // Activate the button
			}
		}
	}
}
