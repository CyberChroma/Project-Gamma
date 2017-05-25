using UnityEngine;
using System.Collections;

public class MovingObjectController : MonoBehaviour {

	public enum ActivationType // Dropdown for how the object's movement is started
	{	// The object start moving...
		StartActive, // Immediately
		Trigger, // When it is triggered (usually by a button)
		CollisionByPlayer, // When the player collides with it
	}

	public enum MotionType // Dropdown for the type of motion
	{	// When the object reaches its last target...
		Loop, // it goes back to the first target
		ForwardAndBack, // it starts going in reverse
		OneTime // it stops
	}

	public enum DeactivationType // Dropdown for what happens when the object's movement is stopped
	{	// When the object stops...
		None, // The object can't be deavtivated
		Freeze, // It just stays it the same position (nothing happens)
		StraightToStart, // It goes in a straight line back to its starting position
		ReverseToStart, // It starts going back to its starting position
	}

	public float moveSpeed; // The movespeed of the object
	public float rotSpeed; // The rotation speed of the object
	public ActivationType activationType; // Stores the selected activation type
	public int triggersRequired; // The number triggers required to activate the moving platform
	public MotionType motionType; // Stores the selected motion type
	public bool ignoreY; // Whether to ignore the y axis (For falling platforms)
	public DeactivationType deactivationType; // Stores the selected activation type
	public float startDelay; // The amount of delay until the object starts moving
	public float nextTargetDelay; // The amount of delay until the object moves towards the next target
	public float stopDelay; // The amount of delay until the object stops moving
	public Transform[] locations; // References to the locations the object moves to

	[HideInInspector] public bool isActive; // Bool for whether the object is moving
	[HideInInspector] public int triggersReceived; // The number of triggers that have been recieved
	[HideInInspector] public Vector3 velocity; // The velocity of the platform
	[HideInInspector] public Quaternion rotVelocity; // The rotational velocity of the platform

