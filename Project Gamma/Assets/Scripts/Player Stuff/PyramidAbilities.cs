using UnityEngine;
using System.Collections;

public class PyramidAbilities : MonoBehaviour {

	public float shotDelay; // Variable for the time between ice block time shots
	public GameObject iceShard; // Reference to the ice shard prefab
	public Transform  iceShardParent;

	[HideInInspector] public bool canMove;
	private bool canShootIceShard; // Bool for whether the player can shoot
	private int numIceShards = 5;
	private GameObject[] iceShards;
	private int nextIceShard;

	// Bools for whether the player has unlocked certain abilities
	public bool shootIceShardUnlocked;

	// Use this for initialization
	void Awake () {
		// Setting starting values for bools
		canMove = false;
		canShootIceShard = true;
		iceShards = new GameObject[numIceShards];
		for (int i = 0; i < numIceShards; i++) {
			iceShards [i] = Instantiate (iceShard, iceShardParent);
			iceShards [i].SetActive (false);
		}
		nextIceShard = 0;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (canMove) {
			if (shootIceShardUnlocked) { // If the player has unlocked the shoot ice block ability
				ShootIceShard ();
			}
		}
	}

	void ShootIceShard () { // Makes the player shoot the ice block
		if (canShootIceShard && Input.GetKey(KeyCode.Mouse0)) { // If the player can shoot and has pressed the left mouse button
			iceShards [nextIceShard].transform.position = transform.position; // Sets the ice shard's positon
			iceShards [nextIceShard].transform.rotation = transform.rotation; // Sets the ice shard's rotation
			iceShards [nextIceShard].SetActive (true); // Turns on the ice shard
			nextIceShard++;
			if (nextIceShard >= numIceShards) {
				nextIceShard = 0;
			}
			canShootIceShard = false; // Disables the player's ability to shoot
			StartCoroutine (WaitToShoot ());
		}
	}

	IEnumerator WaitToShoot () { // Makes a delay for when the player can shoot again
		yield return new WaitForSeconds (shotDelay); // Waits for the desired amount of time
		canShootIceShard = true; // Re-enables the player's ability to shoot
	}
}