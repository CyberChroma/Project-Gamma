using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByForce : MonoBehaviour {

	public float force = 30; // The force being applied
	public Vector3 dir = Vector3.zero; // Direction of force
	public bool relative = false; // Whether the object moves on the global or local axis

	private Vector3 lookDir;
	private Rigidbody rb; // Reference to rigidbody

	// Use this for initialization
	void Awake () {
		lookDir = transform.forward;
		rb = GetComponent<Rigidbody> (); // Getting the reference
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (relative) {
			rb.AddRelativeForce (dir.normalized * force); // Applying the force
		} else {
			rb.AddForce (dir.normalized * force); // Applying the force
		}
		if (dir != Vector3.zero) {
			lookDir = dir;
		}
		rb.rotation = Quaternion.Slerp (rb.rotation, Quaternion.LookRotation (lookDir), 0.1f); // Rotating the player
	}
}