using UnityEngine;
using System.Collections;

public class PyramidAbilities : MonoBehaviour {

	public float shotDelay; // Variable for the time between ice block time shots
	public GameObject shot; // Reference to the pyramid shot prefab

	[HideInInspector] public bool canMove; // Whether the pyrmid can move
	private bool canShootShot; // Bool for whether the player can shoot

	// Bools for whether the player has unlocked certain abilities
	public bool shootShotUnlocked;

	private PyramidMovement pyramidMovement;
	private Animator anim; // Reference to the animator component
	private AnimatorStateInfo stateInfo; // Reference to the current animation state
	// Use this for initialization
	void Awake () {
		// Setting starting values for bools
		canMove = false; // Setting the bool
		canShootShot = true; // Setting the bool
		pyramidMovement = GetComponent<PyramidMovement> (); // Getting the reference
		anim = GetComponentInChildren<Animator> (); // Getting the reference
	}

	// Update is called once per frame
	void FixedUpdate () {
		stateInfo = anim.GetCurrentAnimatorStateInfo (0); // Getting the information
		if (canMove && !pyramidMovement.turning) { // If the player can move and isn't turning
			if (shootShotUnlocked) { // If the player has unlocked the shoot ice block ability
				Shoot ();
			}
		}
	}

	void Shoot () { // Makes the player shoot the ice block
		if (canShootShot && Input.GetKey(KeyCode.Mouse0)) { // If the player can shoot and has pressed the left mouse button
			Instantiate (shot, transform.position, transform.rotation); // Creates the shot
			anim.ResetTrigger ("Idle"); // Resetting the trigger
			anim.ResetTrigger ("Moving"); // Resetting the trigger
			anim.ResetTrigger ("Jump"); // Resetting the trigger
			anim.ResetTrigger ("Land"); // Resetting the trigger
			anim.SetTrigger ("Shoot"); // Playing the animation
			StartCoroutine (WaitToShoot ());
		}
	}

	IEnumerator WaitToShoot () { // Makes a delay for when the player can shoot again
		canShootShot = false; // Disables the player's ability to shoot
		yield return new WaitForSeconds (shotDelay); // Waits for the desired amount of time
		canShootShot = true; // Re-enables the player's ability to shoot
	}
}