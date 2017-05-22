using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidMovement : MonoBehaviour {

	public float moveSpeed; // The speed of the player
	public float turnSpeed;

	public float gravity;
	public float jumpPower;
	public float doubleJumpPower; // Variable for the speed of the double jump

	[HideInInspector] public bool canMove;
	[HideInInspector] public float verticalVelocity;
	[HideInInspector] public Vector3 inertia;
	[HideInInspector] public bool invertMovement;
	[HideInInspector] public bool turning;
	[HideInInspector] public float targetRotation;

	private bool wasOnGround;
	private bool canJump;
	private bool canDoubleJump; // Bool for whether the player can double jump
	private bool storeMovement;
	private Vector3 lastMove;
	private MovingObjectController mOC;
	private FallingPlatformController fpc;
	private CharacterController controller; // Reference to the rigidbody component

	private float inputH;
	private bool inputJ;

	void Awake () {
		// Setting starting values for bools
		wasOnGround = false;
		canJump = false;
		canDoubleJump = false;
		storeMovement = false;
		canMove = false;
		turning = false;
		controller = GetComponent<CharacterController> (); // Getting the reference
	}

	void Update () {
		if (canMove) {
			inputH = Input.GetAxisRaw ("Horizontal"); // Getting input from the left and right arrow keys (or a and d)
			inputJ = Input.GetKeyDown (KeyCode.Space);
			if (invertMovement) {
				inputH *= -1;
			}
			if (inputH != 0 && storeMovement) {
				storeMovement = false;
			}
		} else {
			inputH = 0;
			inputJ = false;
		}
		if (transform.rotation.eulerAngles.x != 0 || transform.rotation.eulerAngles.z != 0) {
			transform.rotation = Quaternion.Euler (Vector3.up * transform.rotation.eulerAngles.y);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (!wasOnGround && controller.isGrounded) {
			canJump = true;
			wasOnGround = true;
		} else if (wasOnGround && !controller.isGrounded) {
			if (mOC) {
				mOC.TestForDeactivate ();
				mOC = null;
			}
			if (fpc) {
				fpc.Rise ();
				fpc = null;
			}
			verticalVelocity = 0;
			inertia = lastMove / 2;
			StartCoroutine (WaitToDisableJump ());
			wasOnGround = false;
		} else if (!controller.isGrounded) {
			verticalVelocity -= gravity * Time.deltaTime;
			verticalVelocity = Mathf.Clamp (verticalVelocity, -gravity, gravity);
		}
		if (canDoubleJump) {
			DoubleJump ();
		}
		if (canJump) {
			Jump ();
		} 
		Move ();
		if (turning) {
			transform.rotation = Quaternion.Euler (transform.rotation.x, Mathf.MoveTowards (transform.rotation.eulerAngles.y, targetRotation, turnSpeed), transform.rotation.z);
			if (Quaternion.Angle(transform.rotation, Quaternion.Euler (Vector3.up * targetRotation)) <= 0.1f) {
				transform.rotation = Quaternion.Euler (Vector3.up * targetRotation);
				turning = false;
				canMove = true;
			}
		}
	}

	void Move () { // Moves the cube
		if (!storeMovement) {
			Vector3 moveVector;
			moveVector = transform.forward * inputH * moveSpeed * Time.deltaTime; // Variable for movement
			if (!controller.isGrounded) {
				moveVector += inertia;
				moveVector /= 2;
			}
			moveVector.y = verticalVelocity * Time.deltaTime;
			if (mOC) {
				moveVector += mOC.velocity;
			}
			controller.Move (moveVector); // Applying the movement
			lastMove = moveVector;
		} else {
			if (mOC && mOC.isActive) {
				lastMove += mOC.velocity;
			}
			lastMove.y = verticalVelocity * Time.deltaTime;
			controller.Move (lastMove);
		}
	}

	void Jump () {
		if (inputJ && canJump) {
			verticalVelocity = jumpPower;
			inertia = lastMove;
			canJump = false;
			canDoubleJump = true;
			wasOnGround = false;
			if (mOC) {
				mOC.TestForDeactivate ();
				mOC = null;
			}
			if (fpc) {
				fpc.Rise ();
				fpc = null;
			}
		}
	}

	void DoubleJump () { // Makes the player double jump
		if (canDoubleJump && !canJump && inputJ) { // If the player can double jump and they press the space bar and they are off the ground
			verticalVelocity = doubleJumpPower;
			inertia = lastMove;
			canDoubleJump = false; // Disabling the ability to jump again until they hit the ground
		}
	}

	IEnumerator WaitToDisableJump () {
		yield return new WaitForSeconds (0.25f);
		if (!controller.isGrounded) {
			canJump = false;
			canDoubleJump = true;
		}
	}

	void OnControllerColliderHit (ControllerColliderHit hit) {
		inertia = Vector3.zero;
		storeMovement = false;
		if (hit.collider.CompareTag ("Stick")) {
			mOC = hit.gameObject.GetComponentInParent<MovingObjectController> ();
			mOC.TestForActivate ();
		} else {
			inertia = Vector3.zero;
			storeMovement = false;
		}
		if (hit.collider.name.StartsWith ("Falling Platform")) {
			fpc = hit.collider.GetComponent<FallingPlatformController> ();
			fpc.Fall ();
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.CompareTag ("Turning Point")) {
			transform.position = other.transform.position;
			turning = true;
			canMove = false;
			targetRotation -= 90;
			inertia = Vector3.zero;
		}
	}

	IEnumerator TempDisable (float delay) {
		canMove = false;
		yield return new WaitForSeconds (delay);
		canMove = true;
	}
}
