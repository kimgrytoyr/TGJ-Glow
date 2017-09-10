using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbController : MonoBehaviour {

	public Color orbColor;

	public GameObject player;

	Light[] lights;

	AudioSource glowActivateSound;
	GameController gc;

	public bool activated = false;

	// Use this for initialization
	void Start () {
		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
		glowActivateSound = GameObject.Find ("GlowActivateSound").GetComponent<AudioSource>();
		lights = GetComponentsInChildren<Light> ();
		for (int i = 0; i < lights.Length; i++) {
			lights [i].color = orbColor;
		}

		player = GameObject.Find ("squirrel_running_01");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (activated)
			return;
		
		Light[] lights = GetComponentsInChildren<Light>();
		for (int i = 0; i < lights.Length; i++) {
			lights[i].enabled = true;
		}

		activated = true;
		gc.OrbActivated ();

		player.SendMessage("setOrb", transform.position);

		glowActivateSound.Play ();
	}
}
