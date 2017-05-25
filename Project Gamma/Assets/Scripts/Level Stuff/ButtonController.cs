using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour {

	public Transform[] objectsToTrigger; // References to the objects that will be activated when the button is pressed

	private bool isPressed; // Bool for whether the button has been pressed yet
	private Animator anim; // Reference to the buttons animation component

	// Use this for initialization
	void Start () {
		isPressed = false; // Setting the bool
		anim = GetComponentInParent <Animator> (); // Getting the reference
	}

	public void Activate () {
		if (!isPressed) {
			anim.SetTrigger ("Activate"); // Plays the button press animation
			for (int i = 0; i <= objectsToTrigger.Length - 1; i++) { // Goes through each object that must be activated
				if (objectsToTrigger [i].GetComponent<MovingObjectController> ()) { // If the object is amoving platform
					objectsToTrigger [i].GetComponent<MovingObjectController> ().triggersReceived++; // Gets the reference to the object's moving object controller script and activates the object's movement
				} else if (objectsToTrigger [i].GetComponent<BoostController> ()) { // If the object is a boost
					objectsToTrigger [i].GetComponent<BoostController> ().Activate (); // Gets the reference to the object's boost controller script and activates the boost
				} else if (objectsToTrigger [i].GetComponent<TurningPointController> ()) { // If the object is a turning point
					objectsToTrigger [i].GetComponent<TurningPointController> ().Activate (); // Gets the reference to the object's turning point controller script and activates the turn point
				}
				isPressed = true; // Setting the bool
			}
		}
	}
}
