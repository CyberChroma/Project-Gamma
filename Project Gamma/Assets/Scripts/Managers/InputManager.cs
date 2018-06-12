using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	public KeyCode moveForward = KeyCode.W; // The key to move forward
	public KeyCode moveBack = KeyCode.S; // The key to move back
	public KeyCode moveLeft = KeyCode.A; // The key to move left
	public KeyCode moveRight = KeyCode.D; // The key to move right

	public KeyCode jump = KeyCode.Space; // The button to jump
	public KeyCode mainAttack = KeyCode.LeftShift; // The key to use main attack
	public KeyCode ability1 = KeyCode.Mouse0; // The key to use ability 1
	public KeyCode ability2 = KeyCode.Mouse1; // The key to use ability 2

	public KeyCode interact = KeyCode.Q; // The key to interact
	public KeyCode change = KeyCode.E; // The key to change characters

	public KeyCode pause = KeyCode.Escape;  // The key to pause the game

	[HideInInspector] public bool inputMF; // Bool for whether the player is pressing the button
	[HideInInspector] public bool inputMB; // Bool for whether the player is pressing the button
	[HideInInspector] public bool inputML; // Bool for whether the player is pressing the button
	[HideInInspector] public bool inputMR; // Bool for whether the player is pressing the button

	[HideInInspector] public bool inputJ; // Bool for whether the player is pressing the button
	[HideInInspector] public bool inputJD; // Bool for whether the player is pressing the button

	[HideInInspector] public bool inputMA; // Bool for whether the player is pressing the button
	[HideInInspector] public bool inputA1; // Bool for whether the player is pressing the button
	[HideInInspector] public bool inputA2; // Bool for whether the player is pressing the button

	[HideInInspector] public bool inputI; // Bool for whether the player is pressing the button
	[HideInInspector] public bool inputC; // Bool for whether the player is pressing the button

	[HideInInspector] public bool inputP; // Bool for whether the player is pressing the button

	private bool canMove;

	void Start () {
		canMove = true;
	}

	// Update is called once per frame
	void Update () {
		if (canMove) { // If the player can move
			inputMF = Input.GetKey (moveForward); // Getting input for moving forward
			inputMB = Input.GetKey (moveBack); // Getting input for moving back
			inputML = Input.GetKey (moveLeft); // Getting input for moving left
			inputMR = Input.GetKey (moveRight); // Getting input for moving right

			inputJ = Input.GetKey (jump); // Getting input for using ability
			inputJD = Input.GetKeyDown (jump); // Getting input for using ability

			inputMA = Input.GetKeyDown (mainAttack); // Getting input for using ability
			inputA1 = Input.GetKeyDown (ability1); // Getting input for using ability
			inputA2 = Input.GetKeyDown (ability2); // Getting input for using ability

			inputI = Input.GetKeyDown (interact); // Getting input for interacting
			inputC = Input.GetKeyDown (change); // Getting input for changing
		} else { // If they can't, reset all input
			inputMF = false;
			inputMB = false;
			inputML = false;
			inputMR = false;

            inputJ = false;
            inputJD = false;

			inputMA = false;
			inputA1 = false;
			inputA2 = false;

			inputI = false;
			inputC = false;
		}
		inputP = Input.GetKeyDown (pause); // Getting input for pausing
	}

	public IEnumerator TempDisable (float delay) { // Temporarily disables the player's movement
		canMove = false; // Setting the bool
		yield return new WaitForSeconds (delay); // Waits...
		canMove = true; // Setting the bool
	}
}
