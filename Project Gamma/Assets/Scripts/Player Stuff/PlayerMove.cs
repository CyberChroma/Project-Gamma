using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

	public float moveSpeed; // The speed of the player
	public float airMoveSpeed; // The speed of the player
	public float rotSmoothing; // The amount of rotational smoothing
	public float airRotSmoothing; // The amount of air rotational smoothing
	public float moveSensitivity;
	public float airMoveSensitivity;

	[HideInInspector] public Rigidbody rb; // Reference to the rigidbody
	[HideInInspector] public Vector3 moveVector;
	[HideInInspector] public bool canTurn;
	[HideInInspector] public Vector3 lookDir; // the direction the player should be facing
	[HideInInspector] public bool canMove;

	private float currentRotSmoothing;
	private Rigidbody stickRb;
	private MovingObjectController moc; // Temporary reference to a moving object controller script
	private FallingPlatformController fpc; // Temporary reference to a falling platform controller script
	private Transform camPivot; // Reference to the camera pivot
	private InputManager inputManager;  // Reference to the input manager
	private PlayerGroundCheck playerGroundCheck;
	private float v = 0;
	private float h = 0;

	// Use this for initialization
	void Awake () {
		canTurn = true;
		canMove = true;
		lookDir = transform.forward; // Setting the default value
		rb = GetComponent<Rigidbody> (); // Getting the reference
		camPivot = GameObject.Find ("Camera Pivot").transform; // Getting the reference
		inputManager = GameObject.Find ("Input Manager").GetComponent<InputManager> (); // Getting the reference
		currentRotSmoothing = rotSmoothing / 10;
		playerGroundCheck = GetComponent<PlayerGroundCheck> ();
	}

    // Update is called once per frame
    void Update () {
        GetInput();
    }

	void FixedUpdate () {
		Move ();
		Turn ();
	}

	void GetInput () {
		if (canMove) {
			if (playerGroundCheck.isGrounded) {
				if (inputManager.inputMB) {
					v = Mathf.Lerp (v, -1, moveSensitivity / 10);
				} else if (inputManager.inputMF) {
					v = Mathf.Lerp (v, 1, moveSensitivity / 10);
				} else {
					v = Mathf.Lerp (v, 0, moveSensitivity / 10);
				}
				// Calculating horizontal movement
				if (inputManager.inputML) {
					h = Mathf.Lerp (h, -1, moveSensitivity / 10);
				} else if (inputManager.inputMR) {
					h = Mathf.Lerp (h, 1, moveSensitivity / 10);
				} else {
					h = Mathf.Lerp (h, 0, moveSensitivity / 10);
				}
			} else {
				if (inputManager.inputMB) {
					v = Mathf.Lerp (v, -1, airMoveSensitivity / 10);
				} else if (inputManager.inputMF) {
					v = Mathf.Lerp (v, 1, airMoveSensitivity / 10);
				} else {
					v = Mathf.Lerp (v, 0, airMoveSensitivity / 10);
				}
				// Calculating horizontal movement
				if (inputManager.inputML) {
					h = Mathf.Lerp (h, -1, airMoveSensitivity / 10);
				} else if (inputManager.inputMR) {
					h = Mathf.Lerp (h, 1, airMoveSensitivity / 10);
				} else {
					h = Mathf.Lerp (h, 0, airMoveSensitivity / 10);
				}
			}
		}
	}

	void Move () { // Moves the cube
		moveVector = Vector3.ProjectOnPlane(camPivot.right, Vector3.up).normalized * h + Vector3.ProjectOnPlane(camPivot.forward, Vector3.up).normalized * v; // Variable for force
		if (moveVector.magnitude > 1) {
			moveVector.Normalize ();
		}
		if (playerGroundCheck.isGrounded) {
			moveVector *= moveSpeed * Time.deltaTime; // Calculating force
		} else {
			moveVector *= airMoveSpeed * Time.deltaTime; // Calculating force
		}
		if (canTurn) {
			lookDir = Vector3.ProjectOnPlane (moveVector, Vector3.up);
			if (lookDir == Vector3.zero) {
				lookDir = transform.forward;
			}
		}
		if (stickRb && stickRb.gameObject.activeSelf) { // If the player has a reference to a moving object controller
			moveVector += Vector3.ProjectOnPlane (stickRb.velocity * Time.deltaTime, Vector3.up); // Adding velocity of platform so they move together
		}
		rb.MovePosition (rb.position + moveVector); // Applying the movement
		if (playerGroundCheck.isGrounded) {
			rb.velocity *= 0.9f; // Simulating friction
		}
	}

	void Turn () {
		if (playerGroundCheck.isGrounded) {
			currentRotSmoothing = rotSmoothing / 10;
		} else {
			currentRotSmoothing = airRotSmoothing / 10;
		}
		rb.rotation = Quaternion.Slerp (rb.rotation, Quaternion.LookRotation (lookDir), currentRotSmoothing); // Rotating the player
		if (moc && moc.isActive) { // If the player has a reference to the moving object controller
			rb.rotation *= moc.rotVelocity; // Adds rotational velocity to make the player rotate with the platform
		}
	}

	void OnCollisionEnter (Collision other) {
		moc = other.gameObject.GetComponentInParent<MovingObjectController> (); // Getting a reference to the moving object controller script
		if (moc) {
			moc.TestForActivate (); // Testing to activate it
		}
		if (other.collider.CompareTag ("Stick")) { // If the player collided with a moving platform they should stick to
			stickRb = other.gameObject.GetComponentInParent<Rigidbody> ();
		} else if (other.gameObject.CompareTag ("Button")) {
            if (other.gameObject.GetComponent<ActivateFollowTarget>()) {
			    other.gameObject.GetComponent<ActivateFollowTarget>().Activate (); // Activates the button
            }
            if (other.gameObject.GetComponent<ActivateFallOnActivate>()) {
                other.gameObject.GetComponent<ActivateFallOnActivate>().Activate (); // Activates the button
            }
		} else if (other.gameObject.CompareTag ("Button-All")) { // If the player collided with a button
			other.gameObject.GetComponent<ButtonController>().Activate (); // Activates the button
		}
		if (other.collider.name.StartsWith ("Falling Platform")) { // If the player collided with falling platform (can't use tag)
			fpc = other.collider.GetComponent<FallingPlatformController> (); // Getting a reference to the falling platform controller script
			fpc.Fall (); // Making the platform fall
		}
	}

	void OnCollisionExit (Collision other) {
		if (moc) { // If the player has a reference to the moving object controller
			moc.TestForDeactivate (); // Tests to deactivate the platform
			moc = null; // Removing the reference
		}
		if (fpc) { // If the player has a reference to the falling platform controller
			fpc.Rise (); // Makes the platform start to rise
			fpc = null; // Removing the reference
		}
		if (stickRb) {
			stickRb = null;
		}
	}

	public IEnumerator TempStopMove (float delay) {
		h = 0;
		v = 0;
		canMove = false;
		yield return new WaitForSeconds (delay);
		canMove = true;
	}
}
