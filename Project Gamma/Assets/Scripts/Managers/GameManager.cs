using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public float delayToStart = 1;
    public float fadeInSpeed = 0.5f;
    public Image blackScreen;

    private CheckpointManager checkpointManager;
    private CameraController cameraController;
    private GameObject cube;
    private PlayerLifeAndDeath playerLifeAndDeath;

	void Awake () {
        checkpointManager = FindObjectOfType<CheckpointManager>();
        cameraController = FindObjectOfType<CameraController>();
        cube = GameObject.Find("Cube Character");
        playerLifeAndDeath = cube.GetComponent<PlayerLifeAndDeath>();
        blackScreen.gameObject.SetActive(true);
        StartCoroutine(WaitToStart());
	}

    void Update () {
        if (blackScreen.color != new Color(0, 0, 0, 0))
        {
            blackScreen.color = Color.Lerp(blackScreen.color, new Color(0, 0, 0, 0), fadeInSpeed);
        }
    }

    IEnumerator WaitToStart () {
        cameraController.enabled = false;
        cube.GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitForSeconds(delayToStart);
        cameraController.enabled = true;
        cube.GetComponent<Rigidbody>().isKinematic = false;
        checkpointManager.SetSpawn(cube.transform.transform, checkpointManager.checkpoints[0].transform);
        playerLifeAndDeath.Die();
    }
}
