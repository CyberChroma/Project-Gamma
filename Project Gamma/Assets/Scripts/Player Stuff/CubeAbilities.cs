using UnityEngine;
using System.Collections;

public class CubeAbilities : MonoBehaviour {

	public float punchDelay; // Variable for the time between punches

	// Bools for whether the player has unlocked certain abilities
	public bool punchUnlocked;

	// Bools for whether the player can perform certain actions at certain times
	[HideInInspector] public bool canMove;
	private bool canPunch;

	// Bools for whether the player is performing certain actions at certain times
	[HideInInspector] public bool isPunching; 

	private Animator anim; // Reference to the animator component
	private AnimatorStateInfo stateInfo; // Reference to the current animation state
	private AnimatorStateInfo nextStateInfo; // Reference to the next animation state


	// Use this for initialization
	void Awake () {
		// Setting starting values for bools
		canMove = false; // Setting the bool
		canPunch = true; // Setting the bool
		anim = GetComponentInChildren<Animator> (); // Getting the reference
	}

	// Update is called once per frame
	void FixedUpdate () {
		stateInfo = anim.GetCurrentAnimatorStateInfo (0); // Getting the information
		nextStateInfo = anim.GetNextAnimatorStateInfo (0); // Getting the information
		if (canMove) {
			if (punchUnlocked) { // If the player has unlocked the punch ability
				if (canPunch && Input.GetMouseButtonDown (0)) { // If the player can punch and has pressed the left mouse button
					Punch ();
				}
			}
			if (nextStateInfo.IsName ("Punch") || stateInfo.IsName ("Punch")) { // If the player is punching or is about to punch
				isPunching = true; // Setting the bool
			} else { // (If the player is not punching)
				isPunching = false; // Setting the bool
			}
		}
	}

	void Punch () { // Makes the player do a punch attack
		anim.ResetTrigger ("Idle"); // Resetting the trigger
		anim.ResetTrigger ("Moving"); // Resetting the trigger
		anim.ResetTrigger ("Jump"); // Resetting the trigger
		anim.ResetTrigger ("Land"); // Resetting the trigger
		anim.SetTrigger ("Punch"); // Playing the animation
		StartCoroutine (WaitToPunch ()); // Delay for between punches
	}

	IEnumerator WaitToPunch () { // Makes a delay for when the player can punch again
		canPunch = false; // Disables the punch ability
		yield return new WaitForSeconds (punchDelay); // Waits for the desired amount of time
		canPunch = true; // Re-enables the player's ability to punch
	}
}