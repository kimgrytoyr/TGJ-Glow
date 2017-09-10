using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrounCheckScript : MonoBehaviour {

    public GameObject player;

	private void OnTriggerEnter2D(Collider2D collider)
    {
        player.SendMessage("grounded", true);
		player.GetComponent<Animator>().SetBool ("Running", true);
		player.GetComponent<Animator>().SetBool ("Jumping_Descending", false);
    }
}
