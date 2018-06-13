using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public GameObject parent; // The object parent (to destroy)
    public float startHealth = 1; // The health the object starts with
    public float tempStopHitsTime = 0.1f; // Time between hits
    public bool disableOnDeath = false;
    public float disableDelay = 1;

    [HideInInspector] public bool healthChanged; // UI updates when true
    [HideInInspector] public float currentHealth; // The current health
    [HideInInspector] public bool canBeHit = true; // Whether the object can be hit
    private bool dead = false;
    private Rigidbody rb;
    private Animator anim;

    // Use this for initialization
    void Start () {
        // Setting start values
        currentHealth = startHealth;
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    public void Damage () {
        if (canBeHit) { // If the object can be hit
            currentHealth--; // Lose health
            healthChanged = true; // Updates the UI
            if (gameObject.activeSelf) {
                StartCoroutine (TempStopHits ()); // Temporarily stops the object from taking damage
            }
            healthChanged = true; // Updates the UI
            if (currentHealth <= 0 && !dead) { // If the object has no health left
                if (rb) {
                    rb.isKinematic = true;
                }
                if (anim) {
                    anim.SetTrigger("Death");
                }
                if (disableOnDeath) {
                    StartCoroutine (Disable ());
                }
                dead = true;
            }
        }
    }

    public IEnumerator TempStopHits () { // Temporarily stops the object from taking more damage
        canBeHit = false;
        yield return new WaitForSeconds (tempStopHitsTime);
        canBeHit = true;
    }

    IEnumerator Disable () {
        yield return new WaitForSeconds(disableDelay);
        gameObject.SetActive(false);
    }
}
