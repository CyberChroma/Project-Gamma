using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour {

	public Material activeMat; // The active material of the checkpoint

	private MeshRenderer meshRenderer; // Reference to the mesh renderer (and material) of the checkpoint

	// Use this for initialization
	void Start () {
		meshRenderer = GetComponent<MeshRenderer> (); // Getting the reference
	}

	void OnTriggerEnter (Collider other) {
		if (other.CompareTag ("Player")) { // If the collided object is a player
			meshRenderer.material = activeMat; // Change the material to the active one
		}
	}
}
