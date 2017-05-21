using UnityEngine;
using System.Collections;

public class ProjectileMover : MonoBehaviour {

	public float moveSpeed; // The speed of the object
	public float lifetime;

	private Rigidbody rb; // Reference to the rigidbody component

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> (); // Getting the reference
	}

	void OnEnable () {
		StartCoroutine ("Disable");
	}

	void OnDisable () {
		StopCoroutine ("Disable");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		rb.position = rb.position + moveSpeed * Vector3.forward * Time.deltaTime; // Moving the object to the object
	}

	IEnumerator Disable () {
		yield return new WaitForSeconds (lifetime);
		gameObject.SetActive (false);
	}

	void OnTriggerEnter (Collider other) {
		if (!other.CompareTag ("Player")) {
			gameObject.SetActive (false);
		}
	}
}