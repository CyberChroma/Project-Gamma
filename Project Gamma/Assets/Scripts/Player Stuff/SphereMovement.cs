using UnityEngine;
using System.Collections;

public class SphereMovement : MonoBehaviour {

	public float moveSpeed; // The move speed of the player
	public float airMoveSpeed;
	public float velocityDecreaseRate; // The speed the player slows down
	public Transform camPivot; // Reference to the camera pivot's transform

	[HideInInspector] public bool canMove;
	[HideInInspector] public float currentSpeed;
	[HideInInspector] public bool isGrounded;
	[HideInInspector] public Rigidbody rb; // Reference to the rigidbody component
	private FallingPlatformController fpc;
	private MovingObjectController mOC;

	void Awake () {
		canMove = false;
		isGrounded = false;
		currentSpeed = moveSpeed;
		rb = GetComponent<Rigidbody> (); // Getting the reference
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (canMove) {
			float h = Input.GetAxis ("Horizontal"); // Getting input from the left and right arrow keys (or a and d)
			float v = Input.GetAxis ("Vertical"); // Getting input from the up and down arrow keys (or w and s)
			Move (h, v);
		} else {
			rb.velocity = new Vector3 (Mathf.Lerp (rb.velocity.x, 0, velocityDecreaseRate / 10), rb.velocity.y, Mathf.Lerp (rb.velocity.z, 0, velocityDecreaseRate / 10));
		}
	}

	void Move (float h, float v) { // Moves the sphere
		if (mOC && mOC.isActive) {
			rb.position = (rb.position + mOC.velocity);
		}
		Vector3 force = camPivot.right * h + camPivot.forward * v; // Variable for force
		force = new Vector3(force.x, 0, force.z).normalized * currentSpeed;
		rb.AddForce (force); // Applying the force
		if (Input.GetKey (KeyCode.Space)) {
			rb.velocity = new Vector3 (Mathf.Lerp (rb.velocity.x, 0, velocityDecreaseRate / 10), rb.velocity.y, Mathf.Lerp (rb.velocity.z, 0, velocityDecreaseRate / 10));
		}
	}

	void OnCollisionStay (Collision other) {
		currentSpeed = moveSpeed;
		if (!isGrounded) {
			RaycastHit hit;
			if (Physics.Raycast (transform.position, Vector3.down, out hit, 0.35f)) {
				isGrounded = true;
			}
		}
		if (other.collider.CompareTag ("Stick") && !mOC) {
			mOC = other.gameObject.GetComponentInParent<MovingObjectController> ();
			rb.velocity -= mOC.velocity * 50;
			mOC.TestForActivate ();
		}
		if (other.collider.name.StartsWith ("Falling Platform")) {
			fpc = other.collider.GetComponent<FallingPlatformController> ();
			fpc.Fall ();
		}
	}

	void OnCollisionExit (Collision other) {
		currentSpeed = airMoveSpeed;
		if (isGrounded) {
			RaycastHit hit;
			if (!Physics.Raycast (transform.position, Vector3.down, out hit, 0.35f)) {
				isGrounded = false;
			}
		}
		if (other.collider.CompareTag ("Stick") && mOC) {
			rb.velocity += mOC.velocity * 50;
			mOC.TestForDeactivate ();
			mOC = null;
		}
		if (other.collider.name.StartsWith ("Falling Platform") && fpc) {
			fpc.Rise ();
			fpc = null;
		}
	}
}