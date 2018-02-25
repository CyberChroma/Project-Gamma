using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSlam : MonoBehaviour {

	public float slamForce; // The slam force applied to the player
	public float airTime;

	public bool slamUnlocked;

	private bool canSlam; // Whether the player can bounce
	private bool isSlamming; // Whether the player is slamming

	private PlayerMove playerMove;
	private PlayerGroundCheck playerGroundCheck;
	private Rigidbody rb; // Reference to the rigidbody
	private InputManager inputManager;

	// Use this for initialization
	void Awake () {
		canSlam = true; // Setting the bool
		isSlamming = false; // Setting the bool
		playerMove = GetComponent<PlayerMove> ();
		playerGroundCheck = GetComponent<PlayerGroundCheck> ();
		rb = GetComponent<Rigidbody> (); // Getting the reference
		inputManager = GameObject.Find ("Input Manager").GetComponent<InputManager> (); // Getting the reference
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (slamUnlocked) { // If bounce is unlocked
			if (isSlamming && (playerGroundCheck.isGrounded || rb.velocity.y > 0)) { // If the player is slamming and their velocity becomes positive
				isSlamming = false; // Setting the bool
			}
			if (canSlam && inputManager.inputA2 && !playerGroundCheck.isGrounded) { // If the player hits the left mouse button
				StartCoroutine (WaitToSlam ());
				StartCoroutine (Slam ());
			}
		}
	}

	IEnumerator WaitToSlam () {
		StartCoroutine (playerMove.TempStopMove (airTime + 0.1f));
		StartCoroutine (inputManager.TempDisable (airTime + 0.1f));
		yield return new WaitForSeconds (airTime);
		isSlamming = true;
	}

	IEnumerator Slam () {
		while (!isSlamming) {
			rb.velocity = Vector3.zero;
			yield return null;
		}
		rb.velocity /= 2; // Lowering velocity
		rb.velocity = new Vector3(rb.velocity.x, -0.1f, rb.velocity.z); // Lowering y velocity
		rb.AddForce (Vector3.down * 100 * slamForce); // Adding slam force
	}
}
