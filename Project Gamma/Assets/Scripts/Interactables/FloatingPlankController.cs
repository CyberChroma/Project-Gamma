using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlankController : MonoBehaviour {

    public float speed = 10;
    public float gravityMultiplier = 1;
    public float delay = 1;
    public float fallSmoothing = 0.1f;

    private bool falling = false;
    private Rigidbody rb;

	// Use this for initialization
	void Awake () {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (falling)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, -transform.right * speed + Physics.gravity * gravityMultiplier, fallSmoothing);
        }
        else
        {
            rb.velocity = -transform.right * speed;
        }
	}

    void OnCollisionEnter (Collision other) {
        if (other.collider.CompareTag("Environment"))
        {
            StartCoroutine(Fall());
        }
    }

    void OnTriggerEnter (Collider other) {
        if (other.CompareTag("Environment"))
        {
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall () {
        yield return new WaitForSeconds(delay);
        falling = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
}
