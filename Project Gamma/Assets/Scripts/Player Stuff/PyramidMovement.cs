using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidMovement : MonoBehaviour {

	public float moveSpeed; // The speed of the player
	public float turnSpeed; // The turn speed of the player

	public float gravity; // The gravity applied to the player
	public float jumpPower; // The amount of jump force
	public float doubleJumpPower; // The amount of double jump force

	[HideInInspector] public bool canMove; // Whether the player can move
	[HideInInspector] public float verticalVelocity; // The current upwards/downwards force being applied to the player
	[HideInInspector] public bool invertMovement; // Whether the movement should be inverted
	[HideInInspector] public bool turning; // Whether the player is turning
	[HideInInspector] public float targetRotation; // Where the player should be turning to (y axis)
	[HideInInspector] public Vector3 inertia; // The current force of inertia being appied to the player
	[HideInInspector] public Vector3 lastMove; // How much the player was moved last frame

	private bool wasOnGround; // Whether the player was on the ground last frame
	private bool canJump; // Whether the player can jump
	private bool canDoubleJump; // Bool for whether the player can double jump
	private bool storeMovement; // Whether the force from the last frame should be applied
	private MovingObjectController mOC; // Temporary reference to a moving object controller script
	private FallingPlatformController fpc; // Temporary reference to a falling platform controller script
	private CharacterController controller; // Reference to the character controller

	// Input variables
	private float inputH;
	private bool inputJ;

	void Awake () {
		// Setting starting values for bools
		wasOnGround = false; // Setting the bool
		canJump = false; // Setting the bool
		canDoubleJump = false; // Setting the bool
		storeMovement = false; // Setting the bool
		canMove = false; // Setting the bool
		turning = false; // Setting the bool
		targetRotation = transform.rotation.eulerAngles.y; // Setting the target rotation to the player's start rotation
		controller = GetComponent<CharacterController> (); // Getting the reference
	}

	void Update () {
		if (canMove) { // If the player can move
			inputH = Input.GetAxisRaw ("Horizontal"); // Getting input from the left and right arrow keys (or a and d)
			inputJ = Input.GetKeyDown (KeyCode.Space); // Getting input from the space bar
			if (invertMovement) { // If the movement is inverted
				inputH *= -1; // Inverts the input
			}
		} else { // (If the player can't move)
			inputH = 0; // Resetting input
			inputJ = false; // Resetting input
		}
		if (transform.rotation.eulerAngles.x != 0 || transform.rotation.eulerAngles.z != 0) { // If the x or z rotation is not zero
			transform.rotation = Quaternion.Euler (Vector3.up * transform.rotation.eulerAngles.y); // Correcting rotation
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (!wasOnGround && controller.isGrounded) { // If the player is on the ground and wasn't last frame
			canJump = true; // Setting the bool
			wasOnGround = true; // Setting the bool
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
		if (canDoubleJump) { // If the player can double jump
			DoubleJump ();
		}
		if (canJump) { // If the player can jump
			Jump ();
		} 
		Move ();
		if (turning) { // If the player should be turning
			transform.rotation = Quaternion.Euler (transform.rotation.x, Mathf.MoveTowards (transform.rotation.eulerAngles.y, targetRotation, turnSpeed), transform.rotation.z); // Turning the player
			if (Quaternion.Angle(transform.rotation, Quaternion.Euler (Vector3.up * targetRotation)) <= 0.1f) { // If the player has reached the target rotation
				transform.rotation = Quaternion.Euler (Vector3.up * targetRotation); // Setting the rotation to equal the target rotation
				turning = false; // Setting the bool
				canMove = true; // Setting the bool
			}
		}
	}

	void Move () { // Moves the pyramid
		if (!storeMovement) { // If movement is not being stored
			Vector3 moveVector = transform.forward * inputH * moveSpeed * Time.deltaTime; // Variable for movement
			if (!controller.isGrounded) { // If the player is not on the ground
				moveVector += inertia; // Adding inertia
				moveVector /= 2; // Slowing speed
			}
			moveVector.y = verticalVelocity * Time.deltaTime; // Adding vertical velocity
			if (mOC && mOC.isActive) { // If the player has a reference to the moving object controller
				moveVector += mOC.velocity; // Adding velocity of platform so the move together
			}
			controller.Move (moveVector); // Applying the movement
			lastMove = moveVector; // Storing the move
		} else { // (If the movement is being stored)
			if (mOC && mOC.isActive) { // If the player has a reference to the moving object controller
				lastMove += mOC.velocity; // Adding velocity of platform so the move together
			}
			lastMove.y = verticalVelocity * Time.deltaTime; // Adding vertical velocity
			controller.Move (lastMove); // Applying the movement
		}
	}

	void Jump () { // Makes the pyramid jump
		if (inputJ && canJump) { // If the player hits the jump button and can jump
			verticalVelocity = jumpPower; // Applying jump force
			inertia = lastMove; // Setting inertia
			canJump = false; // Setting the bool
			canDoubleJump = true; // Setting the bool
			wasOnGround = false; // Setting the bool
			if (mOC) { // If the player has a reference to the moving object controller
				mOC.TestForDeactivate (); // Tests to deactivate the platform
				mOC = null; // Removing the reference
			}
			if (fpc) { // If the player has a reference to the falling platform controller
				fpc.Rise (); // Makes the platform start to rise
				fpc = null; // Removing the reference
			}
		}
	}

	void DoubleJump () { // Makes the pyramid double jump
		if (canDoubleJump && !canJump && inputJ) { // If the player can double jump and they press the space bar and they are off the ground
			verticalVelocity = doubleJumpPower; // Applying the double jump force
			inertia = lastMove; // Setting inertia
			canDoubleJump = false; // Disabling the ability to jump again until they hit the ground
		}
	}

	IEnumerator WaitToDisableJump () { // Waiting to disable the ability to jump
		yield return new WaitForSeconds (0.25f); // Waits...
		if (!controller.isGrounded) { // If the player is not on the ground
			canJump = false; // Setting the bool
			canDoubleJump = true; // Setting the bool
		}
	}

	void OnControllerColliderHit (ControllerColliderHit hit) {
		inertia = Vector3.zero; // Getting rid of inertia
		storeMovement = false; // Setting the bool
		if (hit.collider.CompareTag ("Stick")) { // If the player collided with a moving platform they should stick to
			mOC = hit.gameObject.GetComponentInParent<MovingObjectController> (); // Getting a reference to the moving object controller script
			mOC.TestForActivate (); // Testing to activate it
		} else if (hit.gameObject.CompareTag ("Button-All")) { // If the player collided with a button
			hit.gameObject.GetComponent<ButtonController> ().Activate (); // Activates it
		}
		if (hit.collider.name.StartsWith ("Falling Platform")) { // If the player collided with falling platform (can't use tag)
			fpc = hit.collider.GetComponent<FallingPlatformController> (); // Getting a reference to the falling platform controller script
			fpc.Fall (); // Making the platform fall
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.CompareTag ("Turning Point")) { // If the player collided with a turn point
			transform.position = other.transform.position; // Setting the position to the player to the turn point's position
			turning = true; // Setting the bool
			canMove = false; // Setting the bool
			targetRotation = transform.rotation.eulerAngles.y - 90; // Setting the target rotation
			inertia = Vector3.zero; // Zeroing inertia
		}
	}

	IEnumerator TempDisable (float delay) { // Temporarily desables the player's movement
		canMove = false; // Setting the bool
		yield return new WaitForSeconds (delay); // Waits...
		canMove = true; // Setting the bool
	}
}
