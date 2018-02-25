using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAndDestroyEffects : MonoBehaviour {

    public GameObject spawnParticles;
    public GameObject deathParticles;

    private Transform parent;

    void Awake () {
        parent = GameObject.Find("Shots").transform;
    }

    void OnEnable () {
        if (spawnParticles) {
            Instantiate(spawnParticles, transform.position, Quaternion.identity, parent);
        }
    }

    void OnDisable () {
        if (deathParticles) {
            Instantiate(deathParticles, transform.position, Quaternion.identity, parent);
        }
    }
}