	private bool triggered; // Whether the platform has been triggered
	private int targetLocation = 0; // The location the object should be moving towards
	private int increaseAmount = 1; // The amount that the target location increases by
	private bool isDeactivating; // Bool for whether the object is deactivating
	private Rigidbody rb; // Reference to the rigidbody
	private Vector3 lastPos; // The last position of the platform
	private Vector3 lastRot; // The last rotation of the platform

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> (); // Getting the reference
		rb.position = locations [0].position; // Sets the position of the object to its first location's position
		rb.rotation = locations [0].rotation; // Sets the rotation of the object to its first location's rotation
		if (activationType == ActivationType.StartActive) { // If activation type is set to start active
			triggered = true;
			StartCoroutine (ActivateMovement ());
		} else {
			triggered = false;
			isActive = false; // The object does not start moving
			triggersReceived = 0;
		}
		lastPos = rb.position; // Setting the last position
		lastRot = rb.rotation.eulerAngles; // Setting the last rotation
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isActive) { // If the object should be moving
			if (ignoreY) { // if the y axis sould be ignored
				// Moves the position of the object towards its target's position (ignoring y axis)
				rb.position = Vector3.MoveTowards (rb.position, new Vector3(locations [targetLocation].position.x, rb.position.y, locations [targetLocation].position.z), moveSpeed / 10);
			} else {
				// Moves the position of the object towards its target's position
				rb.position = Vector3.MoveTowards (rb.position, locations [targetLocation].position, moveSpeed / 10);
			}
			// Moves the rotation of the object towards its target's rotation
			rb.rotation = Quaternion.RotateTowards (rb.rotation, locations [targetLocation].rotation, rotSpeed / 10);
			if (ignoreY) { // If the y axis should be ignored
				// If the position and rotation of the object is equal to the position and rotation of its target (ignoring y axis position)
				if (Vector3.Distance (rb.position, new Vector3(locations [targetLocation].position.x, rb.position.y, locations [targetLocation].position.z)) <= 0.01f && Quaternion.Angle (rb.rotation, locations [targetLocation].rotation) <= 0.01f) {
					StartCoroutine (GetNextTarget ());
				}
			} else {
				// If the position and rotation of the object is equal to the position and rotation of its target
				if (Vector3.Distance (rb.position, locations [targetLocation].position) <= 0.01f && Quaternion.Angle (rb.rotation, locations [targetLocation].rotation) <= 0.01f) {
					StartCoroutine (GetNextTarget ());
				}
			}
		}
		if (Time.deltaTime != 0) { // If the time scale is not zero
			velocity = (rb.position - lastPos) / Time.deltaTime / 50; // Calculating the velocity
			lastPos = rb.position; // Setting the last position
			rotVelocity = Quaternion.Euler((rb.rotation.eulerAngles - lastRot) / Time.deltaTime / 50); // Calculating the rotational velocity
			lastRot = rb.rotation.eulerAngles; // Setting the last rotation
		}
		if (triggersReceived == triggersRequired && activationType == ActivationType.Trigger && !isActive && !triggered) {
			// If the platform has recieved enough triggers and the activation type is trigger anf the platform is not currently active and the platform has not been triggered already
			triggered = true; // Setting the bool
			StartCoroutine (ActivateMovement ());
		}
	}

	IEnumerator ActivateMovement () { // Activates movement
		yield return new WaitForSeconds (startDelay); // Creates a delay for when the object starts moving
		if (triggered) { // If the object has been triggered
			isActive = true; // Activating movement
			isDeactivating = false; // Activating movement
			if (increaseAmount == -1) {
				increaseAmount = 1; // Makes the increase amount negative
				targetLocation += increaseAmount; // Start moving towards the next target
			}
		}
	}

	IEnumerator GetNextTarget () { // Gets the next target the object should move to or stops it
		isActive = false; // Disabling movement while the object is getting the next target
		yield return new WaitForSeconds (nextTargetDelay); // Creates a delay for when the object starts moving again
		if (isDeactivating && targetLocation == 0) { // If the object is deactivating and it has reached the starting position
			increaseAmount = 1; // Makes the increase amount positive again
			targetLocation = 1; // Sets the target location to 1
			isActive = false; // Disables movement
		} else {
			targetLocation += increaseAmount; // Start moving towards the next target
			if (targetLocation < 0) { // If the target location less than 0 (from going down from forward and back motion)
				increaseAmount = 1; // Makes the increase amount positive again
				targetLocation = 1; // Sets the target location to 1
			}
			isActive = true; // Reactivating movement
			if (targetLocation >= locations.Length) { // If the object is at the last target
				if (motionType == MotionType.Loop) { // If the selected motion type is loop
					targetLocation = 0; // Start moving back toward the first target
				} else if (motionType == MotionType.ForwardAndBack) { // If the selected motion type is forward and back
					increaseAmount = -1; // Makes the increase amount negative
					targetLocation += increaseAmount; // Start moving towards the next target
				} else { // If the selected motion type is one time
					targetLocation = locations.Length - 1; // Sets the target location back within range to avoid errors
					isActive = false; // Disables movement
				}
			}
		}
	}

	IEnumerator DeactivateMovement () {  // Deactivates the movement depending on the deactivation type
		yield return new WaitForSeconds (stopDelay); // Creates a delay for when the object stops moving
		if (!triggered) { // If the object is not triggered
			isDeactivating = true; // Starts deactivating the object's movement
			if (deactivationType == DeactivationType.None) { // If the deactivation type is set to none
				isDeactivating = false; // The object should not be stopping
			} else if (deactivationType == DeactivationType.Freeze) { // If the deactivation type is set to freeze
				isActive = false; // Simply disables movement
			} else if (deactivationType == DeactivationType.StraightToStart) { // If the decativation type is set to straight to start
				targetLocation = 0; // Sets the target location to the starting position of the object
			} else { // (If the activation type is reverse to start)
				if (increaseAmount == 1) {
					increaseAmount = -1; // Starts moving in the opposite direction
					targetLocation -= 1; // Starts moving towards the last location
				}
				if (targetLocation < 0) {
					targetLocation = 0; // Setting it to the start location
				}
			} 
		}
	}

	public void TestForActivate () { // Tests if another object is colliding with the object
		// If the object collided with a player and the activation type is set to collision by player or the activation type is set to collision by any
		if (activationType == ActivationType.CollisionByPlayer && !triggered) { // If the activation type is collision by player and the object has not been triggered
			triggered = true; // Setting the bool
			StopCoroutine ("DeactivateMovement");
			StartCoroutine ("ActivateMovement");
		}
	}

	public void TestForDeactivate () {
		if (isActive) {
			if (activationType == ActivationType.CollisionByPlayer && triggered) {
				triggered = false; // Setting the bool
				StartCoroutine ("DeactivateMovement");
			}
		}
	}
}