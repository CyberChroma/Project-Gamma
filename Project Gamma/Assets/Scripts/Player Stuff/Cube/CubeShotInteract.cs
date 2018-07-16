using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeShotInteract : MonoBehaviour {
    
    void OnTriggerStay (Collider other) {
        if (other.gameObject.CompareTag("Button"))
        {
            if (other.GetComponent<ActivateFollowTarget>() && other.GetComponent<ActivateFollowTarget>().type == ActivateFollowTarget.Type.target)
            {
                other.GetComponent<ActivateFollowTarget>().Activate(); // Activates the button
            }
            if (other.GetComponent<ActivateFallOnActivate>() && other.GetComponent<ActivateFallOnActivate>().type == ActivateFallOnActivate.Type.target)
            {
                other.GetComponent<ActivateFallOnActivate>().Activate(); // Activates the button
            }
            if (other.GetComponent<ActivateGameObject>() && other.GetComponent<ActivateGameObject>().type == ActivateGameObject.Type.target)
            {
                other.GetComponent<ActivateGameObject>().Activate(); // Activates the button
            }
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Enemy")) 
        {
            //Damage();
        }
    }
}
