using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracking : MonoBehaviour {

	public float distance = 4; // The starting distance between the camera and the pivot
	public float noClipSmoothing = 4; // The smoothing applied to the movement of the camera to avoid clipping through walls
	public Transform camPivot; // Reference to the camera pivot's transform
	public Transform player; // Reference to the player's transform

	private float currentDis; // Distance between player and camera
	private CameraController cameraController; // Reference to the camera controller

	// Use this for initialization
	void Awake () {
		currentDis = -distance; // Setting distance
		cameraController = camPivot.GetComponent<CameraController> (); // Getting reference
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (cameraController.target.gameObject == player.gameObject) { // If the active player is the player this script is following
			MoveCam (); // Moves the camera, in the event of wall clipping
			TurnCam (); // Turns the camera to face just above the player, mainly in the event of wall clipping
		}
	}

	void MoveCam () {
		RaycastHit hit; // Used to get information from the raycast
		if (Physics.Linecast (player.position, transform.position, out hit, 1 << 8) && hit.distance < distance) { // Casts a line from the player to the camera. The if statement runs if it hits something
			currentDis = -Mathf.Clamp (hit.distance - 0.6f, 1f, distance); // Changing the distance to zoom in
		} else if (!Physics.Linecast (player.position, camPivot.transform.TransformPoint (new Vector3 (transform.localPosition.x, 0, currentDis - 1f)), out hit, 1 << 8) && currentDis < distance) { // Casts a line to the camera's original position to see if it can move back
			currentDis = -Mathf.Clamp (-currentDis + 0.1f, 1f, distance); // Changing the distance to its original position
		}
		transform.localPosition = Vector3.Lerp (transform.localPosition, new Vector3 (transform.localPosition.x, 0, currentDis), noClipSmoothing / 10); // Moves the camera back to its normal position
	}

	void TurnCam () {
		transform.LookAt (new Vector3 (player.position.x, player.position.y + 0.5f, player.position.z)); // Turns the camera to look just above the player
	}
}
