using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {

	public bool lockCursor; // Whether the cursor is locked (For development purposes)
	public GameObject pauseScreen; // Reference to the pause screen panel
	public GameObject controlsScreen; // Reference to the control screen panel

	[HideInInspector] public bool isPaused; // Bool for if the game is paused

	// Use this for initialization
	void Start () {
		Resume ();
	}

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown (KeyCode.Escape)) {
            if (isPaused) {
                Resume();
            } else {
			    Pause ();
            }
		} 
	}

	void OnApplicationPause () { // Runs when the application is paused
		Pause ();
	}

	void Pause () { // Pausing the game
		pauseScreen.SetActive (true); // Activates the pause screen panel
		Time.timeScale = 0; // Freezes time
		if (lockCursor) { // If the cursor should be locked
			Cursor.lockState = CursorLockMode.None; // Unlocks the cursor
            Cursor.visible = true;
		}
		isPaused = true; // Setting the bool
	}

	public void Resume () { // Resuming the game
		controlsScreen.SetActive (false); // Deactivates the control screen panel
		pauseScreen.SetActive (false); // Deactivates the pause screen panel
		if (lockCursor) { // If the cursor should be locked
			Cursor.lockState = CursorLockMode.Locked; // Unlocks the cursor
            Cursor.visible = false;
		}
		Time.timeScale = 1; // Unfreezes time
		isPaused = false; // Setting the bool

	}

	public void Controls () { // Bringing up the controls screen
		controlsScreen.SetActive (true); // Activates the control screen panel
		pauseScreen.SetActive (false); // Deactivates the pause screen panel
	}

	public void Back () { // Going back to the pause screen
		controlsScreen.SetActive (false); // Deactivates the control screen panel
		pauseScreen.SetActive (true); // Activates the pause screen panel
	}

	public void Quit () { // Quitting the game
		Application.Quit (); // Quits the game
	}
}
