using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeWallJump : MonoBehaviour {

	public float wallJumpPowerU; // The amount of vertical force in a wall jump
	public float wallJumpPowerF; // The amount of horizontal force in a wall jump
	public float minWallHeight;

	[HideInInspector] public Rigidbody rb; // Reference to the rigidbody

	private bool canWallJump;
	private PlayerMove playerMove;
	private PlayerJump playerJump;
	private Animator anim; // Reference to the animator component
	private InputManager inputManager;  // Reference to the input manager

	// Use this for initialization
	void Awake () {
		playerMove = GetComponent<PlayerMove> ();
		playerJump = GetComponent<PlayerJump> ();
		rb = GetComponent<Rigidbody> (); // Getting the reference
		anim = GetComponentInChildren<Animator> (); // Getting the reference
		inputManager = GameObject.Find ("Input Manager").GetComponent<InputManager> (); // Getting the reference
	}

	void OnEnable () {
		canWallJump = true;
	}

	void OnDisable () {
		canWallJump = false;
	}

	void OnCollisionStay (Collision other) {
		if (canWallJump) {
			if (Vector3.Angle (Vector3.up, other.contacts [0].normal) <= 100 && Vector3.Angle (Vector3.up, other.contacts [0].normal) >= 80 && !playerJump.canJump) { // If the player presses the space bar and are not on the ground
				if (rb.velocity.y < 0) { 
					rb.velocity *= 0.9f;
				}
				if (playerMove.moveVector != Vector3.zero && inputManager.inputJD && !Physics.Raycast (transform.position, Vector3.down, minWallHeight)) {
					WallJump (other.contacts [0].normal);
				}
			}
		}
	}

	void WallJump (Vector3 normal) {
		inputManager.inputJ = false;
		rb.velocity = Vector3.zero; // Resetting the velocity
		rb.AddForce (Vector3.ProjectOnPlane(Vector3.Reflect (playerMove.moveVector.normalized, normal), Vector3.up) * wallJumpPowerF * 100 + Vector3.up * wallJumpPowerU * 100); // Pushing the player up and away from the wall
		playerMove.lookDir = Vector3.ProjectOnPlane(normal, Vector3.up); // Turning the player to face away from the wall
		StartCoroutine (StopTurn ());
		StartCoroutine (StopWallJump ());
		anim.SetTrigger ("Jump"); // Setting the trigger
		StartCoroutine (playerMove.TempStopMove (0.25f));
	}

	IEnumerator StopWallJump () {
		canWallJump = false;
		yield return new WaitForSeconds (0.1f);
		canWallJump = true;
	}

	IEnumerator StopTurn () {
		playerMove.canTurn = false;
		yield return new WaitForSeconds (0.5f);
		playerMove.canTurn = true;
	}
}
