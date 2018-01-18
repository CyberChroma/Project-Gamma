using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float speed; // The move speed of the camera

	public bool rotInvert; // Whether the rotation of the camera is inverted
	public Transform camPos;
	public Transform cam; // Reference to the camera

	public Transform target; // The currently active player
	private Rigidbody rb; // References to the rigidbody component

	public float sensitivityX = 3f; // The horizontal rotation speed of the camera from the mouse
	public float sensitivityY = 3f;	// The vertical rotation speed of the camera from the mouse

	private float rotationX = 0f; // The x rotation of the camera
	private float rotationY = 0f; // The y rotation of the camera

	// Use this for initialization
	void Start () {		
		transform.position = target.position; // Moving the camera pivot to the player's position
		transform.rotation = target.rotation; // Rotating the camera pivot to behind the player
		rb = GetComponent <Rigidbody> (); // Getting the reference
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		FollowPlayer ();
		TurnCam ();
	}

	void FollowPlayer () {
		if (target != null) {
			rb.position = Vector3.Lerp (rb.position, target.position, speed / 10); // Making the camera pivot follow the player
		}
	}

	void TurnCam () {
		float inputX = Input.GetAxis ("Mouse X"); // Gets input from the mouse's vertical position
		float inputY = Input.GetAxis ("Mouse Y"); // Gets input from the mouse's horizontal position
		if (rotInvert) {
			rotationX -= inputY * sensitivityY; // Setting the x rotation
			rotationY -= inputX * sensitivityX; // Setting the y rotation
		} else {
			rotationX += inputY * sensitivityY; // Setting the x rotation
			rotationY += inputX * sensitivityX; // Setting the y rotation
		}
		rotationX = Mathf.Clamp (rotationX, -80, 80);// Clamping the rotation
		rb.rotation = Quaternion.Euler (new Vector3(-rotationX, rotationY, 0)); // Turning the camera
	}
}