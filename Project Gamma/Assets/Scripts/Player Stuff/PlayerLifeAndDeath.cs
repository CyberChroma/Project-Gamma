using UnityEngine;
using System.Collections;

public class PlayerLifeAndDeath : MonoBehaviour {

	public GameObject deathParticles; // Reference to the death particles
	public GameObject spawnParticles; // Reference to the death particles
	public CharacterChanging characterChanging; // Reference to the character changing script

	private Vector3 spawnPos; // The respawn position of the player
	private Quaternion spawnRot; // The respawn rotation of the player
	private CubeMovement cubeMovement; // Reference to the cube movement script
	private SphereMovement sphereMovement; // Reference to the sphere movement script
	private PyramidMovement pyramidMovement; // Reference to the pyramid movement script

	// Use this for initialization
	void Start () {
		spawnPos = transform.position; // The initial position of the player becomes the respawn position
		spawnRot = transform.rotation; // The initial rotation of the player becomes the respawn rotation
		if (gameObject == characterChanging.cube) { // If this gameobject is the cube
			cubeMovement = GetComponent<CubeMovement> (); // Getting reference
		} else if (gameObject == characterChanging.sphere) { // If this gameobject is the sphere
			sphereMovement = GetComponent<SphereMovement> (); // Getting reference
		} else { // (If this gameobject is the pyramid)
			pyramidMovement = GetComponent<PyramidMovement> (); // Getting reference
		}
	}

	// Update is called once per frame
	void Update () {
		if (transform.position.y < -30) { // Basically if the player falls off the map
			Die ();
		}
	}

	void Die () {// Kills the player
		Instantiate (deathParticles, transform.position, transform.rotation);
		transform.position = spawnPos; // Moves the player to its spawn position
		transform.rotation = spawnRot; // Rotates the player to its spawn rotation
		Instantiate (spawnParticles, transform.position, transform.rotation); // Creating the spawn particles
		if (gameObject == characterChanging.cube) { // If this gameobject is the cube
			cubeMovement.verticalVelocity = 0; // Zeroing vertical velocity
			cubeMovement.inertia = Vector3.zero; // Zeroing inertia
			cubeMovement.lastMove = Vector3.zero; // Zeroing last move
		} else if (gameObject == characterChanging.sphere) { // If this gameobject is the sphere
			sphereMovement.rb.velocity = Vector3.zero; // Zeroing velocity
			sphereMovement.rb.angularVelocity = Vector3.zero;
		} else { // (If this gameobject is the pyramid)
			pyramidMovement.verticalVelocity = 0; // Zeroing vertical velocity
			pyramidMovement.inertia = Vector3.zero; // Zeroing inertia
			pyramidMovement.lastMove = Vector3.zero; // Zeroing last move
		}
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Checkpoint")) { // If the player collides with a checkpoint
			spawnPos = other.transform.position; // The checkpoint's position becomes the respawn position
			spawnRot = other.transform.rotation; // The checkpoint's rotation becomes the respawn rotation
		}
	}
}