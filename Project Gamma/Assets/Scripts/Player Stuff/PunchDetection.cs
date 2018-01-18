using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchDetection : MonoBehaviour {
	
	private PyramidPunch pyramidPunch; // Reference to the pyramid abilities script

	void Start () {
		pyramidPunch = GetComponentInParent<PyramidPunch> (); // Getting the reference
	}

	void OnTriggerStay (Collider other) {
		if (pyramidPunch.isPunching) { // If the pyramid is punching
			if (other.CompareTag ("Breakable")) { // If the player collided with a breakable wall
				other.GetComponentInChildren<Animator> ().SetTrigger ("Activate"); // Activates the animation
			} else if (other.CompareTag ("Button-Pyramid")) { // If the player collided with a button
				other.GetComponent<ButtonController> ().Activate (); // Activate the button
			}
		}
	}
}
