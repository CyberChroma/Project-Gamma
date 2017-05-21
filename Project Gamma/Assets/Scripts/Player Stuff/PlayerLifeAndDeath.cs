using UnityEngine;
using System.Collections;

public class PlayerLifeAndDeath : MonoBehaviour {

	public GameObject deathParticles; // Reference to the death particles
	public GameObject spawnParticles; // Reference to the death particles
	public CharacterChanging characterChanging;

	private Vector3 spawnPos; // The respawn position of the player
	private Quaternion spawnRot; // The respawn rotation of the player
	private CubeMovement cubeMovement;
	private SphereMovement sphereMovement;

	// Use this for initialization
	void Start () {
		spawnPos = transform.position; // The initial position of the player becomes the respawn position
		spawnRot = transform.rotation; // The initial rotation of the player becomes the respawn rotation
		if (gameObject == characterChanging.cube) {
			cubeMovement = GetComponent<CubeMovement> ();
		} else if (gameObject == characterChanging.sphere) {
			sphereMovement = GetComponent<SphereMovement> ();
		}
	}

	void Update () {
		if (transform.position.y < -30) {
			Die ();
		}
	}

	void Die () {// Kills the player
		Instantiate (deathParticles, transform.position, transform.rotation);
		transform.position = spawnPos; // Moves the player to its spawn position
		transform.rotation = spawnRot; // Rotates the player to its spawn rotation
		Instantiate (spawnParticles, transform.position, transform.rotation);
		if (gameObject == characterChanging.cube) {
			cubeMovement.verticalVelocity = 0;
			cubeMovement.inertia = Vector3.zero;
		} else if (gameObject == characterChanging.sphere) {
			sphereMovement.rb.velocity = Vector3.zero;
		}
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Checkpoint")) { // If the player collides with a checkpoint
			spawnPos = other.transform.position; // The checkpoint's position becomes the respawn position
			spawnRot = other.transform.rotation; // The checkpoint's rotation becomes the respawn rotation
		}
	}
}