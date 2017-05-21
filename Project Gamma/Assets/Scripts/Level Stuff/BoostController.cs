using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostController : MonoBehaviour {

	public float speed;
	public float disableMoveTime;
	public bool activated = true;
	public Material arrowsUnactiveMat;
	public Material arrowsActiveMat;
	private MeshRenderer arrowsMat;
	private Rigidbody sphereRb;
	private SphereMovement sphereMovement;

	// Use this for initialization
	void Start () {
		sphereRb = GameObject.Find ("Players/Sphere Character").GetComponent<Rigidbody> ();
		sphereMovement = sphereRb.GetComponent<SphereMovement> ();
		arrowsMat = transform.Find ("Arrows").GetComponent<MeshRenderer> ();
		if (activated) {
			arrowsMat.material = arrowsActiveMat;
		} else {
			arrowsMat.material = arrowsUnactiveMat;
		}
	}

	public void Activate () {
		arrowsMat.material = arrowsActiveMat;
		activated = true;
	}

	void OnTriggerEnter (Collider other) {
		if (other.name == "Sphere Character" && activated) {
			other.transform.position = new Vector3 (transform.position.x, other.transform.position.y, transform.position.z);
			sphereRb.angularVelocity = Vector3.zero;
			sphereRb.velocity = speed * transform.forward;
			StartCoroutine (DisableMovement ());
		}
	}
		
	IEnumerator DisableMovement () {
		sphereMovement.enabled = false;
		yield return new WaitForSeconds (disableMoveTime);
		sphereMovement.enabled = true;
	}
}
