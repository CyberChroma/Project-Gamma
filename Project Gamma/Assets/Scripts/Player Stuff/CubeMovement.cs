using UnityEngine;
using System.Collections;

public class CubeMovement : MonoBehaviour {

	public float moveSpeed; // The speed of the player
	public float rotSpeed; // The rotation speed of the player
	public float gravity; // The strength of gravity
	public float jumpPower; // The amount of jump force

	public float wallJumpVSpeedUp; // The upwards force of a vertical wall jump
	public float wallJumpVSpeedSide; // The sideways force of a vertical wall jump
	public float wallJumpHSpeedForward; // The forwards force of a horizontal wall jump
	public float wallJumpHSpeedSide; // The sideways force of a horizontal wall jump
	public float wallJumpHSpeedUp; // The upwards force of a horizontal wall jump

	[HideInInspector] public bool canMove; // Whether the player can move
	[HideInInspector] public float verticalVelocity; // The current upwards/downwards force being applied to the player
	[HideInInspector] public Vector3 inertia; // The current force of inertia being appied to the player
	[HideInInspector] public Vector3 lastMove; // How much the player was moved last frame

	private bool wasOnGround; // Whether the player was on the ground last frame
	private bool canJump; // If the player can jump
	private bool storeMovement; // Whether the force from the last frame should be applied
	private CharacterController controller; // Reference to the character controller
	private Animator anim; // Reference to the animator component
	private AnimatorStateInfo stateInfo; // Reference to the current animation state
	private MovingObjectController mOC; // Temporary reference to a moving object controller script
	private FallingPlatformController fpc; // Temporary reference to a falling platform controller script


	// Input variables
	private float inputV;
	private float inputH;
	private bool inputJ;

	void Awake () {
		wasOnGround = false; // Setting the bool
		canJump = false; // Setting the bool
		storeMovement = false; // Setting the bool
		canMove = false; // Setting the bool
		controller = GetComponent<CharacterController> (); // Getting the reference
		anim = GetComponentInChildren<Animator> (); // Getting the reference
	}

