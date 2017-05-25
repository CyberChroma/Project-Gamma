using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostController : MonoBehaviour {

	public float speed; // The speed of the sphere after touching the boost
	public float disableMoveTime; // How long the player's controls are disabled for
	public bool activated = true; // Whether the boost starts activated
	public Material arrowsUnactiveMat; // The unactive material of the arrows
	public Material arrowsActiveMat; // The active material of the arrows

	private MeshRenderer arrowsMat; // Reference to the mesh renderer (and current material) of the arrows
	private Rigidbody sphereRb; // Reference to the sphere's rigidbody
	private SphereMovement sphereMovement; // Reference to the sphere's movement script

	// Use this for initialization
	void Start () {
		sphereRb = GameObject.Find ("Players/Sphere Character").GetComponent<Rigidbody> (); // Getting the reference to the sphere's rigidbody
		sphereMovement = sphereRb.GetComponent<SphereMovement> (); // Getting the reference to the sphere's movement script
		arrowsMat = transform.Find ("Arrows").GetComponent<MeshRenderer> (); // Getting the reference to the mesh renderer of the arrows
		if (activated) { // If the boost starts activated
			arrowsMat.material = arrowsActiveMat; // Setting the material
		} else { // (If the boost starts decactivated
			arrowsMat.material = arrowsUnactiveMat; // Setting the material
		}
	}

	public void Activate () { // Called by a button to activate the boost
		arrowsMat.material = arrowsActiveMat; // Changing the material to the active one
		activated = true; // Setting the boost active
	}

	void OnTriggerEnter (Collider other) {
		if (other.name == "Sphere Character" && activated) { // If the sphere character has hit the boost and the boost is activated
			other.transform.position = new Vector3 (transform.position.x, other.transform.position.y, transform.position.z); // Moves the sphere to be exactly on the boost (other than y position)
			sphereRb.angularVelocity = Vector3.zero; // Setting the angular velocity of the sphere to zero
			sphereRb.velocity = speed * transform.forward; // Setting the velocity of the sphere to the disired direction the speed
			StartCoroutine (DisableMovement ()); // Temporarily disables sphere movement
		}
	}
		
	IEnumerator DisableMovement () { // Temporarily disables sphere movement
		sphereMovement.enabled = false; // Turns off the script
		yield return new WaitForSeconds (disableMoveTime); // Waits...
		sphereMovement.enabled = true; // Turns on the script
	}
}
