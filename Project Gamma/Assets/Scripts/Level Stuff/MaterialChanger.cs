using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour {

	public string changeTag;
	public Material startMat;
	public Material changeMat;

	private MeshRenderer meshRenderer;

	// Use this for initialization
	void Start () {
		meshRenderer = GetComponent<MeshRenderer> ();
		meshRenderer.material = startMat;
	}

	void OnTriggerEnter (Collider other) {
		if (other.CompareTag (changeTag)) {
			meshRenderer.material = changeMat;
		}
	}
}
