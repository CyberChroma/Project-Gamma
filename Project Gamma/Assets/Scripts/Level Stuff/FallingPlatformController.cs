using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformController : MonoBehaviour {

	public float fallSpeed;
	public float riseSpeed;
	public float riseDelay;

	private bool falling;
	private float startHeight;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		startHeight = transform.position.y;
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (falling) {
			rb.velocity = Vector3.down * fallSpeed;
		} else {
			if (transform.position.y >= startHeight) {
				rb.velocity = Vector3.zero;
			} else {
				rb.velocity = Vector3.up * riseSpeed;
			}
		}
	}

	public void Fall () {
		StopCoroutine ("DelayToRise");
		falling = true;
	}

	public void Rise () {
		StartCoroutine ("DelayToRise");
	}

	IEnumerator DelayToRise () {
		yield return new WaitForSeconds (riseDelay);
		falling = false;
	}
}
