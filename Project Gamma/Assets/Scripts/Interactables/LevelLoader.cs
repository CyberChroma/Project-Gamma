using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour {

    public string levelToLoad;

    private GameManager gameManager;

	// Use this for initialization
	void Awake () {
        gameManager = FindObjectOfType<GameManager>();
	}

    void OnTriggerEnter (Collider other) {
        gameManager.EndLevel(levelToLoad);
    }
}
