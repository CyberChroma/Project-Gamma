﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour {

	public Text gFText;

	private float numGF = 0;

	// Use this for initialization
	void Start () {
		gFText.text = "Fuel Cells: " + numGF;
	}

	public void IncreaseGearFragments () {
		numGF++;
		gFText.text = "Fuel Cells: " + numGF;
	}
}
