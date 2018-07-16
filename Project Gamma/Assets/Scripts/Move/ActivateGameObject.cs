using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateGameObject : MonoBehaviour {

    public enum Type
    {
        floor,
        target
    }

    public Type type;
    public GameObject[] objectsToTrigger; // References to the objects that will be activated
	public bool oneTime;

	private bool isActivated; // Bool for whether the objects have been activated
	private Animator anim; // Reference to the buttons animation component

	// Use this for initialization
	void Awake () {
		isActivated = false; // Setting the bool
		anim = GetComponentInParent<Animator>(); // Getting the reference
		foreach (GameObject triggerObject in objectsToTrigger) { // Goes through each object that must be activated
            triggerObject.SetActive(false);
		}
	}

	public void Activate () {
		if (!oneTime || (oneTime && !isActivated)) {
			if (anim) {
				anim.SetTrigger ("Activate"); // Plays the button press animation
			}
            foreach (GameObject triggerObject in objectsToTrigger) { // Goes through each object that must be activated
                triggerObject.SetActive(true); // Enables the script
			}
			isActivated = true;
		}
	}
}
