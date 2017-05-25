using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceController : MonoBehaviour {

	public float bounceForce = 20; // The bounce force of the platform
	
	void OnCollisionEnter (Collision other) {
		if (other.collider.GetComponent<Rigidbody> ()) { // If the collided object has a rigidbody (either sphere or enemy)
			other.collider.GetComponent<Rigidbody> ().AddForce (Vector3.up * bounceForce * 10); // Adds the desired force to the object to bounce the object up
		}
	}
}