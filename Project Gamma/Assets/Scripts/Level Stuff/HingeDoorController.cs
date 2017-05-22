using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeDoorController : MonoBehaviour {

	public float turnSpeed = 1;
	public float turnAmount = 90;

	private bool turning;
	private float targetRotation;

	// Use this for initialization
	void Start () {
		turning = false;
		targetRotation = transform.rotation.eulerAngles.y;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (turning) {
			transform.rotation = Quaternion.Euler (transform.rotation.x, Mathf.MoveTowards (transform.rotation.eulerAngles.y, targetRotation, turnSpeed), transform.rotation.z);
			if (Quaternion.Angle(transform.rotation, Quaternion.Euler (Vector3.up * targetRotation)) <= 0.1f) {
				transform.rotation = Quaternion.Euler (Vector3.up * targetRotation);
				turning = false;
			}
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.CompareTag ("Pyramid Shot") && !turning) {
			turning = true;
			targetRotation -= turnAmount;
		}
	}
}
