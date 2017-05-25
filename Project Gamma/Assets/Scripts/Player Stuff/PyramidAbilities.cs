using UnityEngine;
using System.Collections;

public class PyramidAbilities : MonoBehaviour {

	public float shotDelay; // Variable for the time between ice block time shots
	public GameObject shot; // Reference to the ice shard prefab
	public Transform  shotParent; // Reference to the parent of the shots

	[HideInInspector] public bool canMove; // Whether the pyrmid can move
	private bool canShootShot; // Bool for whether the player can shoot
	private int numShots = 5; // The number of shots for the object pool
	private GameObject[] shots; // Array for the shots
	private int nextShot;
	private PyramidMovement pyramidMovement;

	// Bools for whether the player has unlocked certain abilities
	public bool shootShotUnlocked;

	// Use this for initialization
	void Awake () {
		// Setting starting values for bools
		canMove = false; // Setting the bool
		canShootShot = true; // Setting the bool
		shots = new GameObject[numShots]; // Initalizing the array
		for (int i = 0; i < numShots; i++) { // For the number of required shots
			shots [i] = Instantiate (shot, shotParent); // Creates the shot
			shots [i].SetActive (false); // Turning off the shot
		}
		nextShot = 0; // The first shot should be shot first
		pyramidMovement = GetComponent<PyramidMovement> (); // Getting the reference
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (canMove && !pyramidMovement.turning) { // If the player can move and isn't turning
			if (shootShotUnlocked) { // If the player has unlocked the shoot ice block ability
				ShootShot ();
			}
		}
	}

	void ShootShot () { // Makes the player shoot the ice block
		if (canShootShot && Input.GetKey(KeyCode.Mouse0)) { // If the player can shoot and has pressed the left mouse button
			shots [nextShot].transform.position = transform.position; // Sets the shot's positon
			shots [nextShot].transform.rotation = transform.rotation; // Sets the shot's rotation
			shots [nextShot].SetActive (true); // Turns on the shot
			nextShot++; // Increase the shot number
			if (nextShot >= numShots) { // If the end of the array has been reached
				nextShot = 0; // Go back to the beginning of the array
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