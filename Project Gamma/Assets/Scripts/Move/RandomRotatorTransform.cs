using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotatorTransform : MonoBehaviour {

    public float smoothing = 1;

    // Update is called once per frame
    void FixedUpdate () {
        transform.rotation = Quaternion.Slerp(transform.rotation, Random.rotation, smoothing); // Setting the rotational velocity
    }
}
