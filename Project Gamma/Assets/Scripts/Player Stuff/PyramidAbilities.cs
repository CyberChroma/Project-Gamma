using UnityEngine;
using System.Collections;

public class PyramidAbilities : MonoBehaviour {

	public float shotDelay; // Variable for the time between ice block time shots
	public GameObject shot; // Reference to the ice shard prefab
	public Transform  shotParent;

	[HideInInspector] public bool canMove;
	private bool canShootShot; // Bool for whether the player can shoot
	private int numShots = 5;
	private GameObject[] shots;
	private int nextShot;
	private PyramidMovement pyramidMovement;

	// Bools for whether the player has unlocked certain abilities
	public bool shootShotUnlocked;

	// Use this for initialization
	void Awake () {
		// Setting starting values for bools
		canMove = false;
		canShootShot = true;
		shots = new GameObject[numShots];
		for (int i = 0; i < numShots; i++) {
			shots [i] = Instantiate (shot, shotParent);
			shots [i].SetActive (false);
		}
		nextShot = 0;
		pyramidMovement = GetComponent<PyramidMovement> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (canMove && !pyramidMovement.turning) {
			if (shootShotUnlocked) { // If the player has unlocked the shoot ice block ability
				ShootShot ();
			}
		}
	}

	void ShootShot () { // Makes the player shoot the ice block
		if (canShootShot && Input.GetKey(KeyCode.Mouse0)) { // If the player can shoot and has pressed the left mouse button
			shots [nextShot].transform.position = transform.position; // Sets the ice shard's positon
			shots [nextShot].transform.rotation = transform.rotation; // Sets the ice shard's rotation
			shots [nextShot].SetActive (true); // Turns on the ice shard
			nextShot++;
			if (nextShot >= numShots) {
				nextShot = 0;
			}
			canShootShot = false; // Disables the player's ability to shoot
			StartCoroutine (WaitToShoot ());
		}
	}

	IEnumerator WaitToShoot () { // Makes a delay for when the player can shoot again
		yield return new WaitForSeconds (shotDelay); // Waits for the desired amount of time
		canShootShot = true; // Re-enables the player's ability to shoot
	}
}