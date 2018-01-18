using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PyramidMovement : MonoBehaviour {

	public float moveSpeed; // The speed of the player
	public float turnSpeed; // The turn speed of the player
	public float turnSpeedLR; // The turn speed of the player
	public float jumpPower; // The amount of jump force
	public float doubleJumpPower; // The amount of double jump force
	public float slopeLimit; // The steepest slopes the player can climb

	[HideInInspector] public bool canMove; // Whether the player can move
	[HideInInspector] public Rigidbody rb; // Reference to the rigidbody

	[HideInInspector] public bool invertMovement; // Whether the movement should be inverted
	[HideInInspector] public bool turning; // Whether the player is turning
	[HideInInspector] public float targetRotation; // Where the player should be turning to (y axis)
	[HideInInspector] public bool turningLR; // Whether the player is turning
	[HideInInspector] public float targetRotationLR; // Where the player should be turning to (y axis)

	private bool isGrounded; // Whether the player is on the ground
	private bool canJump; // Whether the player can jump
	private bool canDoubleJump; // Bool for whether the player can double jump
	private Transform pyramidObject;
	private Animator anim; // Reference to the animator component
	private MovingObjectController mOC; // Temporary reference to a moving object controller script
	private FallingPlatformController fpc; // Temporary reference to a falling platform controller script
	private InputManager inputManager;  // Reference to the input manager

	private float v;
	private float h;

	void Awake () {
		// Setting starting values for bools
		canJump = false;
		canDoubleJump = false;
		canMove = false;
		turning = false;
		targetRotation = transform.rotation.eulerAngles.y; // Setting the target rotation to the player's start rotation
		pyramidObject = transform.Find ("Pyramid Object"); // Getting the reference
		turningLR = false;
		targetRotationLR = pyramidObject.rotation.eulerAngles.y; // Setting the target rotation to the player's start rotation
		rb = GetComponent<Rigidbody> (); // Getting the reference
		anim = GetComponentInChildren<Animator> (); // Getting the reference
		inputManager = GameObject.Find ("Input Manager").GetComponent<InputManager> (); // Getting the reference
	}

	// Update is called once per frame
	void FixedUpdate () {
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
		if (canMove) {
			if (canDoubleJump) { // If the player can double jump
				DoubleJump ();
			}
			if (canJump) { // If the player can jump
				Jump ();
			} 
			Move ();
		} else {
			h = 0;
			v = 0;
		}
	}

	void Move () { // Moves the pyramid
		if (inputManager.inputMB) {
			v = -1;
		} else if (inputManager.inputMF) {
			v = 1;
		} else {
			v = 0;
		}
		// Calculating horizontal movement
		if (inputManager.inputML) {
			h = -1;
			targetRotationLR = 180;
		} else if (inputManager.inputMR) {
			h = 1;
			targetRotationLR = 0;
		} else {
			h = 0;
		}
		Vector3 moveVector = transform.forward * h * moveSpeed * Time.deltaTime; // Variable for movement
		if (mOC && mOC.isActive) { // If the player has a reference to the moving object controller
			moveVector += mOC.velocity; // Adding velocity of platform so the move together
		} else if (fpc) {
			moveVector += fpc.velocity;
		}
		rb.MovePosition (rb.position + moveVector); // Applying the movement
		if (turning) { // If the player should be turning
			transform.rotation = Quaternion.Euler (transform.rotation.x, Mathf.MoveTowards (transform.rotation.eulerAngles.y, targetRotation, turnSpeed), transform.rotation.z); // Turning the player
			if (Quaternion.Angle(transform.rotation, Quaternion.Euler (Vector3.up * targetRotation)) <= 0.1f) { // If the player has reached the target rotation
				transform.rotation = Quaternion.Euler (Vector3.up * targetRotation); // Setting the rotation to equal the target rotation
				turning = false; // Setting the bool
				canMove = true; // Setting the bool
			}
		}
		pyramidObject.rotation = Quaternion.Euler (pyramidObject.rotation.x, Mathf.MoveTowards (pyramidObject.rotation.eulerAngles.y, targetRotationLR, turnSpeedLR), pyramidObject.rotation.z); // Turning the player
		if (Quaternion.Angle(pyramidObject.rotation, Quaternion.Euler (Vector3.up * targetRotationLR)) <= 0.1f) { // If the player has reached the target rotation
			pyramidObject.rotation = Quaternion.Euler (Vector3.up * targetRotationLR); // Setting the rotation to equal the target rotation
		}
	}

	void Jump () { // Makes the pyramid jump
		if (inputManager.inputJ && canJump) { // If the player hits the jump button and can jump
			rb.AddForce (Vector3.up * jumpPower * 100); // Applying jump force
			anim.SetTrigger ("Jump"); // Setting the trigger
			anim.SetBool ("Falling", false); // Setting the bool
			anim.SetBool ("Idle", false); // Setting the bool
			anim.SetBool ("Moving", false); // Setting the bool
			if (mOC) { // If the player has a reference to the moving object controller
				mOC.TestForDeactivate (); // Tests to deactivate the platform
				mOC = null; // Removing the reference
			}
			if (fpc) { // If the player has a reference to the falling platform controller
				fpc.Rise (); // Makes the platform start to rise
				fpc = null; // Removing the reference
			}
			inputManager.inputJ = false;
			isGrounded = false; // Setting the bool
			canJump = false; // Setting the bool
			canDoubleJump = true; // Setting the bool
		}
	}

	void DoubleJump () { // Makes the pyramid double jump
		if (canDoubleJump && !canJump && inputManager.inputJ) { // If the player can double jump and they press the space bar and they are off the ground
			rb.velocity = new Vector3 (rb.velocity.x, 0, rb.velocity.z);
			rb.AddForce (Vector3.up * jumpPower * 100); // Applying the double jump force
			canDoubleJump = false; // Disabling the ability to jump again until they hit the ground
			anim.SetTrigger ("Double Jump"); // Setting the trigger
			anim.SetBool ("Falling", false); // Setting the bool
			anim.SetBool ("Idle", false); // Setting the bool
			anim.SetBool ("Moving", false); // Setting the bool
		}
	}
		
	void OnCollisionEnter (Collision other) {
		if (Vector3.Angle (Vector3.up, other.contacts [0].normal) <= slopeLimit && Vector3.Angle (Vector3.up, other.contacts [0].normal) >= -slopeLimit) { // If the slope is not too steep
			isGrounded = true; // Setting the bool
			canJump = true; // Setting the bool
			canDoubleJump = false;
			anim.SetTrigger ("Land"); // Setting the trigger
		}
		if (other.collider.CompareTag ("Stick")) { // If the player collided with a moving platform they should stick to
			mOC = other.gameObject.GetComponentInParent<MovingObjectController> (); // Getting a reference to the moving object controller script
			mOC.TestForActivate (); // Testing to activate it
		} else if (other.gameObject.CompareTag ("Button-All")) { // If the player collided with a button
			other.gameObject.GetComponent<ButtonController> ().Activate (); // Activates the button
		}
		if (other.collider.name.StartsWith ("Falling Platform")) { // If the player collided with falling platform (can't use tag)
			fpc = other.collider.GetComponent<FallingPlatformController> (); // Getting a reference to the falling platform controller script
			fpc.Fall (); // Making the platform fall
		}
	}

	void OnCollisionStay (Collision other) {
		if (Vector3.Angle (Vector3.up, other.contacts [0].normal) <= slopeLimit && Vector3.Angle (Vector3.up, other.contacts [0].normal) >= -slopeLimit) { // If the slope is not too steep
			isGrounded = true; // Setting the bool
			canJump = true; // Setting the bools
			canDoubleJump = false;
		}
	}

	void OnCollisionExit (Collision other) {
		isGrounded = false; // Setting the bool
		canJump = false; // Setting the bool
		if (mOC) { // If the player has a reference to the moving object controller
			mOC.TestForDeactivate (); // Tests to deactivate the platform
			mOC = null; // Removing the reference
		}
		if (fpc) { // If the player has a reference to the falling platform controller
			fpc.Rise (); // Makes the platform start to rise
			fpc = null; // Removing the reference
		}
		StartCoroutine (WaitToDisableJump ()); // The player can still jump for a few frames after they have left the platform
	}


	void OnTriggerStay (Collider other) {
		print (targetRotation);
		if (other.CompareTag ("Turning Point")) { // If the player collided with a turn point
			if (v != 0 && !turning) {
				transform.position = other.transform.position; // Setting the position to the player to the turn point's position
				turning = true; // Setting the bool
				rb.velocity = Vector3.zero;
				if (v == 1) {
					targetRotation = transform.rotation.eulerAngles.y - 90; // Setting the target rotation
				} else if (v == -1) {
					targetRotation = transform.rotation.eulerAngles.y + 90; // Setting the target rotation
				}
				StartCoroutine (inputManager.TempDisable (0.25f));
			}
		}
	}

	IEnumerator WaitToDisableJump () { // Waiting to disable the ability to jump
		yield return new WaitForSeconds (0.25f); // Waits...
		if (!isGrounded) { // If the player is not on the ground
			canJump = false; // Setting the bool
			canDoubleJump = true; // Setting the bool
		}
	}
}
