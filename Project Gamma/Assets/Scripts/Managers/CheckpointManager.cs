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

	[HideInInspector] public int cubeActiveCheckpoint;
	[HideInInspector] public int sphereActiveCheckpoint;
	[HideInInspector] public int pyramidActiveCheckpoint;

	[HideInInspector] public Vector3 cubeSpawnPos; // The spawn position of the cube
	[HideInInspector] public Quaternion cubeSpawnRot; // The spawn rotation of the cube
	[HideInInspector] public Vector3 sphereSpawnPos; // The spawn position of the sphere
	[HideInInspector] public Quaternion sphereSpawnRot; // The spawn rotation of the sphere
	[HideInInspector] public Vector3 pyramidSpawnPos; // The spawn position of the pyramid
	[HideInInspector] public Quaternion pyramidSpawnRot; // The spawn rotation of the pyramid

	void Start () {
		cubeActiveCheckpoint = -1;
		sphereActiveCheckpoint = -1;
		pyramidActiveCheckpoint = -1;
	}

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

	public void SetSpawn (Transform character, Transform checkpoint) { // Setting the spawn of the player to the checkpoint it touched and deactivating the other checkpoints by changing their material
		for (int i = 0; i < checkpoints.Length; i++) {
			if (checkpoints [i].name == checkpoint.name) {
				if (character.name == "Cube Character") {
					cubeSpawnPos = checkpoint.position;
					cubeSpawnRot = checkpoint.rotation;
					cubeActiveCheckpoint = i;
					checkpoint.Find ("Cube_Active_State").GetComponent<MeshRenderer> ().material = cubeActiveMat;
				} else if (character.name == "Sphere Character") {
					sphereSpawnPos = checkpoint.position;
					sphereSpawnRot = checkpoint.rotation;
					sphereActiveCheckpoint = i;
					checkpoint.Find ("Sphere_Active_State").GetComponent<MeshRenderer> ().material = sphereActiveMat;
				} else {
					pyramidSpawnPos = checkpoint.position;
					pyramidSpawnRot = checkpoint.rotation;
					pyramidActiveCheckpoint = i;
					checkpoint.Find ("Pyramid_Active_State").GetComponent<MeshRenderer> ().material = pyramidActiveMat;
				}
			} else {
				if (character.name == "Cube Character") {
					checkpoints [i].transform.Find("Cube_Active_State").GetComponent<MeshRenderer> ().material = cubeUnactiveMat;
				} else if (character.name == "Sphere Character") {
					checkpoints [i].transform.Find("Sphere_Active_State").GetComponent<MeshRenderer> ().material = sphereUnactiveMat;
				} else {
					checkpoints [i].transform.Find("Pyramid_Active_State").GetComponent<MeshRenderer> ().material = pyramidUnactiveMat;
				}
			}
		}
	}
}
