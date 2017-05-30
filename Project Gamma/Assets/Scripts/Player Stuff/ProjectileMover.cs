using UnityEngine;
using System.Collections;

public class ProjectileMover : MonoBehaviour {

	public float moveSpeed; // The speed of the object
	public float lifetime; // The lifetime of the object
	public GameObject spawnExplosion;
	public GameObject deathExplosion;

	private Rigidbody rb; // Reference to the rigidbody component

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> (); // Getting the reference
	}

	void OnEnable () {
		Instantiate (spawnExplosion, transform.position, transform.rotation);
		StartCoroutine ("Disable");
	}

	void OnDisable () {
		StopCoroutine ("Disable");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		rb.position = rb.position + moveSpeed * transform.forward * Time.deltaTime; // Moving the object
	}

	IEnumerator Disable () { // Deactivating it after a certain number of seconds
		yield return new WaitForSeconds (lifetime);
		Instantiate (deathExplosion, transform.position, transform.rotation);
		Destroy (gameObject); // Destroying the gameobject
	}

	void OnTriggerEnter (Collider other) {
		if (!other.CompareTag ("Player") && !other.isTrigger) { // Of the projectile hit something that is not the player
			if (other.CompareTag ("Button-Pyramid")) { // If the projectile collided with a pyramid button
				other.GetComponent<ButtonController> ().Activate (); // Activate the button
			}
			Instantiate (deathExplosion, transform.position, transform.rotation);
			Destroy (gameObject); // Destroying the gameobject
		}
	}
}