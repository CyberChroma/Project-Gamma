using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {

	public float delay = 1; // The lifetime of the object

	// Use this for initialization
	void Start () {
		Destroy (gameObject, delay); // Destroys the object after a certain number of seconds
	}
}
