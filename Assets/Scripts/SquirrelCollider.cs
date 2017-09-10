using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelCollider : MonoBehaviour {

	GameController gc;

	void Start() {
		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.transform.CompareTag ("Player")) {
			Time.timeScale = 0;
			gc.Victory ();
		}
	}
}
