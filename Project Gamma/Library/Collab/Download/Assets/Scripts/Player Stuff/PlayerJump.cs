using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour {

	public float jumpPower; // The amount of jump force
	public float fallMultiplier = 2f;
	public float lowJumpMultiplier = 1.5f;

	[HideInInspector] public bool canJump; // If the player can jump
	[HideInInspector] public Rigidbody rb; // Reference to the rigidbody

	private Animator anim; // Reference to the animator component
	private InputManager inputManager;  // Reference to the input manager
	private MovingObjectController moc; // Temporary reference to a moving object controller script
	private FallingPlatformController fpc; // Temporary reference to a falling platform controller script
	private PlayerGroundCheck playerGroundCheck;

	// Use this for initialization
	void Awake () {
		rb = GetComponent<Rigidbody> (); // Getting the reference
		anim = GetComponentInChildren<Animator> (); // Getting the reference
		inputManager = GameObject.Find ("Input Manager").GetComponent<InputManager> (); // Getting the reference
		playerGroundCheck = GetComponent<PlayerGroundCheck> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (playerGroundCheck.isGrounded) { // If the player is on the ground
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
		if (canJump) { // If the player can jump
			Jump ();
		}
		if (rb.velocity.y < 0) {
			rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		} else if (rb.velocity.y > 0 && !inputManager.inputJ) {
			rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}
	}

	void Jump () { // Makes the cube jump
		if (inputManager.inputJD && canJump) { // If the player hits the jump button and can jump
			rb.AddForce (Vector3.up * jumpPower * 100); // Applying jump force
			anim.SetTrigger ("Jump"); // Setting the trigger
			anim.SetBool ("Falling", false); // Setting the bool
			anim.SetBool ("Idle", false); // Setting the bool
			anim.SetBool ("Moving", false); // Setting the bool
			if (moc) { // If the player has a reference to the moving object controller
				moc.TestForDeactivate (); // Tests to deactivate the platform
				moc = null; // Removing the reference
			}
			inputManager.inputJ = false; // Setting the bool
			playerGroundCheck.isGrounded = false; // Setting the bool
			canJump = false; // Setting the bool
		}
	}

	void OnCollisionEnter (Collision other) {
		if (Vector3.Angle (Vector3.up, other.contacts [0].normal) <= playerGroundCheck.slopeLimit && Vector3.Angle (Vector3.up, other.contacts [0].normal) >= -playerGroundCheck.slopeLimit) { // If the slope is not too steep
			playerGroundCheck.isGrounded = true; // Setting the bool
			canJump = true; // Setting the bool
			anim.SetTrigger ("Land"); // Setting the trigger
		}
	}

	void OnCollisionStay (Collision other) {
		if (Vector3.Angle (Vector3.up, other.contacts [0].normal) <= playerGroundCheck.slopeLimit && Vector3.Angle (Vector3.up, other.contacts [0].normal) >= -playerGroundCheck.slopeLimit) { // If the slope is not too steep
			playerGroundCheck.isGrounded = true; // Setting the bool
			canJump = true; // Setting the bools
		}
	}

	void OnCollisionExit (Collision other) {
		playerGroundCheck.isGrounded = false; // Setting the bool
		canJump = false; // Setting the bool
		StartCoroutine (WaitToDisableJump ()); // The player can still jump for a few frames after they have left the platform
	}

	IEnumerator WaitToDisableJump () { // Waiting to disable the ability to jump
		yield return new WaitForSeconds (0.25f);
		if (!playerGroundCheck.isGrounded) { // If the player is not on the ground
			canJump = false; // Setting the bool
		}
	}

}
