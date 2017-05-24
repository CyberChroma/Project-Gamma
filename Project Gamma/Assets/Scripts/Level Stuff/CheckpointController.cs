using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour {

	public Material activeMat;

	private MeshRenderer meshRenderer;

	// Use this for initialization
	void Start () {
		meshRenderer = GetComponent<MeshRenderer> ();
	}

	void OnTriggerEnter (Collider other) {
		if (other.CompareTag ("Player")) {
			meshRenderer.material = activeMat;
		}
	}
}
