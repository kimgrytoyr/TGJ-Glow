using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killzoneScript : MonoBehaviour {

	public GameObject player;



	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.CompareTag ("Player")){
			player.SendMessage("respawn");
		}

}
}
