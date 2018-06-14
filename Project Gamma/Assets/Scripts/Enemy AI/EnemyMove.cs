using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour {

	public float moveSensitivity = 0.1f; // Used to make the enemy movement less snappy

    private float radius = 10;
	private MoveByForce enemyMove; // Reference to the move script
	private Transform player; // Reference to the player
    private float moveSpeed;

	// Use this for initialization
	void Awake () {
        enemyMove = GetComponent<MoveByForce> (); // Getting the reference
		player = GameObject.Find ("Cube Character").transform; // Getting the reference
	}

	// Update is called once per frame
	void FixedUpdate () {
        if (player && Vector3.Distance(player.position, transform.position) <= radius) { 
            Vector3 dir = player.position - transform.position; // Sets its direction
            dir = new Vector3(dir.x, 0, dir.z); // Elimintates y value
            enemyMove.dir = Vector3.Lerp (enemyMove.dir, dir.normalized, moveSensitivity); // Sets the magnitude to 1
            if (dir.magnitude >= 0.1f) {
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.position - transform.position), 0.1f); // Looks at the player
                transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0); // Ignores x and z values
            }
        } else {
            enemyMove.dir = Vector3.Lerp (enemyMove.dir, Vector3.zero, moveSensitivity);
        }
	}


}
