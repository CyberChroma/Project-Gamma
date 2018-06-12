using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectByTime : MonoBehaviour {

	public GameObject objectToSpawn; // The object to spawn
    public Transform parent; // The object to parent the spawned object under
    public bool copyTransform;
    public float delay = 1; // The time between spawning
	public float randDelay = 0.1f; // Adds slight randomness to the delay
    public float offset = 0;

	[HideInInspector] public GameObject spawnedObject; // Reference to the spawned object
	private bool canSpawn; // Whether a object can be spawned
	private float timeUntilNextSpawn; // The delay before starting to spawn again

	void Awake () {
        if (parent == null)
        {
            parent = GameObject.Find("Object Pools").transform;
        }
        timeUntilNextSpawn = Time.time + offset;
	}

	void OnEnable () {
        if (delay == 0) {
            Spawn();
            gameObject.SetActive (false);
        }
	}

	void OnDisable () {
		canSpawn = false;
	}

	void FixedUpdate () {
		if (Time.time >= timeUntilNextSpawn) { // If the time has elapsed
			canSpawn = true;
		}
		if (canSpawn) { // If this emitter can spawn objects
			Spawn ();
		}
	}

	void Spawn () {
        if (copyTransform) { // If there is a parent
			spawnedObject = Instantiate (objectToSpawn, parent); // Spawns the object as a parent of a transform
		} else {
			spawnedObject = Instantiate (objectToSpawn, transform.position, transform.rotation, parent); // Spawns the object as a parent of a transform
		}
		canSpawn = false;
		timeUntilNextSpawn = Time.time + delay + Random.Range (-randDelay, randDelay); // Setting the delay with slight randomness
	}
}
