using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour {

	public Transform[] objectsToTrigger; // References to the objects who's movements will be triggered when the button is pressed

	private bool isPressed; // Bool for whether the button has been pressed yet
	private Animator anim; // Reference to the animation component

	// Use this for initialization
	void Start () {
		isPressed = false;
		anim = GetComponentInParent <Animator> (); // Getting the reference
	}

	public void Activate () {
		if (!isPressed) {
			anim.SetTrigger ("Activate"); // Makes the button pressed
			for (int i = 0; i <= objectsToTrigger.Length - 1; i++) {
				if (objectsToTrigger [i].GetComponent<MovingObjectController> ()) {
					MovingObjectController mOC = objectsToTrigger [i].GetComponent<MovingObjectController> (); // Gets the reference to the object's moving object controller script
					mOC.triggersReceived++; // Activates the object's movement
				} else if (objectsToTrigger [i].GetComponent<BoostController> ()) {
					BoostController bC = objectsToTrigger [i].GetComponent<BoostController> ();
					bC.Activate ();
				}
				isPressed = true;
			}	
		}
	}
}
