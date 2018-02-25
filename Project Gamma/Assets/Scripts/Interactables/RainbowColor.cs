using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class RainbowColor : MonoBehaviour {

	public float changeColourDelay = 1;

	private Color newColour;
	private Renderer rendMat;

	// Use this for initialization
	void Start () {
		rendMat = GetComponent<Renderer> ();
		ChangeColour ();
		rendMat.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.75f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		rendMat.material.color = Color.Lerp (rendMat.material.color, newColour, 0.05f * Time.timeScale);
	}

	void ChangeColour () {
		newColour = Random.ColorHSV(0f, 1f, 1f, 1f, 0.75f, 1f);
		StartCoroutine (WaitToChangeColour ());
	}

	IEnumerator WaitToChangeColour () {
		yield return new WaitForSeconds (changeColourDelay);
		ChangeColour ();
	}
}
