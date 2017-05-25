using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformController : MonoBehaviour {

	public float fallSpeed; // The speed the platform falls
	public float riseSpeed; // The speed the platform rises
	public float riseDelay; // The delay time between falling and rising

	private bool falling; // Whether the platform is currently falling
	private float startHeight; // The start height of the platform
	private Rigidbody rb; // Reference to the rigidbody

	// Use this for initialization
	void Start () {
		startHeight = transform.position.y; // Getting the start height
		rb = GetComponent<Rigidbody> (); // Getting the reference to the rigidbody
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (falling) { // If the platform is falling
			rb.velocity = Vector3.down * fallSpeed; // Moving the platform by changing its velocity
		} else { // (If the platform is rising)
			if (transform.position.y >= startHeight) { // If the platform has reached its start height
				rb.velocity = Vector3.zero; // Changes the velocity of the platform to zero
			} else { // (If the platform should still be rising)
				rb.velocity = Vector3.up * riseSpeed; // Moving the platform by changing its velocity
			}
		}
	}

	public void Fall () {
		StopCoroutine ("DelayToRise");
		falling = true; // Setting the bool
	}

	public void Rise () {
		StartCoroutine ("DelayToRise");
	}

	IEnumerator DelayToRise () { // Creates a delay before the platform starts rising again
		yield return new WaitForSeconds (riseDelay); // Waits...
		falling = false; // Setting the bool
	}
}
