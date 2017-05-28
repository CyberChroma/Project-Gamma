using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public float normalMusicVolume; // The normal volume of the background music
	public float pauseMusicVolume; // The volume of the music when the game is paused

	private AudioSource source; // Reference to the audio source playing the background music
	private PauseManager pauseManager; // Reference to the pause manager

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> (); // Getting the reference
		pauseManager = GameObject.Find ("Pause Manager").GetComponent<PauseManager> (); // Getting the reference
	}

	// Update is called once per frame
	void Update () {
		if (pauseManager.isPaused) { // if the game is paused
			source.volume = Mathf.Lerp (source.volume, pauseMusicVolume, 0.5f); // Brings the volume down
		} else {
			source.volume = Mathf.Lerp (source.volume, normalMusicVolume, 0.5f); // Brings the volume up
		}
	}
}
