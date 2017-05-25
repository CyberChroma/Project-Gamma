using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningPointController : MonoBehaviour {

	public bool activated; // Whether the turn point has been activated

	public Material unactiveMat; // The unactive material
	public Material activeMat; // The active material

	private MeshRenderer[] meshRenderers; // References to the mesh renderers

	// Use this for initialization
	void Start () {
		meshRenderers = GetComponentsInChildren<MeshRenderer> (); // Getting the references
		if (!activated) { // If the turn point starts unactive
			foreach (MeshRenderer meshRenderer in meshRenderers) { // Going through each object
				meshRenderer.material = unactiveMat; // Changing the material
			}
			gameObject.tag = "Untagged"; // Setting the tag
		}
	}

	public void Activate () { // Activates the turn point (Called by button)
		foreach (MeshRenderer meshRenderer in meshRenderers) { // Going through each object
			meshRenderer.material = activeMat; // Changing the material
		}
		gameObject.tag = "Turning Point"; // Setting the tag
	}
}
