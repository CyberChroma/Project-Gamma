using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {

	public bool lockCursor;
	public GameObject pauseScreen; // Reference to the pause screen panel
	public GameObject controlsScreen; // Reference to the control screen panel

	private bool isPaused; // Bool for if the game is paused

	// Use this for initialization
	void Start () {
		Resume ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape) && !isPaused) { // Getting input to pause the game and making sure the game is not already paused
			Pause ();
		} 
	}

	void OnApplicationPause () { // Runs when the application is paused
		Pause ();
	}

	void Pause () {
		pauseScreen.SetActive (true); // Activates the pause screen panel
		Time.timeScale = 0; // Freezes time
		if (lockCursor) { // If the cursor should be locked
			Cursor.lockState = CursorLockMode.None; // Unlocks the cursor
		}
		isPaused = true; // Setting the bool
	}

	public void Resume () {
		controlsScreen.SetActive (false); // Deactivates the control screen panel
		pauseScreen.SetActive (false); // Deactivates the pause screen panel
		if (lockCursor) { // If the cursor should be locked
			Cursor.lockState = CursorLockMode.Locked; // Unlocks the cursor
		}
		Time.timeScale = 1; // Unfreezes time
		isPaused = false; // Setting the bool

	}

	public void Controls () {
		controlsScreen.SetActive (true); // Activates the control screen panel
		pauseScreen.SetActive (false); // Deactivates the pause screen panel
	}

	public void Back () {
		controlsScreen.SetActive (false); // Deactivates the control screen panel
		pauseScreen.SetActive (true); // Activates the pause screen panel
	}

	public void Quit () {
		Application.Quit (); // Quits the game
	}
}
