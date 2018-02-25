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
			cam.position = Vector3.Lerp (cam.position, camPos.position, speed / 10); // Moving the camera
			cam.rotation = Quaternion.Lerp (cam.rotation, camPos.rotation, speed / 10); // Rotating the camera
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
	/*

	public float speed; // The move speed of the camera
	public float transitionSpeed; // The transition speed
	public float transitionRotSpeed; // The transition rotation speed

	public bool rotInvert; // Whether the rotation of the camera is inverted
	public Transform cam; // Reference to the camera

	[HideInInspector] public GameObject activePlayer; // The currently active player
	private CharacterChanging characterChanging; // Reference to the character changing script
	private Rigidbody rb; // References to the rigidbody component
	private PyramidMovement pyramidMovement; // Reference to the pyramid's movement script

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
		pyramidMovement = characterChanging.pyramid.GetComponent<PyramidMovement> (); // Getting the reference
	}

	// Update is called once per frame
	void FixedUpdate () {
		FollowPlayer ();
		TurnCam ();
	}

	void FollowPlayer () {
		if (activePlayer == characterChanging.cube) { // If the active player is the cube
			cam.position = Vector3.Lerp (cam.position, characterChanging.cubeCamPos.position, transitionSpeed / 10); // Moving the camera
			cam.rotation = Quaternion.Lerp (cam.rotation, characterChanging.cubeCamPos.rotation, transitionRotSpeed / 10); // Rotating the camera
		} else if (activePlayer == characterChanging.sphere) { // If the active player is the sphere
			cam.position = Vector3.Lerp (cam.position, characterChanging.sphereCamPos.position, transitionSpeed / 10); // Moving the camera
			cam.rotation = Quaternion.Lerp (cam.rotation, characterChanging.sphereCamPos.rotation, transitionRotSpeed / 10); // Rotating the camera
		} else { // (If the active player is the pyramid)
			cam.position = Vector3.Lerp (cam.position, characterChanging.pyramidCamPos.position, transitionSpeed / 10); // Moving the camera
			cam.rotation = Quaternion.Lerp (cam.rotation, characterChanging.pyramidCamPos.rotation, transitionRotSpeed / 10); // Rotating the camera
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
		if (activePlayer == characterChanging.pyramid) { // If the active player is the pyramid
			if (characterChanging.pyramid.transform.rotation.eulerAngles.y > 350 && rotationY < 180) { // If the rotation of the pyramid is greater that 350 and the y rotation is less than 180
				rotationY += 360; // Adds 360 to the y rotation
			}
			if (rotationY < characterChanging.pyramid.transform.rotation.eulerAngles.y - 180 || rotationY > characterChanging.pyramid.transform.rotation.eulerAngles.y) {
				pyramidMovement.invertMovement = true;
			} else {
				pyramidMovement.invertMovement = false;
			}
		}
		rb.rotation = Quaternion.Euler (new Vector3(-rotationX, rotationY, 0)); // Turning the camera
	} */
}