using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByLifetime : MonoBehaviour {

	public float lifetime = 1; // The lifetime of the object

	// Use this for initialization
	void Start () {
		Destroy (gameObject, lifetime); // Destroys the object after a certain number of seconds
	}
}
