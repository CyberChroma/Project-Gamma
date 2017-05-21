using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceController : MonoBehaviour {

	public float bounceForce = 10;
	
	void OnCollisionEnter (Collision other) {
		if (other.collider.GetComponent<Rigidbody> ()) {
			other.collider.GetComponent<Rigidbody> ().AddForce (Vector3.up * bounceForce * 10);
		}
	}
}