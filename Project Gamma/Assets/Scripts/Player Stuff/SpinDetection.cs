using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinDetection : MonoBehaviour {
	
    private PlayerSpin playerSpin; // Reference to the pyramid abilities script

	void Start () {
        playerSpin = GetComponentInParent<PlayerSpin> (); // Getting the reference
	}

	void OnTriggerStay (Collider other) {
        if (playerSpin.isSpinning) { // If the pyramid is punching
			if (other.CompareTag ("Breakable")) { // If the player collided with a breakable wall
				other.GetComponentInChildren<Animator> ().SetTrigger ("Activate"); // Activates the animation
			} else if (other.CompareTag ("Button-Wall")) { // If the player collided with a button
                other.GetComponent<ActivateFollowTarget> ().Activate (); // Activate the button
			}
		}
	}
}
