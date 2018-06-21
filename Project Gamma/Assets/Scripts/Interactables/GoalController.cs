using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour {

    private GameManager gameManager;

	// Use this for initialization
	void Awake () {
        gameManager = FindObjectOfType<GameManager>();
	}

    void OnTriggerEnter (Collider other) {
        if (other.CompareTag("Player"))
        {
            gameManager.LevelComplete();
        }
    }
}
