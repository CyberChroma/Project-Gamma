using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour {

	public float slopeLimit; // The steepest slopes the cube can climb
	[HideInInspector] public bool isGrounded;

	private InputManager inputManager;  // Reference to the input manager
	private Animator anim;

	// Use this for initialization
	void Awake () {
		inputManager = GameObject.Find ("Input Manager").GetComponent<InputManager> (); // Getting the reference
		anim = GetComponentInChildren<Animator> (); // Getting the reference
	}
	
	// Update is called once per frame
	void Update () {
		if (isGrounded) { // If the player is on the ground
			if (inputManager.inputMF || inputManager.inputML || inputManager.inputMR || inputManager.inputMB) { // If the player is not moving
				anim.SetBool ("Idle", true); // Setting the bool
				anim.SetBool ("Falling", false); // Setting the bool
				anim.SetBool ("Moving", false); // Setting the bool
			} else { // If the player is moving
				anim.SetBool ("Moving", true); // Setting the bool
				anim.SetBool ("Idle", false); // Setting the bool
				anim.SetBool ("Falling", false); // Setting the bool
			}
		} else { // If the player is in the air
			anim.SetBool ("Falling", true); // Setting the bool
			anim.SetBool ("Idle", false); // Setting the bool
			anim.SetBool ("Moving", false); // Setting the bool
		}
	}

	void OnCollisionEnter (Collision other) {
		foreach (ContactPoint point in other.contacts) {
			if (Vector3.Angle (Vector3.up, point.normal) <= slopeLimit && Vector3.Angle (Vector3.up, point.normal) >= -slopeLimit) { // If the slope is not too steep
				isGrounded = true; // Setting the bool
				anim.SetTrigger ("Land"); // Setting the trigger
			}
		}
	}

	void OnCollisionStay (Collision other) {
		foreach (ContactPoint point in other.contacts) {
			if (Vector3.Angle (Vector3.up, point.normal) <= slopeLimit && Vector3.Angle (Vector3.up, point.normal) >= -slopeLimit && !isGrounded) { // If the slope is not too steep
				isGrounded = true; // Setting the bool
			}
		}
	}

	void OnCollisionExit (Collision other) {
		isGrounded = false; // Setting the bool
	}
}
