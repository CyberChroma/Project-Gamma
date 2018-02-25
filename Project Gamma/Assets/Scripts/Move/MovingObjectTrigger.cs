using UnityEngine;
using System.Collections;

public class MovingObjectTrigger : MonoBehaviour {

	public GameObject objectToMove; // Refernce to the object to move

	private MovingObjectController mOC; // Reference to the moving object controller script

	// Use this for initialization
	void Start () {
		mOC = objectToMove.GetComponent<MovingObjectController> (); // Getting the reference
	}

	void OnTriggerEnter (Collider other) {
		if (other.CompareTag ("Player")) { // If the collided object is the player
			mOC.isActive = true; // Activating the object's movement
			Destroy (gameObject); // Destroys the gameobject
		}
	}
}
