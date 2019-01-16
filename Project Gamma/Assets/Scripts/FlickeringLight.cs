using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour {

    public float onTime = 2;
    public float offTime = 2;

    private Light light;

	// Use this for initialization
	void Awake () {
        light = GetComponent<Light>();
        StartCoroutine(WaitToDeactivate());
	}

    IEnumerator WaitToActivate () {
        yield return new WaitForSeconds(offTime);
        light.enabled = true;
        StartCoroutine(WaitToDeactivate());
    }

    IEnumerator WaitToDeactivate () {
        yield return new WaitForSeconds(onTime);
        light.enabled = false;
        StartCoroutine(WaitToActivate());
    }
}
