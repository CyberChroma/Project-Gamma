using UnityEngine;
using System.Collections;

public class DestroyByLifetime : MonoBehaviour {

	public float lifetime; // Variable for the lifetime of the object
	
	// Update is called once per frame
	void Update () {
		Destroy (gameObject, lifetime); // Destroys the gameobject after a certain amount of time
	}
}