using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChanging : MonoBehaviour {

	public enum StartCharacter {
		Cube, Sphere, Pyramid
	}
	public StartCharacter startCharacter;

	public GameObject cube; // Reference to the cube
	public Transform cubeCamPos;
	public bool cubeCanChange;

	public GameObject sphere; // Reference to the sphere
	public Transform sphereCamPos;
	public bool sphereCanChange;

	public GameObject pyramid; // Reference to the pyramid
	public Transform pyramidCamPos;
	public bool pyramidCanChange;

	private CameraController cameraController;

	// Use this for initialization
	void Start () {
		cameraController = GetComponent<CameraController> (); // Getting the reference
		if (startCharacter == StartCharacter.Cube) {
			ActivateCube ();
		} else if (startCharacter == StartCharacter.Sphere) {
			ActivateSphere ();
		} else {
			ActivatePyramid ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			ChangeCharacter ();
		}
	}

	void ChangeCharacter () {
		if (cameraController.activePlayer == cube) {
			if (sphereCanChange || pyramidCanChange) {
				cube.GetComponent<CubeMovement> ().canMove = false;
				cube.GetComponent<CubeAbilities> ().canMove = false;
				if (sphereCanChange) {
					ActivateSphere ();
				} else if (pyramidCanChange) {
					ActivatePyramid ();
				}
			}
		} else if (cameraController.activePlayer == sphere) {
			if (cubeCanChange || pyramidCanChange) {
				sphere.GetComponent<SphereMovement> ().canMove = false;
				sphere.GetComponent<SphereAbilities> ().canMove = false;
				if (pyramidCanChange) {
					ActivatePyramid ();
				} else if (cubeCanChange) {
					ActivateCube ();
				}
			}
		} else {
			if (sphereCanChange || cubeCanChange) {
				pyramid.GetComponent<PyramidMovement> ().canMove = false;
				pyramid.GetComponent<PyramidAbilities> ().canMove = false;
				if (cubeCanChange) {
					ActivateCube ();
				} else if (sphereCanChange) {
					ActivateSphere ();
				}
			}
		}
	}

	void ActivateCube () {
		cameraController.activePlayer = cube;
		cube.GetComponent<CubeMovement> ().canMove = true;
		cube.GetComponent<CubeAbilities> ().canMove = true;
	}

	void ActivateSphere () {
		cameraController.activePlayer = sphere;
		sphere.GetComponent<SphereMovement> ().canMove = true;
		sphere.GetComponent<SphereAbilities> ().canMove = true;
	}

	void ActivatePyramid () {
		cameraController.activePlayer = pyramid;
		pyramid.GetComponent<PyramidMovement> ().canMove = true;
		pyramid.GetComponent<PyramidAbilities> ().canMove = true;
	}
}
