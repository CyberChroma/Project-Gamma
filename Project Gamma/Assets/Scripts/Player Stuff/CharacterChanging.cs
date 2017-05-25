using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChanging : MonoBehaviour {

	public enum StartCharacter {
		Cube, Sphere, Pyramid
	}
	public StartCharacter startCharacter;

	public GameObject cube; // Reference to the cube
	public Transform cubeCamPos; // Reference to the camera position
	public bool cubeCanChange; // Whether the player can ching into the cube character

	public GameObject sphere; // Reference to the sphere
	public Transform sphereCamPos; // Reference to the camera position
	public bool sphereCanChange; // Whether the player can ching into the sphere character

	public GameObject pyramid; // Reference to the pyramid
	public Transform pyramidCamPos; // Reference to the camera position
	public bool pyramidCanChange; // Whether the player can ching into the pyramid character

	private CameraController cameraController; // Reference to the camera controller

	// Use this for initialization
	void Start () {
		cameraController = GetComponent<CameraController> (); // Getting the reference
		if (startCharacter == StartCharacter.Cube) { // If the active player is the cube
			ActivateCube ();
		} else if (startCharacter == StartCharacter.Sphere) { // If the active player is the sphere
			ActivateSphere ();
		} else { // (If the active player is the pyramid)
			ActivatePyramid ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) { // If the player presses 'q'
			ChangeCharacter ();
		}
	}

	void ChangeCharacter () { // Changes the user to a different character
		if (cameraController.activePlayer == cube) { // If the active player is the cube
			if (sphereCanChange || pyramidCanChange) { // If the user can change into the sphere or the pyramid
				cube.GetComponent<CubeMovement> ().canMove = false; // Deactivating movement
				cube.GetComponent<CubeAbilities> ().canMove = false; // Deactivating abilities
				if (sphereCanChange) { // If the user can change into the sphere
					ActivateSphere ();
				} else if (pyramidCanChange) { // If the user can change into the pyramid
					ActivatePyramid ();
				}
			}
		} else if (cameraController.activePlayer == sphere) { // If the active player is the sphere
			if (pyramidCanChange || cubeCanChange) { // If the user can change into the cube or the pyramid
				sphere.GetComponent<SphereMovement> ().canMove = false; // Deactivating movement
				sphere.GetComponent<SphereAbilities> ().canMove = false; // Deactivating abilities
				if (pyramidCanChange) { // If the user can change into the pyramid
					ActivatePyramid ();
				} else if (cubeCanChange) { // If the user can change into the cube
					ActivateCube ();
				}
			}
		} else { // (If the active player is the pyramid)
			if (cubeCanChange || sphereCanChange) { // If the user can change into the sphere or the cube
				pyramid.GetComponent<PyramidMovement> ().canMove = false; // Deactivating movement
				pyramid.GetComponent<PyramidAbilities> ().canMove = false; // Deactivating abilities
				if (cubeCanChange) { // If the user can change into the cube
					ActivateCube ();
				} else if (sphereCanChange) { // If the user can change into the sphere
					ActivateSphere ();
				}
			}
		}
	}

	void ActivateCube () { // Activates the cube
		cameraController.activePlayer = cube; // Setting the active player
		cube.GetComponent<CubeMovement> ().canMove = true; // Activating movement
		cube.GetComponent<CubeAbilities> ().canMove = true; // Activating abilities
	}

	void ActivateSphere () { // Activates the sphere
		cameraController.activePlayer = sphere; // Setting the active player
		sphere.GetComponent<SphereMovement> ().canMove = true; // Activating movement
		sphere.GetComponent<SphereAbilities> ().canMove = true; // Activating abilities
	}

	void ActivatePyramid () { // Activates the pyramid
		cameraController.activePlayer = pyramid; // Setting the active player
		pyramid.GetComponent<PyramidMovement> ().canMove = true; // Activating movement
		pyramid.GetComponent<PyramidAbilities> ().canMove = true; // Activating abilities
	}
}
