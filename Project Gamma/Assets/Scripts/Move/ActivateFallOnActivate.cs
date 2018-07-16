using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateFallOnActivate : MonoBehaviour {

    public enum Type
    {
        floor,
        target
    }

    public Type type;
    public FallOnActivate[] objectsToTrigger; // References to the objects that will be activated
	public bool oneTime;

	private bool isActivated; // Bool for whether the objects have been activated

	// Use this for initialization
	void Awake () {
		isActivated = false; // Setting the bool
	}

	public void Activate () {
		if (!oneTime || (oneTime && !isActivated)) {
            foreach (FallOnActivate triggerObject in objectsToTrigger) { // Goes through each object that must be activated
                triggerObject.Activate(); // Enables the script
			}
			isActivated = true;
		}
	}
}
