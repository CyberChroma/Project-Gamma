﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeWallJump : MonoBehaviour {

	public float wallJumpPowerU; // The amount of vertical force in a wall jump
	public float wallJumpPowerF; // The amount of horizontal force in a wall jump
	public float minWallHeight;

	[HideInInspector] public Rigidbody rb; // Reference to the rigidbody

    private bool canWallJump;
	private bool stopWallJump;
	private PlayerMove playerMove;
	private PlayerJump playerJump;
    private Vector3 launchDir;
	private Animator anim; // Reference to the animator component
	private InputManager inputManager;  // Reference to the input manager

	// Use this for initialization
	void Awake () {
		playerMove = GetComponent<PlayerMove> ();
		playerJump = GetComponent<PlayerJump> ();
		rb = GetComponent<Rigidbody> (); // Getting the reference
		anim = GetComponentInChildren<Animator> (); // Getting the reference
		inputManager = GameObject.Find ("Input Manager").GetComponent<InputManager> (); // Getting the reference
	}

	void OnEnable () {
		canWallJump = true;
	}

	void OnDisable () {
		canWallJump = false;
	}

    void Update () {
        if (canWallJump)
        {
            if (inputManager.inputJD && !Physics.Raycast (transform.position, Vector3.down, minWallHeight)) {
                WallJump ();
            }
        }
    }

    void OnCollisionEnter (Collision other) {
        if (!stopWallJump) {
            if (Vector3.Angle(Vector3.up, other.contacts[0].normal) <= 100 && Vector3.Angle(Vector3.up, other.contacts[0].normal) >= 80 && !playerJump.canJump)
            {
                if (Vector3.Angle(other.contacts[0].normal, Vector3.ProjectOnPlane(Vector3.Reflect(playerMove.moveVector.normalized, other.contacts[0].normal), Vector3.up)) <= 90)
                {
                    launchDir = Vector3.ProjectOnPlane(Vector3.Reflect(playerMove.moveVector.normalized, other.contacts[0].normal), Vector3.up);
                }
                canWallJump = true;
            }
        }
    }

	void OnCollisionStay (Collision other) {
		if (!stopWallJump) {
            if (Vector3.Angle(Vector3.up, other.contacts[0].normal) <= 100 && Vector3.Angle(Vector3.up, other.contacts[0].normal) >= 80 && !playerJump.canJump)
            {
                if (rb.velocity.y < 0)
                { 
                    rb.velocity *= 0.9f;
                }
                if (Vector3.Angle(other.contacts[0].normal, Vector3.ProjectOnPlane(Vector3.Reflect(playerMove.moveVector.normalized, other.contacts[0].normal), Vector3.up)) <= 90)
                {
                    launchDir = Vector3.ProjectOnPlane(Vector3.Reflect(playerMove.moveVector.normalized, other.contacts[0].normal), Vector3.up);
                }
                canWallJump = true;
            }
		}
	}

    void OnCollisionExit (Collision other) {
        StartCoroutine(WaitToDisableWallJump());
    }

	void WallJump () {
		inputManager.inputJ = false;
		rb.velocity = Vector3.zero; // Resetting the velocity
        rb.AddForce (launchDir * wallJumpPowerF * 100 + Vector3.up * wallJumpPowerU * 100); // Pushing the player up and away from the wall
        playerMove.lookDir = Vector3.ProjectOnPlane(launchDir, Vector3.up); // Turning the player to face away from the wall
		StartCoroutine (StopTurn ());
		StartCoroutine (StopWallJump ());
		anim.SetTrigger ("Jump"); // Setting the trigger
		StartCoroutine (playerMove.TempStopMove (0.25f));
	}

    IEnumerator WaitToDisableWallJump () {
        yield return new WaitForSeconds(0.1f);
        canWallJump = false;
        launchDir = Vector3.zero;
    }

	IEnumerator StopWallJump () {
        stopWallJump = true;
		yield return new WaitForSeconds (0.1f);
        stopWallJump = false;
	}

	IEnumerator StopTurn () {
		playerMove.canTurn = false;
		yield return new WaitForSeconds (0.5f);
		playerMove.canTurn = true;
	}
}
