using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOnActivate : MonoBehaviour {

    public float delay = 1;
    public float rumbleAmount = 0.1f;
    public float fallTime = 2;
    public bool rise;
    public float riseSmoothing;

    private bool rumbling;
    private bool falling;
    private bool rising;
    private Vector3 startPos;
    private Quaternion startRot;
    private Rigidbody rb;

    // Use this for initialization
    void Awake () {
        rumbling = false;
        falling = false;
        rising = false;
        startPos = transform.position;
        startRot = transform.rotation;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void FixedUpdate () {
        if (rise && rising)
        {
            rb.position = Vector3.Lerp(rb.position, startPos, riseSmoothing);
            if (Vector3.Distance(rb.position, startPos) <= 0.1f)
            {
                rb.position = startPos;
                rising = false;
            }
        }
        if (rumbling)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(startPos.x + Random.Range(-rumbleAmount, rumbleAmount), startPos.y + Random.Range(-rumbleAmount, rumbleAmount), startPos.z + Random.Range(-rumbleAmount, rumbleAmount)), 0.5f);
        }
    }

    public void Activate () {
        if (!falling)
        {
            rumbling = true;
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall () {
        yield return new WaitForSeconds(delay);
        rumbling = false;
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        falling = true;
        yield return new WaitForSeconds(fallTime);
        rb.isKinematic = true;
        falling = false;
        rising = true;
        transform.rotation = startRot;
    }
}
