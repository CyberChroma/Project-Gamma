using UnityEngine;
using System.Collections;

public class SphereMovement : MonoBehaviour {

	public float moveSpeed; // The move speed of the player
	public float airMoveSpeed; // The move speed of the sphere while in the air
	public float jumpPower;
	public float slopeLimit; // The steepest slopes the sphere can climb
	public float velocityDecreaseRate; // How fast the player slows down
	public Transform camPivot; // Reference to the camera pivot's transform

	[HideInInspector] public bool canMove; // Whether the player can move
	[HideInInspector] public float currentAirSpeed; // The current speed of the player
	[HideInInspector] public bool isGrounded; // Whether the player is on the ground
	[HideInInspector] public Rigidbody rb; // Reference to the rigidbody component
	private bool canJump;
	private InputManager inputManager;
	private float v = 0;
	private float h = 0;

	void Awake () {
		canMove = false; // Setting the bool
		isGrounded = false; // Setting the bool
		currentAirSpeed = airMoveSpeed; // Setting the air move speed
		rb = GetComponent<Rigidbody> (); // Getting the reference
		inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (canMove) {
			Move ();
			Jump ();
		}
	}

	void Move () { // Moves the sphere
		// Calculating vertical movement
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
		} else if (inputManager.inputMR) {
			h = 1;
		} else {
			h = 0;
		}
		Vector3 force = Vector3.ProjectOnPlane (camPivot.right, Vector3.up).normalized * h + Vector3.ProjectOnPlane (camPivot.forward, Vector3.up).normalized * v; // Variable for force
		if (isGrounded) {
			force *= moveSpeed; // Calculating force
		} else {
			force *= currentAirSpeed; // Calculating force
		}
		rb.AddForce (force); // Applying the force
	}

	void Jump () {
		if (inputManager.inputJ && canJump) { // If the player presses the space button
			rb.AddForce (Vector3.up * jumpPower * 100); // Applying jump force
			canJump = false;
		}
	}

	void OnCollisionEnter (Collision other) {
		if (Vector3.Angle (Vector3.up, other.contacts [0].normal) <= slopeLimit && Vector3.Angle (Vector3.up, other.contacts [0].normal) >= -slopeLimit) { // If the slope is not too steep
			isGrounded = true; // Setting the bool
			canJump = true; // Setting the bool
		}
		if (other.gameObject.CompareTag ("Button-All")) { // If then player has hit a button
            other.gameObject.GetComponent<ActivateFollowTarget> ().Activate (); // Activates it
		}
	}

	void OnCollisionStay (Collision other) {
		if (Vector3.Angle (Vector3.up, other.contacts [0].normal) <= slopeLimit && Vector3.Angle (Vector3.up, other.contacts [0].normal) >= -slopeLimit) { // If the slope is not too steep
			isGrounded = true; // Setting the bool
			canJump = true; // Setting the bool
		}
	}

	void OnCollisionExit (Collision other) {
		isGrounded = false; // Setting the bool
		canJump = false; // Setting the bool
		StartCoroutine (WaitToDisableJump ()); // The player can still jump for a few frames after they have left the platform
	}

	IEnumerator WaitToDisableJump () { // Waiting to disable the ability to jump
		yield return new WaitForSeconds (0.25f); // Waits...
		if (!isGrounded) { // If the player is not on the ground
			canJump = false; // Setting the bool
		}
	}
}