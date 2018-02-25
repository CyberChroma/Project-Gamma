using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceController : MonoBehaviour {

	public float bounceForce = 30; // The bounce force of the platform
    public float upForce = 10;
	void OnCollisionEnter (Collision other) {
		if (other.collider.GetComponent<Rigidbody> ()) { // If the collided object has a rigidbody (either sphere or enemy)
            other.collider.GetComponent<Rigidbody> ().AddForce (-other.contacts[0].normal * bounceForce * 10); // Adds the desired force to the object to bounce the object away
            other.collider.GetComponent<Rigidbody> ().AddForce (Vector3.up * upForce * 10); // Adds the desired force to the object to bounce the object up

        }
	}
}