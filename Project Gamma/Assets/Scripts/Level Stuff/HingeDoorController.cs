using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeDoorController : MonoBehaviour {

	public float turnSpeed = 2; // How fast the door turns
	public float turnAmount = 90; // How much the door turns

	private bool turning; // Whether the door is turning
	private float targetRotation; // Where the door should be turning to (y axis)

	// Use this for initialization
	void Start () {
		turning = false; // Setting the bool
		targetRotation = transform.rotation.eulerAngles.y; // Setting the target location the the door's rotation (y axis)
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (turning) { // If the door should be turning
			transform.rotation = Quaternion.Euler (transform.rotation.x, Mathf.MoveTowards (transform.rotation.eulerAngles.y, targetRotation, turnSpeed), transform.rotation.z); // Rotates the door
			if (Quaternion.Angle(transform.rotation, Quaternion.Euler (Vector3.up * targetRotation)) <= 0.1f) { // If the door has reached the target rotation
				transform.rotation = Quaternion.Euler (Vector3.up * targetRotation); // Setting the rotation to exactly the target rotation
				turning = false; // Setting the bool
			}
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.CompareTag ("Pyramid Shot") && !turning) { // If the door has collided with the pyramid shot and is not turning
			turning = true; // Setting the bool
			targetRotation -= turnAmount; // Setting the target rotation
		}
	}
}
