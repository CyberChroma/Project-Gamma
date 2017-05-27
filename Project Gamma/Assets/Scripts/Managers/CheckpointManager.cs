using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {

	public GameObject[] checkpoints;

	public Material cubeUnactiveMat; // The active material of the checkpoint
	public Material sphereUnactiveMat; // The active material of the checkpoint
	public Material pyramidUnactiveMat; // The active material of the checkpoint

	public Material cubeActiveMat; // The active material of the checkpoint
	public Material sphereActiveMat; // The active material of the checkpoint
	public Material pyramidActiveMat; // The active material of the checkpoint

	private Vector3 cubeSpawnPos; // The spawn position of the cube
	private Quaternion cubeSpawnRot; // The spawn rotation of the cube
	private Vector3 sphereSpawnPos; // The spawn position of the sphere
	private Quaternion sphereSpawnRot; // The spawn rotation of the sphere
	private Vector3 pyramidSpawnPos; // The spawn position of the pyramid
	private Quaternion pyramidSpawnRot; // The spawn rotation of the pyramid

	public void Respawn (Transform character) { // Places the player back to the last checkpoint it touched
		if (character.name == "Cube Character") {
			character.position = cubeSpawnPos;
			character.rotation = cubeSpawnRot;
		} else if (character.name == "Sphere Character") {
			character.position = sphereSpawnPos;
			character.rotation = sphereSpawnRot;
		} else {
			character.position = pyramidSpawnPos;
			character.rotation = pyramidSpawnRot;
		}
	}

	public void SetSpawn (Transform character, Transform checkpoint) { // Setting the spawn of the player to the checkpoint it touched
		if (character.name == "Cube Character") {
			cubeSpawnPos = checkpoint.position;
			cubeSpawnRot = checkpoint.rotation;
		} else if (character.name == "Sphere Character") {
			sphereSpawnPos = checkpoint.position;
			sphereSpawnRot = checkpoint.rotation;
		} else {
			pyramidSpawnPos = checkpoint.position;
			pyramidSpawnRot = checkpoint.rotation;
		}
	}

	public void DeactivateOthers (string name, string character) { // Changing the materials of other checkpoints to 'deactivate' them
		foreach (GameObject checkpoint in checkpoints) {
			if (checkpoint.name != name) {
				MeshRenderer meshRenderer;
				if (character == "Cube Character") {
					meshRenderer = checkpoint.transform.Find("Cube_Active_State").GetComponent<MeshRenderer> ();
					meshRenderer.material = cubeUnactiveMat;
				} else if (character == "Sphere Character") {
					meshRenderer = checkpoint.transform.Find("Sphere_Active_State").GetComponent<MeshRenderer> ();
					meshRenderer.material = sphereUnactiveMat;
				} else {
					meshRenderer = checkpoint.transform.Find("Pyramid_Active_State").GetComponent<MeshRenderer> ();
					meshRenderer.material = pyramidUnactiveMat;
				}
			}
		}
	}
}
