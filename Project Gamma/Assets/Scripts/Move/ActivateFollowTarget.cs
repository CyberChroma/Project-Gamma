using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateFollowTarget : MonoBehaviour {

	public FollowTargetChangeOnReach[] objectsToTrigger; // References to the objects that will be activated
	public bool oneTime;

	private bool isActivated; // Bool for whether the objects have been activated
	private Animator anim; // Reference to the buttons animation component

	// Use this for initialization
	void Awake () {
		isActivated = false; // Setting the bool
		anim = GetComponentInParent<Animator>(); // Getting the reference
		foreach (FollowTargetChangeOnReach triggerObject in objectsToTrigger) { // Goes through each object that must be activated
			triggerObject.enabled = false; // Enables the script
		}
	}

	public void Activate () {
		if (!oneTime || (oneTime && !isActivated)) {
			if (anim) {
				anim.SetTrigger ("Activate"); // Plays the button press animation
			}
			foreach (FollowTargetChangeOnReach triggerObject in objectsToTrigger) { // Goes through each object that must be activated
				triggerObject.enabled = true; // Enables the script
			}
			isActivated = true;
		}
	}
}
