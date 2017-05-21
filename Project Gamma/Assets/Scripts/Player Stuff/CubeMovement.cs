using UnityEngine;
using System.Collections;

public class CubeMovement : MonoBehaviour {

	public float moveSpeed; // The speed of the player
	public float rotSpeed; // The rotation speed of the player

	public float gravity;
	public float jumpPower;

	public float wallJumpVSpeedUp;
	public float wallJumpVSpeedSide;

	public float wallJumpHSpeedForward;
	public float wallJumpHSpeedSide;
	public float wallJumpHSpeedUp;

	[HideInInspector] public bool canMove;
	[HideInInspector] public float verticalVelocity;
	[HideInInspector] public Vector3 inertia;

	private bool wasOnGround;
	private bool canJump;
	private bool storeMovement;
	private Vector3 lastMove;
	private FallingPlatformController fpc;
	private MovingObjectController mOC;
	private CharacterController controller; // Reference to the rigidbody component
	private Animator anim;
	private AnimatorStateInfo stateInfo;

	private float inputV;
	private float inputH;
	private bool inputJ;

	void Awake () {
		wasOnGround = false;
		canJump = false;
		storeMovement = false;
		canMove = false;
		controller = GetComponent<CharacterController> (); // Getting the reference
		anim = GetComponentInChildren<Animator> ();
	}

