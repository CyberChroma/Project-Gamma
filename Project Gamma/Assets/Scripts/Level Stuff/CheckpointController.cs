using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour {

	private MeshRenderer cubeMeshRenderer; // Reference to the mesh renderer (and material) of the checkpoint
	private MeshRenderer sphereMeshRenderer; // Reference to the mesh renderer (and material) of the checkpoint
	private MeshRenderer pyramidMeshRenderer; // Reference to the mesh renderer (and material) of the checkpoint
	private CheckpointManager checkpointManager;

	// Use this for initialization
	void Start () {
		cubeMeshRenderer = transform.Find ("Cube_Active_State").GetComponent<MeshRenderer> (); // Getting the reference
		sphereMeshRenderer = transform.Find ("Sphere_Active_State").GetComponent<MeshRenderer> (); // Getting the reference
		pyramidMeshRenderer = transform.Find ("Pyramid_Active_State").GetComponent<MeshRenderer> (); // Getting the reference
		checkpointManager = GameObject.Find ("Checkpoint Manager").GetComponent<CheckpointManager> ();
	}

	void OnTriggerEnter (Collider other) {
		if (other.CompareTag ("Player")) { // If the collided object is a player
			if (other.name == "Cube Character") {
				cubeMeshRenderer.material = checkpointManager.cubeActiveMat; // Change the material to the active one
			} else if (other.name == "Sphere Character") {
				sphereMeshRenderer.material = checkpointManager.sphereActiveMat; // Change the material to the active one
			} else if (other.name == "Pyramid Character") {
				pyramidMeshRenderer.material = checkpointManager.pyramidActiveMat; // Change the material to the active one
			}
			checkpointManager.DeactivateOthers (gameObject.name, other.name); // Deactivates all other checkpoints for this character
		}
	}
}
