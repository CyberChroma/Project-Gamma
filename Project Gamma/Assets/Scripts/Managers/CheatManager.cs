using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MonoBehaviour {
	
	public bool checkpointCheat;

	private CharacterChanging characterChanging;
	private CameraController cameraController;
	private CheckpointManager checkpointManager;
	private PlayerLifeAndDeath playerLifeAndDeath;

	// Use this for initialization
	void Start () {
		characterChanging = GameObject.Find ("Camera Pivot").GetComponent<CharacterChanging> ();
		cameraController = GameObject.Find ("Camera Pivot").GetComponent<CameraController> ();
		checkpointManager = GameObject.Find ("Checkpoint Manager").GetComponent<CheckpointManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (checkpointCheat) {
			CheckpointCheat ();
		}
	}

	void CheckpointCheat () { // Cheat to teleport the players between checkpoints
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			if (cameraController.activePlayer == characterChanging.cube) {
				checkpointManager.cubeActiveCheckpoint++;
				if (checkpointManager.cubeActiveCheckpoint >= checkpointManager.checkpoints.Length) {
					checkpointManager.cubeActiveCheckpoint = 0;
				}
				checkpointManager.cubeSpawnPos = checkpointManager.checkpoints [checkpointManager.cubeActiveCheckpoint].transform.position;
				checkpointManager.cubeSpawnRot = checkpointManager.checkpoints [checkpointManager.cubeActiveCheckpoint].transform.rotation;
			} else if (cameraController.activePlayer == characterChanging.sphere) {
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
			cameraController.activePlayer.GetComponent<PlayerLifeAndDeath> ().Die ();
		} else if (Input.GetKeyDown (KeyCode.Alpha1)) {
			if (cameraController.activePlayer == characterChanging.cube) {
				checkpointManager.cubeActiveCheckpoint--;
				if (checkpointManager.cubeActiveCheckpoint < 0) {
					checkpointManager.cubeActiveCheckpoint = checkpointManager.checkpoints.Length - 1;
				}
				checkpointManager.cubeSpawnPos = checkpointManager.checkpoints [checkpointManager.cubeActiveCheckpoint].transform.position;
				checkpointManager.cubeSpawnRot = checkpointManager.checkpoints [checkpointManager.cubeActiveCheckpoint].transform.rotation;
			} else if (cameraController.activePlayer == characterChanging.sphere) {
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
			cameraController.activePlayer.GetComponent<PlayerLifeAndDeath> ().Die ();
		}
	}
}
