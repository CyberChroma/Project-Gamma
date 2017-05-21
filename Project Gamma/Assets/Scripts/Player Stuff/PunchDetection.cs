using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchDetection : MonoBehaviour {
	
	private CubeAbilities cubeAbilities;

	void Start () {
		cubeAbilities = GetComponentInParent<CubeAbilities> ();
	}

	void OnTriggerStay (Collider other) {
		if (cubeAbilities.isPunching) {
			if (other.CompareTag ("Breakable")) {
				other.GetComponentInChildren<Animator> ().SetTrigger ("Activate");
			} else if (other.CompareTag ("Button-Cube")) {
				other.GetComponent<ButtonController> ().Activate ();
			}
		}
	}
}
