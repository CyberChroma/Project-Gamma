using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereAbilities : MonoBehaviour {

	public float bounceDelay; // The bounce delay
	public float bounceSlowTime; // The amount of time the player's speed is slowed after bouncing
	public float bounceForce; // The bounce force applied to the ball
	public float bounceAirSpeed;

	// Variables for whether certain abilities are unlocked
	public bool bounceUnlocked;

	[HideInInspector] public bool canMove; // Whether the player can move
	private bool canBounce; // Whether the player can bounce

	private bool isBouncing; // Whether the player is bouncing
	private bool isSlamming; // Whether the player is slamming

	private SphereMovement sphereMovement; // Reference to the sphere movement script
	private Rigidbody rb; // Reference to the rigidbody

	// Use this for initialization
	void Awake () {
		canMove = false; // Setting the bool
		canBounce = true; // Setting the bool
		isBouncing = false; // Setting the bool
		isSlamming = false; // Setting the bool
		sphereMovement = GetComponent<SphereMovement> (); // Getting the reference
		rb = GetComponent<Rigidbody> (); // Getting the reference
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (bounceUnlocked) { // If bounce is unlocked
			if (isSlamming && rb.velocity.y > 0f) { // If the player is slamming and their velocity becomes positive
				isSlamming = false; // Setting the bool
			}
			if (isBouncing && rb.velocity.y < 0f) { // If the player is bouncing and their velocity becomes negative
				isBouncing = false; // Setting the bool
			}

			if (canMove) { // If the player can move
				if (canBounce && Input.GetMouseButtonDown (0)) { // If the player hits the left mouse button
					if (sphereMovement.isGrounded && !isBouncing) { // If the player is on the ground and not bouncing
						Bounce ();
					} else if (!isSlamming) { // If the player is not slamming
						Slam ();
					}
				}
			}
		}
	}

	void Bounce () {
		rb.velocity /= 2; // Lowering velocity
		rb.velocity = new Vector3(rb.velocity.x, 0.1f, rb.velocity.z); // Lowering y velocity
		rb.AddForce (Vector3.up * 100 * bounceForce); // Adding the bounce force
		isBouncing = true; // Setting the bool
		sphereMovement.isGrounded = false; // Setting the bool
		StartCoroutine ("WaitToBounce");
		StartCoroutine ("SlowMovement");
	}

	void Slam () {
		rb.velocity /= 2; // Lowering velocity
		rb.velocity = new Vector3(rb.velocity.x, -0.1f, rb.velocity.z); // Lowering y velocity
		rb.AddForce (Vector3.down * 100 * bounceForce); // Adding slam force
		isSlamming = true; // Setting the bool
	}

	IEnumerator SlowMovement () { // Temporarily slows move speed after jumping
		sphereMovement.currentAirSpeed = bounceAirSpeed; // Making the player move slower
		yield return new WaitForSeconds (bounceSlowTime); // Waits...
		sphereMovement.currentAirSpeed = sphereMovement.airMoveSpeed; // Increasing move speed again
	}

	IEnumerator WaitToBounce () {
		canBounce = false; // Disables the punch ability
		yield return new WaitForSeconds (bounceDelay); // Waits for the desired amount of time
		canBounce = true; // Re-enables the player's ability to punch
	}

	void OnCollisionEnter (Collision other) {
		if (sphereMovement.enabled) { // If the sphere movement script is enabled
			if (bounceUnlocked) { // If the bounce is unlocked
				if (isSlamming) { // If the player is slaming
					if (other.collider.CompareTag ("Button-Sphere")) { // If the object is a sphere button
                        other.collider.GetComponent<ActivateFollowTarget> ().Activate (); // Activate it
					} else if (other.collider.CompareTag ("Smashable")) { // If the object is a smashable object
						other.collider.GetComponentInParent<Animator> ().SetTrigger ("Activate"); // Activate it
					}
					RaycastHit hit; // Used to get information from a raycast
					if (Physics.Raycast (transform.position, Vector3.down, out hit, 0.5f)) { // Shooting a ray down to see it the player is close enough to the ground
						isSlamming = false; // Setting the bool
						Bounce ();
					}
				}
			}
		}
	}
}
