using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour {

	public Text lGText;
	public Text gFText;
	public Text gMText;

	private float numLG = 0;
	private float numGF = 0;
	private float numGM = 0;

	// Use this for initialization
	void Start () {
		lGText.text = "Light Gears: " + numLG;
		gFText.text = "Gear Fragments: " + numGF;
		gMText.text = "Geomorphs: " + numGM;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void IncreaseLightGears () {
		numLG++;
		lGText.text = "Light Gears: " + numLG;
	}

	public void IncreaseGearFragments () {
		numGF++;
		gFText.text = "Gear Fragments: " + numGF;
	}

	public void IncreaseGeomorph () {
		numGM++;
		gMText.text = "Geomorphs: " + numGM;
	}
}
