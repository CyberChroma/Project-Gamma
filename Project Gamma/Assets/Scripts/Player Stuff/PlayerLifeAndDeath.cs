using UnityEngine;
using System.Collections;

public class PlayerLifeAndDeath : MonoBehaviour {

	public GameObject deathParticles; // Reference to the death particles
	public GameObject spawnParticles; // Reference to the death particles

	private CubeMovement cubeMovement; // Reference to the cube movement script
	private SphereMovement sphereMovement; // Reference to the sphere movement script
	private PyramidMovement pyramidMovement; // Reference to the pyramid movement script
	private CharacterChanging characterChanging; // Reference to the character changing script
	private CheckpointManager checkpointManager; // Reference to the checkpoint manager

	// Use this for initialization
	void Start () {
		characterChanging = GameObject.Find ("Camera Pivot").GetComponent<CharacterChanging> (); // Getting the reference
		checkpointManager = GameObject.Find ("Checkpoint Manager").GetComponent<CheckpointManager> (); // Getting the reference
		checkpointManager.SetSpawn (transform, transform); // Setting the spawn position and rotation to the player's start position
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

	void Die () { // Kills the player
		Instantiate (deathParticles, transform.position, transform.rotation); // Creating the death particles
		checkpointManager.Respawn (transform); // Respawns the player
		Instantiate (spawnParticles, transform.position, transform.rotation); // Creating the spawn particles
		if (gameObject == characterChanging.cube) { // If this gameobject is the cube
			cubeMovement.verticalVelocity = 0; // Zeroing vertical velocity
			cubeMovement.inertia = Vector3.zero; // Zeroing inertia
			cubeMovement.lastMove = Vector3.zero; // Zeroing last move
		} else if (gameObject == characterChanging.sphere) { // If this gameobject is the sphere
			sphereMovement.rb.velocity = Vector3.zero; // Zeroing velocity
			sphereMovement.rb.angularVelocity = Vector3.zero; // Zeroing the angular velocity
		} else { // (If this gameobject is the pyramid)
			pyramidMovement.verticalVelocity = 0; // Zeroing vertical velocity
			pyramidMovement.inertia = Vector3.zero; // Zeroing inertia
			pyramidMovement.lastMove = Vector3.zero; // Zeroing last move
		}
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Checkpoint")) { // If the player collides with a checkpoint
			checkpointManager.SetSpawn (transform, other.transform); // Setting the spawn position and rotation to the checkpoint's position and rotation
		}
	}
}