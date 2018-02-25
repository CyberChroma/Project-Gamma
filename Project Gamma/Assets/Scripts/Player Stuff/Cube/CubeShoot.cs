using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeShoot : MonoBehaviour {

	public float shotDelay; // Variable for the time between ice block time shots
	public GameObject shot; // Reference to the pyramid shot prefab

	private bool canShootShot; // Bool for whether the player can shoot
	private InputManager inputManager;
	private Animator anim; // Reference to the animator component

	// Use this for initialization
	void Awake () {
		// Setting starting values for bools
		canShootShot = true; // Setting the bool
		anim = GetComponentInChildren<Animator> (); // Getting the reference
		inputManager = GameObject.Find ("Input Manager").GetComponent<InputManager> (); // Getting the reference
	}

	// Update is called once per frame
	void FixedUpdate () {
		Shoot ();
	}

	void Shoot () { // Makes the player shoot the projectile
        if (canShootShot && inputManager.inputA1) { // If the player can shoot and has pressed the left mouse button
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
