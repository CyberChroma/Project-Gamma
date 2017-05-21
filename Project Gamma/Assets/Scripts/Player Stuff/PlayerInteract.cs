using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {

	void OnControllerColliderHit (ControllerColliderHit other) {
		if (other.gameObject.CompareTag ("Button-All")) {
			other.gameObject.GetComponent<ButtonController> ().Activate ();
		}
	}

	void OnCollisionEnter (Collision other) {
		if (other.gameObject.CompareTag ("Button-All")) {
			other.gameObject.GetComponent<ButtonController> ().Activate ();
		}
	}
}