	void Update () {
		if (canMove) { // If the player can move
			inputV = Input.GetAxisRaw ("Vertical"); // Getting input from the up and down arrow keys (or w and s)
			inputH = Input.GetAxisRaw ("Horizontal"); // Getting input from the left and right arrow keys (or a and d)
			inputJ = Input.GetKeyDown (KeyCode.Space); // Getting input from the space bar
			if (inputH != 0 && storeMovement) { // If the player turns and the last movement is being stored
				storeMovement = false; // Setting the bool
			}
		} else { // (If the player cannot move)
			inputH = 0; // Removing input
			inputV = 0; // Removing input
			inputJ = false; // Removing input
		}
		if (transform.rotation.eulerAngles.x != 0 || transform.rotation.eulerAngles.z != 0) { // If the player has turned on the x or z axis
			transform.rotation = Quaternion.Euler (Vector3.up * transform.rotation.eulerAngles.y); // Correcting the rotation
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		stateInfo = anim.GetCurrentAnimatorStateInfo (0); // Getting the information
		if (inputV == 0 && inputH == 0 && controller.isGrounded) { // If the player is not moving and is not on the ground
			if (!stateInfo.IsName ("Idle") && !anim.IsInTransition (0)) { // If the player is not in the idle animation
				anim.SetTrigger ("Idle"); // Setting the trigger
			}
		} else { // (If the player is moving and/or is in the air)
			if (stateInfo.IsName ("Idle") && !stateInfo.IsName ("Moving") && !anim.IsInTransition (0)) { // If the player is not in the move animation
				anim.SetTrigger ("Moving"); // Setting the trigger
			}
		}
		if (!wasOnGround && controller.isGrounded) { // If it is the first frame of the player hitting the ground
			canJump = true; // Setting the bool
			wasOnGround = true; // Setting the bool
			if (!stateInfo.IsName ("Land")) { // If the player is not in the land animation
				anim.ResetTrigger ("Idle"); // Resetting the trigger
				anim.ResetTrigger ("Moving"); // Resetting the trigger
				anim.SetTrigger ("Land"); // Setting the trigger
			}
		} else if (wasOnGround && !controller.isGrounded) { // If the player has just left the ground
			if (mOC) { // If the player has a reference to the moving object controller
				mOC.TestForDeactivate (); // Tests to deactivate the platform
				mOC = null; // Removing the reference
			}
			if (fpc) { // If the player has a reference to the falling platform controller
				fpc.Rise (); // Makes the platform start to rise
				fpc = null; // Removing the reference
			}
			verticalVelocity = 0; // Removing the vertical velocity
			inertia = lastMove / 2; // Creating inertia
			StartCoroutine (WaitToDisableJump ()); // The player can still jump for a few frames after they have left the platform
			wasOnGround = false; // Setting the bool
		} else if (!controller.isGrounded) { // If the player is not on the ground
			verticalVelocity -= gravity * Time.deltaTime; // Applying gravity
			verticalVelocity = Mathf.Clamp (verticalVelocity, -gravity, gravity); // Clamping the value
		}
		if (canJump) { // If the player can jump
			Jump ();
		} 
		Move ();
		if (mOC && mOC.isActive) { // If the player has a reference to the moving object controller
			transform.rotation *= mOC.rotVelocity; // Adds rotational velocity to make the player rotate with the platform
		}
		if (inputH != 0 && !storeMovement) { // If the player should be turning
			Turn ();
		}
	}

	void Move () { // Moves the cube
		if (!storeMovement) { // If movement is not being stored
			Vector3 moveVector = transform.forward * inputV * moveSpeed * Time.deltaTime; // Variable for movement
			if (!controller.isGrounded) { // If the player is not on the ground
				moveVector += inertia; // Adding inertia
				moveVector /= 2; // Slowing speed
			}
			moveVector.y = verticalVelocity * Time.deltaTime; // Adding vertical velocity
			if (mOC && mOC.isActive) { // If the player has a reference to a moving object controller
				moveVector += mOC.velocity; // Adding velocity of platform so the move together
			}
			controller.Move (moveVector); // Applying the movement
			lastMove = moveVector; // Storing the move
		} else { // (If the movement is being stored)
			if (mOC && mOC.isActive) { // If the player has a reference to a moving object controller
				lastMove += mOC.velocity; // Adding velocity of platform so the move together
			}
			lastMove.y = verticalVelocity * Time.deltaTime; // Adding vertical velocity
			controller.Move (lastMove); // Applying the movement
		}
	}

	void Turn () { // Turns the cube
		float turning; // Creating a variable for turning
		turning = inputH * rotSpeed * 10 * Time.deltaTime; // Setting the variable
		if (controller.isGrounded) { // If the player is on the ground
			transform.Rotate (Vector3.up * turning); // Turning the player
		} else { // (If the player is not on the ground)
			transform.Rotate (Vector3.up * turning / 2); // Turning the player
		}
	}

	void Jump () { // Makes the cube jump
		if (inputJ && canJump) { // If the player hits the jump button and can jump
			verticalVelocity = jumpPower; // Applying jump force
			inertia = lastMove; // Setting inertia
			canJump = false; // Setting the bool
			wasOnGround = false; // Setting the bool
			if (!stateInfo.IsName ("Jump")) { // If the jump animation is not playing
				anim.SetTrigger ("Jump"); // Setting the trigger
			}
			if (mOC) { // If the player has a reference to the moving object controller
				mOC.TestForDeactivate (); // Tests to deactivate the platform
				mOC = null; // Removing the reference
			}

		}
	}

	IEnumerator WaitToDisableJump () { // Waiting to disable the ability to jump
		yield return new WaitForSeconds (0.25f); // Waits...
		if (!controller.isGrounded) { // If the player is not on the ground
			canJump = false; // Setting the bool
		}
	}

	void OnControllerColliderHit (ControllerColliderHit hit) {
		if (hit.collider.CompareTag ("Wall Jump V")) { // If the player collided with a vertical wall jump
			verticalVelocity *= 0.95f; // Decreasing the vertical velocity
			if (inputJ && !controller.isGrounded) { // If the player presses the space bar and are not on the ground
				StartCoroutine (WallJumpV (hit));
			}
		} else if (hit.collider.CompareTag ("Wall Jump H")) { // If the player collided with a horizontal wall jump
			verticalVelocity *= 0.95f; // Decreasing the vertical velocity
			if (inputJ && !controller.isGrounded) { // If the player presses the space bar and are not on the ground
				StartCoroutine (WallJumpH (hit));
			}
		} else {
			inertia = Vector3.zero; // Getting rid of inertia
			storeMovement = false; // Setting the bool
			if (hit.collider.CompareTag ("Stick")) { // If the player collided with a moving platform they should stick to
				mOC = hit.gameObject.GetComponentInParent<MovingObjectController> (); // Getting a reference to the moving object controller script
				mOC.TestForActivate (); // Testing to activate it
			} else if (hit.gameObject.CompareTag ("Button-All")) { // If the player collided with a button
				hit.gameObject.GetComponent<ButtonController> ().Activate (); // Activates the button
			} else {
				inertia = Vector3.zero; // Getting rid of inertia
				storeMovement = false; // Setting the bool
			}
			if (hit.collider.name.StartsWith ("Falling Platform")) { // If the player collided with falling platform (can't use tag)
				fpc = hit.collider.GetComponent<FallingPlatformController> (); // Getting a reference to the falling platform controller script
				fpc.Fall (); // Making the platform fall
			}
		}
	}

	IEnumerator WallJumpV (ControllerColliderHit hit) {
		yield return new WaitForFixedUpdate (); // Waits...
		transform.rotation = Quaternion.LookRotation (hit.normal); // Turning the player to face away from the wall
		verticalVelocity = wallJumpVSpeedUp; // Pushing the player up
		lastMove = (hit.normal * wallJumpVSpeedSide) * Time.deltaTime; // Adding force to push the player away from the wall
		storeMovement = true; // Setting the bool
		StartCoroutine (TempDisable (0.25f));
		if (!stateInfo.IsName ("Jump")) { // If the jump animation is not playing
			anim.SetTrigger ("Jump"); // Setting the trigger
		}
	}

	IEnumerator WallJumpH (ControllerColliderHit hit) {
		yield return new WaitForFixedUpdate (); // Waits...
		transform.rotation = Quaternion.LookRotation (hit.normal, Vector3.up); // Turning the player to face away from the wall
		verticalVelocity = wallJumpHSpeedUp; // Pushing the player up
		lastMove = (hit.transform.right * wallJumpHSpeedForward + hit.normal * wallJumpHSpeedSide) * Time.deltaTime; // Adding force to push the player away from the wall and forward
		storeMovement = true; // Setting the bool
		StartCoroutine (TempDisable (0.25f));
		if (!stateInfo.IsName ("Jump")) { // If the jump animation is not playing
			anim.SetTrigger ("Jump"); // Setting the trigger
		}
	}

	IEnumerator TempDisable (float delay) { // Temporarily disables the player's movement
		canMove = false; // Setting the bool
		yield return new WaitForSeconds (delay); // Waits...
		canMove = true; // Setting the bool
	}
}