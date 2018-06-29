using UnityEngine;
using System.Collections;

public class PlayerSpin : MonoBehaviour {

	public float spinTime;
	public float spinDelay; // Variable for the time between punches
	public float airUpForce;
    public GameObject spinDetection;

	// Bools for whether the player can perform certain actions at certain times
	private bool canSpin;

	// Bools for whether the player is performing certain actions at certain times
	[HideInInspector] public bool isSpinning; 

	private Rigidbody rb;
	private Animator anim; // Reference to the animator component
	private PlayerGroundCheck playerGroundCheck;
	private InputManager inputManager;  // Reference to the input manager

	// Use this for initialization
	void Awake () {
		// Setting starting values for bools
		canSpin = true; // Setting the bool
		rb = GetComponent<Rigidbody> (); // Getting the reference
		anim = GetComponentInChildren<Animator> (); // Getting the reference
		playerGroundCheck = GetComponent<PlayerGroundCheck> (); // Getting the reference
		inputManager = GameObject.Find ("Input Manager").GetComponent<InputManager> (); // Getting the reference
        spinDetection.SetActive (false);
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (canSpin && inputManager.inputMA) { // If the player can punch and has pressed the ability button
            Spin ();
		}
	}

	void Spin () { // Makes the player do a punch attack
		anim.ResetTrigger ("Idle"); // Resetting the trigger
		anim.ResetTrigger ("Moving"); // Resetting the trigger
		anim.ResetTrigger ("Jump"); // Resetting the trigger
		anim.ResetTrigger ("Land"); // Resetting the trigger
		anim.SetTrigger ("Spin"); // Playing the animation
		if (!playerGroundCheck.isGrounded) {
			if (rb.velocity.y < 0) {
				rb.velocity = new Vector3 (rb.velocity.x, 0, rb.velocity.z);
			} else {
				rb.velocity /= 2;
			}
			rb.AddForce (Vector3.up * airUpForce * 100);
		}
		StartCoroutine (WaitToSpin ()); // Delay for between punches
	}
        
	void OnTriggerStay (Collider other) {
        if (isSpinning) { // If the cube is spinning
			if (other.CompareTag ("Spinnable")) { // If the player collided with something spinnable
                other.GetComponent<Rigidbody>().isKinematic = false;
            } else if (other.CompareTag ("Enemy")) {
                other.GetComponent<Health>().Damage();
            }
		}
	}

	IEnumerator WaitToSpin () { // Makes a delay for when the player can spin again
		canSpin = false; // Disables the spin ability
        isSpinning = true;
        spinDetection.SetActive (true);
		yield return new WaitForSeconds (spinTime); // Waits for the desired amount of time
        isSpinning = false;
        spinDetection.SetActive (false);
		yield return new WaitForSeconds (spinDelay); // Waits for the desired amount of time
        canSpin = true; // Re-enables the player's ability to spin
	}
}