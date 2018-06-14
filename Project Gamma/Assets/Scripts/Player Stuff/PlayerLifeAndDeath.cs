using UnityEngine;
using System.Collections;

public class PlayerLifeAndDeath : MonoBehaviour {

	public GameObject deathParticles; // Reference to the death particles
	public GameObject spawnParticles; // Reference to the death particles

	private PlayerMove playerMove; // Reference to the cube movement script
	private Health health;
	private CheckpointManager checkpointManager; // Reference to the checkpoint manager

	// Use this for initialization
	void Start () {
		playerMove = GetComponent <PlayerMove> ();
		health = GetComponent<Health> ();
        checkpointManager = FindObjectOfType<CheckpointManager>(); // Getting the reference
	}

	// Update is called once per frame
	void Update () {
		if (transform.position.y < -30 || health.currentHealth <= 0) { // If the player falls out of the map or have run out of health
			Die ();
		}
	}

	public void Die () { // Kills the player
		Instantiate (deathParticles, transform.position, transform.rotation); // Creating the death particles
		playerMove.rb.velocity = Vector3.zero;
		playerMove.rb.angularVelocity = Vector3.zero;
		checkpointManager.Respawn (transform); // Respawns the player
		health.currentHealth = health.startHealth;
		health.healthChanged = true;
		Instantiate (spawnParticles, transform.position, transform.rotation); // Creating the spawn particles
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Checkpoint")) { // If the player collides with a checkpoint
			checkpointManager.SetSpawn (transform, other.transform); // Setting the spawn position and rotation to the checkpoint's position and rotation
		} else if (other.gameObject.CompareTag ("Kill")) {
			Die ();
		}
	}
}