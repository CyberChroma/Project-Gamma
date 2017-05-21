using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereAbilities : MonoBehaviour {

	public float bounceDelay;
	public float bounceSlowTime;
	public float bounceForce;

	public bool bounceUnlocked;

	[HideInInspector] public bool canMove;
	private bool canBounce;

	private bool isBouncing;
	private bool isSlamming;

	private SphereMovement sphereMovement;
	private Rigidbody rb;

	// Use this for initialization
	void Awake () {
		canMove = false;
		canBounce = true;
		isBouncing = false;
		isSlamming = false;
		sphereMovement = GetComponent<SphereMovement> ();
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (bounceUnlocked) {
			if (isSlamming && rb.velocity.y > 0f) {
				isSlamming = false;
			}
			if (isBouncing && rb.velocity.y < 0f) {
				isBouncing = false;
			}

			if (canMove) {
				if (canBounce && Input.GetMouseButtonDown (0)) {
					if (sphereMovement.isGrounded && !isBouncing) {
						Bounce ();
					} else if (!isSlamming) {
						Slam ();
					}
				}
			}
		}
	}

	void Bounce () {
		rb.velocity *= 0.5f;
		rb.velocity = new Vector3(rb.velocity.x, 0.1f, rb.velocity.z);
		rb.AddForce (Vector3.up * 100 * bounceForce);
		isBouncing = true;
		sphereMovement.isGrounded = false;
		StartCoroutine ("WaitToBounce");
		StartCoroutine ("SlowMovement");
	}

	void Slam () {
		rb.velocity *= 0.5f;
		rb.velocity = new Vector3(rb.velocity.x, -0.1f, rb.velocity.z);
		rb.AddForce (Vector3.down * 100 * bounceForce);
		isSlamming = true;
	}

	IEnumerator SlowMovement () {
		sphereMovement.airMoveSpeed = 2;
		yield return new WaitForSeconds (bounceSlowTime);
		sphereMovement.airMoveSpeed = 4;
	}

	IEnumerator WaitToBounce () {
		canBounce = false; // Disables the punch ability
		yield return new WaitForSeconds (bounceDelay); // Waits for the desired amount of time
		canBounce = true; // Re-enables the player's ability to punch
	}

	void OnCollisionEnter (Collision other) {
		if (sphereMovement.enabled) {
			if (bounceUnlocked) {
				if (isSlamming) {
					if (other.collider.CompareTag ("Button-Sphere")) {
						other.collider.GetComponent<ButtonController> ().Activate ();
					} else if (other.collider.CompareTag ("Smashable")) {
						other.collider.GetComponentInParent<Animator> ().SetTrigger ("Activate");
					}
					RaycastHit hit;
					if (Physics.Raycast (transform.position, Vector3.down, out hit, 0.5f)) {
						isSlamming = false;
						Bounce ();
					}
				}
			}
		}
	}
}
