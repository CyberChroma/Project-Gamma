using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanController : MonoBehaviour {

    public float force;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay (Collider other) {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody>().AddForce(Vector3.up * force * 10);
        }
    }
}
