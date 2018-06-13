using UnityEngine;
using System.Collections;

public class PyramidPunch : MonoBehaviour {

	public float punchTime;
	public float punchDelay; // Variable for the time between punches
	public float airUpForce;

	// Bools for whether the player can perform certain actions at certain times
	private bool canPunch;

	// Bools for whether the player has unlocked certain abilities
	public bool punchUnlocked;

	// Bools for whether the player is performing certain actions at certain times
	[HideInInspector] public bool isPunching; 

	private Rigidbody rb;
	private Animator anim; // Reference to the animator component
	private PlayerGroundCheck playerGroundCheck;
	private InputManager inputManager;  // Reference to the input manager

	// Use this for initialization
	void Awake () {
		// Setting starting values for bools
		canPunch = true; // Setting the bool
		rb = GetComponent<Rigidbody> (); // Getting the reference
		anim = GetComponentInChildren<Animator> (); // Getting the reference
		playerGroundCheck = GetComponent<PlayerGroundCheck> (); // Getting the reference
		inputManager = GameObject.Find ("Input Manager").GetComponent<InputManager> (); // Getting the reference
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (punchUnlocked) { // If the player has unlocked the punch ability
			if (canPunch && inputManager.inputMA) { // If the player can punch and has pressed the ability button
				Punch ();
			}
		}
	}

	void Punch () { // Makes the player do a punch attack
		anim.ResetTrigger ("Idle"); // Resetting the trigger
		anim.ResetTrigger ("Moving"); // Resetting the trigger
		anim.ResetTrigger ("Jump"); // Resetting the trigger
		anim.ResetTrigger ("Land"); // Resetting the trigger
		anim.SetTrigger ("Punch"); // Playing the animation
		if (!playerGroundCheck.isGrounded) {
			if (rb.velocity.y < 0) {
				rb.velocity = new Vector3 (rb.velocity.x, 0, rb.velocity.z);
			} else {
				rb.velocity /= 2;
			}
			rb.AddForce (Vector3.up * airUpForce * 100);
		}
		StartCoroutine (WaitToPunch ()); // Delay for between punches
	}

	void OnCollisionStay (Collision other) {
		if (isPunching) { // If the cube is punching
			if (other.collider.CompareTag ("Breakable")) { // If the player collided with a breakable wall
				other.collider.GetComponentInChildren<Animator> ().SetTrigger ("Activate"); // Activates the animation
			} else if (other.collider.CompareTag ("Button-Cube")) { // If the player collided with a button
                other.collider.GetComponent<ActivateFollowTarget> ().Activate (); // Activate the button
			}
		}
	}

	IEnumerator WaitToPunch () { // Makes a delay for when the player can punch again
		canPunch = false; // Disables the punch ability
		isPunching = true;
		yield return new WaitForSeconds (punchTime); // Waits for the desired amount of time
		isPunching = false;
		yield return new WaitForSeconds (punchDelay); // Waits for the desired amount of time
		canPunch = true; // Re-enables the player's ability to punch
	}
}