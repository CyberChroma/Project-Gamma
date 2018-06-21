using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public float delayToStart = 1;
    public float fadeInSpeed = 0.5f;
    public Image blackScreen;

    private bool fadeIn;
    private CheckpointManager checkpointManager;
    private CameraController cameraController;
    private InputManager inputManager;
    private GameObject cube;
    private PlayerLifeAndDeath playerLifeAndDeath;

	void Awake () {
        checkpointManager = FindObjectOfType<CheckpointManager>();
        cameraController = FindObjectOfType<CameraController>();
        cube = GameObject.Find("Cube Character");
        playerLifeAndDeath = cube.GetComponent<PlayerLifeAndDeath>();
        inputManager = FindObjectOfType<InputManager>();
        blackScreen.gameObject.SetActive(true);
        fadeIn = false;
        StartCoroutine(WaitToStart());
	}

    void Update () {
        if (fadeIn && blackScreen.color != new Color(0, 0, 0, 0))
        {
            blackScreen.color = Color.Lerp(blackScreen.color, new Color(0, 0, 0, 0), fadeInSpeed);
        } else if (!fadeIn && blackScreen.color != new Color(0, 0, 0, 1)) {
            blackScreen.color = Color.Lerp(blackScreen.color, new Color(0, 0, 0, 1), fadeInSpeed);
        }
    }

    IEnumerator WaitToStart () {
        cameraController.enabled = false;
        cube.GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine(inputManager.TempDisable(delayToStart));
        fadeIn = true;
        yield return new WaitForSeconds(delayToStart);
        cameraController.enabled = true;
        cube.GetComponent<Rigidbody>().isKinematic = false;
        checkpointManager.SetSpawn(cube.transform.transform, checkpointManager.checkpoints[0].transform);
        playerLifeAndDeath.Die();
    }

    public void LevelComplete () {        
        StartCoroutine(WaitToEnd());
    }

    IEnumerator WaitToEnd () {
        cameraController.enabled = false;
        cube.GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine(inputManager.TempDisable(Mathf.Infinity));
        yield return new WaitForSeconds(1);
        fadeIn = false;
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Level Select");
    }
}
