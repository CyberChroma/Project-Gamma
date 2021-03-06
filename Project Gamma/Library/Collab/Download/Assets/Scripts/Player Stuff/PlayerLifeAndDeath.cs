﻿using UnityEngine;
using System.Collections;

public class PlayerLifeAndDeath : MonoBehaviour {

	public GameObject deathParticles; // Reference to the death particles
	public GameObject spawnParticles; // Reference to the death particles

	private PlayerMove playerMove; // Reference to the cube movement script
	//private CharacterChanging characterChanging; // Reference to the character changing script
	private CheckpointManager checkpointManager; // Reference to the checkpoint manager

	// Use this for initialization
	void Start () {
		//characterChanging = GameObject.Find ("Camera Pivot").GetComponent<CharacterChanging> (); // Getting the reference
		checkpointManager = GameObject.Find ("Checkpoint Manager").GetComponent<CheckpointManager> (); // Getting the reference
		//if (gameObject == characterChanging.cube) { // If this gameobject is the cube
			checkpointManager.cubeSpawnPos = transform.position; // Setting spawn position to start position
			checkpointManager.cubeSpawnRot = transform.rotation; // Setting spawn rotation to start rotation
		/*} else if (gameObject == characterChanging.sphere) { // If this gameobject is the sphere
			checkpointManager.sphereSpawnPos = transform.position; // Setting spawn position to start position
			checkpointManager.sphereSpawnRot = transform.rotation; // Setting spawn rotation to start rotation
		} else { // (If this gameobject is the pyramid)
			checkpointManager.pyramidSpawnPos = transform.position; // Setting spawn position to start position
			checkpointManager.pyramidSpawnRot = transform.rotation; // Setting spawn rotation to start rotation
		}*/
		playerMove = GetComponent <PlayerMove> ();
	}

	// Update is called once per frame
	void Update () {
		if (transform.position.y < -30) { // Basically if the player falls off the map
			Die ();
		}
	}

	public void Die () { // Kills the player
		Instantiate (deathParticles, transform.position, transform.rotation); // Creating the death particles
		playerMove.rb.velocity = Vector3.zero;
		checkpointManager.Respawn (transform); // Respawns the player
		Instantiate (spawnParticles, transform.position, transform.rotation); // Creating the spawn particles
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Checkpoint")) { // If the player collides with a checkpoint
			checkpointManager.SetSpawn (transform, other.transform); // Setting the spawn position and rotation to the checkpoint's position and rotation
		}
	}
}