	void Update () {
		stateInfo = anim.GetCurrentAnimatorStateInfo (0);
		if (canMove) {
			inputV = Input.GetAxisRaw ("Vertical"); // Getting input from the up and down arrow keys (or w and s)
			inputH = Input.GetAxisRaw ("Horizontal"); // Getting input from the left and right arrow keys (or a and d)
			inputJ = Input.GetKeyDown (KeyCode.Space);
			if (inputH != 0 && storeMovement) {
				storeMovement = false;
			}
		} else {
			inputH = 0;
			inputV = 0;
			inputJ = false;
		}
		if (transform.rotation.eulerAngles.x != 0 || transform.rotation.eulerAngles.z != 0) {
			transform.rotation = Quaternion.Euler (Vector3.up * transform.rotation.eulerAngles.y);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		stateInfo = anim.GetCurrentAnimatorStateInfo (0);
		if (inputV == 0 && inputH == 0 && controller.isGrounded) {
			if (!stateInfo.IsName ("Idle") && !anim.IsInTransition (0)) {
				anim.SetTrigger ("Idle");
			}
		} else {
			if (!stateInfo.IsName ("Moving") && !anim.IsInTransition (0)) {
				anim.SetTrigger ("Moving");
			}
		}
		if (!wasOnGround && controller.isGrounded) {
			canJump = true;
			wasOnGround = true;
			if (!stateInfo.IsName ("Land")) {
				anim.ResetTrigger ("Idle");
				anim.ResetTrigger ("Moving");
				anim.SetTrigger ("Land");
			}
		} else if (wasOnGround && !controller.isGrounded) {
			if (mOC) {
				mOC.TestForDeactivate ();
				mOC = null;
			}
			if (fpc) {
				fpc.Rise ();
				fpc = null;
			}
			verticalVelocity = 0;
			inertia = lastMove / 2;
			StartCoroutine (WaitToDisableJump ());
			wasOnGround = false;
		} else if (!controller.isGrounded) {
			verticalVelocity -= gravity * Time.deltaTime;
			verticalVelocity = Mathf.Clamp (verticalVelocity, -gravity, gravity);
		}
		if (canJump) {
			Jump ();
		} 
		Move ();
		if (mOC && mOC.isActive) {
			transform.rotation *= mOC.rotVelocity;
		}
		if (inputH != 0 && !storeMovement) { // If the player should be turning
			Turn ();
		}
	}

	void Move () { // Moves the cube
		if (!storeMovement) {
			Vector3 moveVector;
			moveVector = transform.forward * inputV * moveSpeed * Time.deltaTime; // Variable for movement
			if (!controller.isGrounded) {
				moveVector += inertia;
				moveVector /= 2;
			}
			moveVector.y = verticalVelocity * Time.deltaTime;
			if (mOC) {
				moveVector += mOC.velocity;
			}
			controller.Move (moveVector); // Applying the movement
			lastMove = moveVector;
		} else {
			if (mOC && mOC.isActive) {
				lastMove += mOC.velocity;
			}
			lastMove.y = verticalVelocity * Time.deltaTime;
			controller.Move (lastMove);
		}
	}

	void Turn () { // Turns the cube
		float turning; // Creating a variable for turning
		turning = inputH * rotSpeed * 10 * Time.deltaTime; // Setting the variable
		if (controller.isGrounded) {
			transform.Rotate (Vector3.up * turning);
		} else {
			transform.Rotate (Vector3.up * turning / 2);
		}
	}

	void Jump () {
		if (inputJ && canJump) {
			verticalVelocity = jumpPower;
			inertia = lastMove;
			canJump = false;
			wasOnGround = false;
			if (!stateInfo.IsName ("Jump")) {
				anim.SetTrigger ("Jump");
			}
			if (mOC) {
				mOC.TestForDeactivate ();
				mOC = null;
			}
			if (fpc) {
				fpc.Rise ();
				fpc = null;
			}
		}
	}

	IEnumerator WaitToDisableJump () {
		yield return new WaitForSeconds (0.25f);
		if (!controller.isGrounded) {
			canJump = false;
		}
	}

	void OnControllerColliderHit (ControllerColliderHit hit) {
		if (hit.collider.CompareTag ("Wall Jump V")) {
			verticalVelocity *= 0.95f;
			if (inputJ && !controller.isGrounded) { // If the player presses the space bar and are not on the ground
				StartCoroutine (WallJumpV (hit));
			}
		} else if (hit.collider.CompareTag ("Wall Jump H")) {
			verticalVelocity *= 0.95f;
			if (inputJ && !controller.isGrounded) { // If the player presses the space bar and are not on the ground
				StartCoroutine (WallJumpH (hit));
			}
		} else if (hit.collider.CompareTag ("Stick")) {
			mOC = hit.gameObject.GetComponentInParent<MovingObjectController> ();
			mOC.TestForActivate ();
		} else {
			inertia = Vector3.zero;
			storeMovement = false;
		}
		if (hit.collider.name.StartsWith ("Falling Platform")) {
			fpc = hit.collider.GetComponent<FallingPlatformController> ();
			fpc.Fall ();
		}
	}

	IEnumerator WallJumpV (ControllerColliderHit hit) {
		yield return new WaitForFixedUpdate ();
		transform.rotation = Quaternion.LookRotation (hit.normal);
		verticalVelocity = wallJumpVSpeedUp;
		lastMove = (hit.normal * wallJumpVSpeedSide) * Time.deltaTime;
		storeMovement = true;
		StartCoroutine (TempDisable (0.25f));
		if (!stateInfo.IsName ("Jump")) {
			anim.SetTrigger ("Jump");
		}
	}

	IEnumerator WallJumpH (ControllerColliderHit hit) {
		yield return new WaitForFixedUpdate ();
		Debug.DrawRay (hit.point, hit.normal, Color.red, 5f);
		transform.rotation = Quaternion.LookRotation (hit.normal, Vector3.up);
		verticalVelocity = wallJumpHSpeedUp;
		lastMove = (hit.transform.right * wallJumpHSpeedForward + hit.normal * wallJumpHSpeedSide) * Time.deltaTime;
		storeMovement = true;
		StartCoroutine (TempDisable (0.25f));
		if (!stateInfo.IsName ("Jump")) {
			anim.SetTrigger ("Jump");
		}
	}

	IEnumerator TempDisable (float delay) {
		canMove = false;
		yield return new WaitForSeconds (delay);
		canMove = true;
	}
}