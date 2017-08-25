using UnityEngine;
using System.Collections;

public class CubeMovement : MonoBehaviour {

	public float moveSpeed; // The speed of the player
	public float rotSmoothing; // The amount of rotational smoothing
	public float jumpPower; // The amount of jump force
	public float slopeLimit; // The steepest slopes the cube can climb
	public float wallJumpPowerU; // The amount of vertical force in a wall jump
	public float wallJumpPowerF; // The amount of horizontal force in a wall jump

	[HideInInspector] public bool canMove; // Whether the player can move
	[HideInInspector] public Rigidbody rb; // Reference to the rigidbody

	private bool isGrounded; // Whether the player is on the ground
	private bool canJump; // If the player can jump
	private Vector3 lookDir; // the direction the player should be facing
	private Animator anim; // Reference to the animator component
	private AnimatorStateInfo stateInfo; // Reference to the current animation state
	private MovingObjectController mOC; // Temporary reference to a moving object controller script
	private FallingPlatformController fpc; // Temporary reference to a falling platform controller script
	private Transform camPivot; // Reference to the camera pivot

	// Input variables
	private float inputV;
	private float inputH;
	private bool inputJ;

	void Awake () {
		canJump = false; // Setting the bool
		canMove = false; // Setting the bool
		lookDir = transform.forward; // Setting the default value
		rb = GetComponent<Rigidbody> (); // Getting the reference
		anim = GetComponentInChildren<Animator> (); // Getting the reference
		camPivot = GameObject.Find ("Camera Pivot").transform; // Getting the reference
	}

	void Update () {
		if (canMove) { // If the player can move
			inputV = Input.GetAxis ("Vertical"); // Getting input from the up and down arrow keys (or w and s)
			inputH = Input.GetAxis ("Horizontal"); // Getting input from the left and right arrow keys (or a and d)
			inputJ = Input.GetKeyDown (KeyCode.Space); // Getting input from the space bar
		} else { // (If the player cannot move)
			inputH = 0; // Removing input
			inputV = 0; // Removing input
			inputJ = false; // Removing input
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (isGrounded) { // If the player is on the ground
			if (inputV == 0 && inputH == 0) { // If the player is not moving
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
		Move ();

	}

	void Move () { // Moves the cube
		Vector3 moveVector = Vector3.ProjectOnPlane(camPivot.right, Vector3.up).normalized * inputH + Vector3.ProjectOnPlane(camPivot.forward, Vector3.up).normalized * inputV; // Variable for force
		if (moveVector.magnitude > 1) {
			moveVector.Normalize ();
		}
		if (moveVector != Vector3.zero) {
			lookDir = moveVector;
		}
		moveVector *= moveSpeed * Time.deltaTime; // Calculating force
		if (mOC && mOC.isActive) { // If the player has a reference to a moving object controller
			moveVector += mOC.velocity; // Adding velocity of platform so the move together
		} else if (fpc) {
			moveVector += fpc.velocity;
		}
		rb.MovePosition (rb.position + moveVector); // Applying the movement
		rb.rotation = Quaternion.Slerp (rb.rotation, Quaternion.LookRotation (lookDir), rotSmoothing); // Rotating the player
		if (mOC && mOC.isActive) { // If the player has a reference to the moving object controller
			rb.rotation *= mOC.rotVelocity; // Adds rotational velocity to make the player rotate with the platform
		}
	}

	void Jump () { // Makes the cube jump
		if (inputJ && canJump) { // If the player hits the jump button and can jump
			rb.AddForce (Vector3.up * jumpPower * 100); // Applying jump force
			anim.SetTrigger ("Jump"); // Setting the trigger
			anim.SetBool ("Falling", false); // Setting the bool
			anim.SetBool ("Idle", false); // Setting the bool
			anim.SetBool ("Moving", false); // Setting the bool
			if (mOC) { // If the player has a reference to the moving object controller
				mOC.TestForDeactivate (); // Tests to deactivate the platform
				mOC = null; // Removing the reference
			}
			isGrounded = false; // Setting the bool
			canJump = false; // Setting the bool
		}
	}

	void OnCollisionEnter (Collision other) {
		if (Vector3.Angle (Vector3.up, other.contacts [0].normal) <= slopeLimit && Vector3.Angle (Vector3.up, other.contacts [0].normal) >= -slopeLimit) { // If the slope is not too steep
			isGrounded = true; // Setting the bool
			canJump = true; // Setting the bool
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
		}
		if (inputJ && !canJump && !isGrounded) { // If the player presses the space bar and are not on the ground
			WallJump (other.contacts [0].normal);
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

	IEnumerator WaitToDisableJump () { // Waiting to disable the ability to jump
		yield return new WaitForSeconds (0.25f); // Waits...
		if (!isGrounded) { // If the player is not on the ground
			canJump = false; // Setting the bool
		}
	}

	void WallJump (Vector3 normal) {
		lookDir = Vector3.ProjectOnPlane(normal, Vector3.up); // Turning the player to face away from the wall
		rb.velocity = Vector3.zero; // Resetting the velocity
		rb.AddForce (Vector3.ProjectOnPlane(normal, Vector3.up) * wallJumpPowerF * 100 + Vector3.up * wallJumpPowerU * 100); // Pushing the player up
		anim.SetTrigger ("Jump"); // Setting the trigger
		StartCoroutine (TempDisable (0.25f));
	}

	IEnumerator TempDisable (float delay) { // Temporarily disables the player's movement
		canMove = false; // Setting the bool
		yield return new WaitForSeconds (delay); // Waits...
		if (!(Input.GetAxis ("Vertical") == -1 || Input.GetAxis ("Vertical") == 1 || Input.GetAxis ("Horizontal") == -1 || Input.GetAxis ("Horizontal") == 1)) { // If the player is not holding a button
			Input.ResetInputAxes (); // Resets input
		}
		canMove = true; // Setting the bool
	}
}