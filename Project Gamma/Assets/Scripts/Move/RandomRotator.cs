using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator : MonoBehaviour {

	public float maxSpeed; // The maximum speed the object will rotate at

	private Rigidbody rb; // Reference to the rigidbody

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> (); // Getting the force
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		rb.AddTorque (new Vector3 (Random.Range (-maxSpeed, maxSpeed), Random.Range (-maxSpeed, maxSpeed), Random.Range (-maxSpeed, maxSpeed))); // Randomly rotates the object
	}
}
