using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour {

	public float jumpPower; // The amount of jump force
	public float fallMultiplier = 2f;
	public float lowJumpMultiplier = 1.5f;

	[HideInInspector] public bool canJump; // If the player can jump
	[HideInInspector] public Rigidbody rb; // Reference to the rigidbody

	private Animator anim; // Reference to the animator component
	private InputManager inputManager;  // Reference to the input manager
	private PlayerGroundCheck playerGroundCheck;
    private bool hasJumped;

	// Use this for initialization
	void Awake () {
		rb = GetComponent<Rigidbody> (); // Getting the reference
		anim = GetComponentInChildren<Animator> (); // Getting the reference
		inputManager = GameObject.Find ("Input Manager").GetComponent<InputManager> (); // Getting the reference
		playerGroundCheck = GetComponent<PlayerGroundCheck> ();
        canJump = false;
        hasJumped = false;
	}

	// Update is called once per frame
	void Update () {
		if (canJump) { // If the player can jump
			Jump ();
		}
		if (rb.velocity.y < 0) {
			rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		} else if (rb.velocity.y > 0 && !inputManager.inputJ) {
			rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}
	}

	void Jump () { // Makes the cube jump
        if (inputManager.inputJD) { // If the player hits the jump button and can jump
			rb.AddForce (Vector3.up * jumpPower * 100); // Applying jump force
			anim.SetTrigger ("Jump"); // Setting the trigger
			anim.SetBool ("Falling", false); // Setting the bool
			anim.SetBool ("Idle", false); // Setting the bool
			anim.SetBool ("Moving", false); // Setting the bool
			playerGroundCheck.isGrounded = false; // Setting the bool
			canJump = false; // Setting the bool
            hasJumped = true;
            StartCoroutine(WaitToJump());
		}
	}

	void OnCollisionEnter (Collision other) {
		foreach (ContactPoint point in other.contacts) {
            if (Vector3.Angle (Vector3.up, point.normal) <= playerGroundCheck.slopeLimit && Vector3.Angle (Vector3.up, point.normal) >= -playerGroundCheck.slopeLimit && !hasJumped) { // If the slope is not too steep
				canJump = true; // Setting the bool
			}
		}
	}

	void OnCollisionStay (Collision other) {
		foreach (ContactPoint point in other.contacts) {
            if (Vector3.Angle (Vector3.up, point.normal) <= playerGroundCheck.slopeLimit && Vector3.Angle (Vector3.up, point.normal) >= -playerGroundCheck.slopeLimit && !canJump && !hasJumped) { // If the slope is not too steep
				canJump = true; // Setting the bool
			}
		}
	}

	void OnCollisionExit (Collision other) {
        if (rb.velocity.y > 1)
        {
            canJump = false;
        }
        StartCoroutine(WaitToDisableJump());
	}

    IEnumerator WaitToDisableJump () {
        yield return new WaitForSeconds(0.2f);
        if (canJump) {
            canJump = false; // Setting the bool
        }
    }

    IEnumerator WaitToJump () {
        yield return new WaitForSeconds(0.2f);
        hasJumped = false;
    }
}
