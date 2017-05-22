using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float speed; // The move speed of the camera
	public float transitionSpeed;
	public float transitionRotSpeed;

	public bool rotInvert; // Is the rotation of the camera inverted
	public Transform cam;

	[HideInInspector] public GameObject activePlayer; // The currently active player
	private CharacterChanging characterChanging;
	private Rigidbody rb; // References to the rigidbody component
	private PyramidMovement pyramidMovement;

	public float sensitivityX = 3f; // The horizontal rotation speed of the camera from the mouse
	public float sensitivityY = 3f;	// The vertical rotation speed of the camera from the mouse

	private float rotationX = 0f; // The x rotation of the camera
	private float rotationY = 0f; // The y rotation of the camera

	// Use this for initialization
	void Start () {		
		characterChanging = GetComponent<CharacterChanging> (); // Getting the reference
		transform.position = activePlayer.transform.position; // Moving the camera pivot to the player's position
		transform.rotation = activePlayer.transform.rotation; // Rotating the camera pivot to behind the player
		rb = GetComponent <Rigidbody> (); // Getting the reference
		pyramidMovement = characterChanging.pyramid.GetComponent<PyramidMovement> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		FollowPlayer ();
		TurnCam ();
	}

	void FollowPlayer () {
		if (activePlayer == characterChanging.cube) {
			cam.position = Vector3.Lerp (cam.position, characterChanging.cubeCamPos.position, transitionSpeed / 10);
			cam.rotation = Quaternion.Lerp (cam.rotation, characterChanging.cubeCamPos.rotation, transitionRotSpeed / 10);
		} else if (activePlayer == characterChanging.sphere) {
			cam.position = Vector3.Lerp (cam.position, characterChanging.sphereCamPos.position, transitionSpeed / 10);
			cam.rotation = Quaternion.Lerp (cam.rotation, characterChanging.sphereCamPos.rotation, transitionRotSpeed / 10);
		} else {
			cam.position = Vector3.Lerp (cam.position, characterChanging.pyramidCamPos.position, transitionSpeed / 10);
			cam.rotation = Quaternion.Lerp (cam.rotation, characterChanging.pyramidCamPos.rotation, transitionRotSpeed / 10);
		}
		rb.position = Vector3.Lerp (rb.position, activePlayer.transform.position, speed / 10); // Making the camera pivot follow the player
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
		if (activePlayer == characterChanging.pyramid) {
			if (characterChanging.pyramid.transform.rotation.eulerAngles.y > 350 && rotationY < 180) {
				rotationY += 360;
			}
			rotationY = Mathf.Clamp (rotationY, characterChanging.pyramid.transform.rotation.eulerAngles.y - 170, characterChanging.pyramid.transform.rotation.eulerAngles.y - 10);// Clamping the rotation
		}
		if (Input.GetMouseButton (2) && activePlayer == characterChanging.cube) {
			rotationY = activePlayer.transform.rotation.eulerAngles.y; // Center the camera behind the player
		}
		if (Input.GetMouseButtonDown (2) && activePlayer == characterChanging.pyramid) {
			pyramidMovement.turning = true;
			pyramidMovement.canMove = false;
			pyramidMovement.targetRotation -= 180;
			pyramidMovement.inertia = Vector3.zero;
		}
		transform.localEulerAngles = new Vector3(-rotationX, rotationY, 0); // Turning the camera
	}
}