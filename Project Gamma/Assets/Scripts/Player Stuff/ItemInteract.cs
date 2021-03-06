using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteract : MonoBehaviour {

	private ItemManager itemManager;

	// Use this for initialization
	void Awake () {
		itemManager = GameObject.Find ("Item Manager").GetComponent<ItemManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider other) {
        if (other.CompareTag ("Gear Fragment")) {
			itemManager.IncreaseGearFragments ();
			other.gameObject.SetActive (false);
		}
	}
}
