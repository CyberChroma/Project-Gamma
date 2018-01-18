using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveInput : MonoBehaviour {

	public float moveSensitivity = 0.5f;
	public Camera mCam;

	private float v = 0; // Vertical direction
	private float h = 0; // Horizontal direction
	private MoveByForce playerMove; // Reference to player move by force script
	private MoveInputReceiver moveInputReceiver; // Reference to input manager

	// Use this for initialization
	void Awake () {
		playerMove = GetComponent<MoveByForce> (); // Getting the reference
		moveInputReceiver = GameObject.Find ("Input Manager").GetComponent<MoveInputReceiver>(); // Getting the reference
	}

	void OnDisable () {
		v = 0;
		h = 0;
		playerMove.dir = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		// Calculating vertical direction
		if (moveInputReceiver.inputMB) {
			v = Mathf.MoveTowards (v, -1, moveSensitivity);
		} else if (moveInputReceiver.inputMF) {
			v = Mathf.MoveTowards (v, 1, moveSensitivity);
		} else {
			v = Mathf.MoveTowards (v, 0, moveSensitivity);
		}
		// Calculating horizontal direction
		if (moveInputReceiver.inputML) {
			h = Mathf.MoveTowards (h, -1, moveSensitivity);
		} else if (moveInputReceiver.inputMR) {
			h = Mathf.MoveTowards (h, 1, moveSensitivity);
		} else {
			h = Mathf.MoveTowards (h, 0, moveSensitivity);
		}
		Vector3 dir = new Vector3 (h, 0, v);
		if (dir.magnitude < 1) {
			playerMove.dir = dir; // Setting direction
		} else {
			playerMove.dir = dir.normalized; // Setting direction
		}
		
	}
}
