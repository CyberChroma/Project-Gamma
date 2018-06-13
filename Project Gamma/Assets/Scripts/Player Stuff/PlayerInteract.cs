﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {

    private Health health;

    void OnCollisionEnter (Collision other) {
        if (other.gameObject.CompareTag("Button"))
        {
            if (other.gameObject.GetComponent<ActivateFollowTarget>())
            {
                other.gameObject.GetComponent<ActivateFollowTarget>().Activate(); // Activates the button
            }
            if (other.gameObject.GetComponent<ActivateFallOnActivate>())
            {
                other.gameObject.GetComponent<ActivateFallOnActivate>().Activate(); // Activates the button
            }
        }
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Hazard")) 
        {
            health.Damage();
        }
    }
}
