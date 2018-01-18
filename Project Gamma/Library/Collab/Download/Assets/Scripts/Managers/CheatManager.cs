using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MonoBehaviour {
	
	public bool checkpointCheat; // Whether the checkpoint cheat is active

	private CharacterChanging characterChanging; // Reference to the character changing script
	private CameraController cameraController; // Reference to the camera controller script
	private CheckpointManager checkpointManager; // Reference to the checkpoint manager sctipt
	private PlayerLifeAndDeath playerLifeAndDeath; // References to the player life and death scripts

	// Use this for initialization
	void Start () {
		characterChanging = GameObject.Find ("Camera Pivot").GetComponent<CharacterChanging> (); // Getting the reference
		cameraController = GameObject.Find ("Camera Pivot").GetComponent<CameraController> (); // Getting the reference
		checkpointManager = GameObject.Find ("Checkpoint Manager").GetComponent<CheckpointManager> (); // Getting the reference
	}
	
	// Update is called once per frame
	void Update () {
		if (checkpointCheat) { // If the cheat is active
			CheckpointCheat ();
		}
	}

	void CheckpointCheat () { // Cheat to teleport the players between checkpoints using the 1 and 2 buttons
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			if (cameraController.target == characterChanging.cube) {
				checkpointManager.cubeActiveCheckpoint++;
				if (checkpointManager.cubeActiveCheckpoint >= checkpointManager.checkpoints.Length) {
					checkpointManager.cubeActiveCheckpoint = 0;
				}
				checkpointManager.cubeSpawnPos = checkpointManager.checkpoints [checkpointManager.cubeActiveCheckpoint].transform.position;
				checkpointManager.cubeSpawnRot = checkpointManager.checkpoints [checkpointManager.cubeActiveCheckpoint].transform.rotation;
			} else if (cameraController.target == characterChanging.sphere) {
				checkpointManager.sphereActiveCheckpoint++;
				if (checkpointManager.sphereActiveCheckpoint >= checkpointManager.checkpoints.Length) {
					checkpointManager.sphereActiveCheckpoint = 0;
				}
				checkpointManager.sphereSpawnPos = checkpointManager.checkpoints [checkpointManager.sphereActiveCheckpoint].transform.position;
				checkpointManager.sphereSpawnRot = checkpointManager.checkpoints [checkpointManager.sphereActiveCheckpoint].transform.rotation;
			} else {
				checkpointManager.pyramidActiveCheckpoint++;
				if (checkpointManager.pyramidActiveCheckpoint >= checkpointManager.checkpoints.Length) {
					checkpointManager.pyramidActiveCheckpoint = 0;
				}
				checkpointManager.pyramidSpawnPos = checkpointManager.checkpoints [checkpointManager.pyramidActiveCheckpoint].transform.position;
				checkpointManager.pyramidSpawnRot = checkpointManager.checkpoints [checkpointManager.pyramidActiveCheckpoint].transform.rotation;
			}
			cameraController.target.GetComponent<PlayerLifeAndDeath> ().Die ();
		} else if (Input.GetKeyDown (KeyCode.Alpha1)) {
			if (cameraController.target == characterChanging.cube) {
				checkpointManager.cubeActiveCheckpoint--;
				if (checkpointManager.cubeActiveCheckpoint < 0) {
					checkpointManager.cubeActiveCheckpoint = checkpointManager.checkpoints.Length - 1;
				}
				checkpointManager.cubeSpawnPos = checkpointManager.checkpoints [checkpointManager.cubeActiveCheckpoint].transform.position;
				checkpointManager.cubeSpawnRot = checkpointManager.checkpoints [checkpointManager.cubeActiveCheckpoint].transform.rotation;
			} else if (cameraController.target == characterChanging.sphere) {
				checkpointManager.sphereActiveCheckpoint--;
				if (checkpointManager.sphereActiveCheckpoint < 0) {
					checkpointManager.sphereActiveCheckpoint = checkpointManager.checkpoints.Length - 1;
				}
				checkpointManager.sphereSpawnPos = checkpointManager.checkpoints [checkpointManager.sphereActiveCheckpoint].transform.position;
				checkpointManager.sphereSpawnRot = checkpointManager.checkpoints [checkpointManager.sphereActiveCheckpoint].transform.rotation;
			} else {
				checkpointManager.pyramidActiveCheckpoint--;
				if (checkpointManager.pyramidActiveCheckpoint < 0) {
					checkpointManager.pyramidActiveCheckpoint = checkpointManager.checkpoints.Length - 1;
				}
				checkpointManager.pyramidSpawnPos = checkpointManager.checkpoints [checkpointManager.pyramidActiveCheckpoint].transform.position;
				checkpointManager.pyramidSpawnRot = checkpointManager.checkpoints [checkpointManager.pyramidActiveCheckpoint].transform.rotation;
			}
			cameraController.target.GetComponent<PlayerLifeAndDeath> ().Die ();
		}
	}
}
