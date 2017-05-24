using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningPointController : MonoBehaviour {

	public bool activated;

	public Material unactiveMat;
	public Material activeMat;

	private MeshRenderer[] meshRenderers;

	// Use this for initialization
	void Start () {
		meshRenderers = GetComponentsInChildren<MeshRenderer> ();
		if (!activated) {
			foreach (MeshRenderer meshRenderer in meshRenderers) {
				meshRenderer.material = unactiveMat;
			}
			gameObject.tag = "Untagged";
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Activate () {
		foreach (MeshRenderer meshRenderer in meshRenderers) {
			meshRenderer.material = activeMat;
		}
		gameObject.tag = "Turning Point";
	}
}
