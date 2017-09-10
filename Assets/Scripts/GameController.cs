using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public int orbsTotal = 9;
	public GameObject deadSquirrel;
	public GameObject victoryMenu;
	public GameObject victorySound;

	public void Restart() {
		Time.timeScale = 1.0f;
		SceneManager.LoadScene ("Test");
	}

	public void QuitGame() {
		Application.Quit ();
	}

	public void OrbActivated() {
		GameObject[] orbs = GameObject.FindGameObjectsWithTag ("Orb");

		int orbsActivated = 0;
		for (int i = 0; i < orbs.Length; i++) {
			if (orbs [i].GetComponent<OrbController>().activated) {
				orbsActivated++;
			}
		}


		if (orbsActivated >= orbsTotal) {
			// Spawn dead squirrel
			deadSquirrel.SetActive(true);
		}
	}

	public void Victory() {
		victoryMenu.SetActive (true);
		victorySound.GetComponent<AudioSource> ().Play ();
	}
}
