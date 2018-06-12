using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOnCollision : MonoBehaviour {

    public float delay = 1;
    public float fallTime = 2;
    public bool rise;
    public float riseSmoothing;

    private bool falling;
    private Vector3 startPos;
    private Rigidbody rb;

	// Use this for initialization
	void Awake () {
        falling = false;
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
	}

    void FixedUpdate () {
        if (rise && !falling) {
            rb.position = Vector3.Lerp(rb.position, startPos, riseSmoothing);
            if (Vector3.Distance(rb.position, startPos) <= 0.1f)
            {
                rb.position = startPos;
            }
        }
    }

    void OnCollisionEnter (Collision other) {
        if (other.collider.CompareTag("Player") && !falling)
        {
            falling = true;
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall () {
        yield return new WaitForSeconds(delay);
        rb.isKinematic = false;
        yield return new WaitForSeconds(fallTime);
        rb.isKinematic = true;
        falling = false;
    }
}
