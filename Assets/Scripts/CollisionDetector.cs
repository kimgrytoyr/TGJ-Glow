using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour {

	public GameObject Player;
	Animator anim;

	float distToGround;

	// Use this for initialization
	void Start () {
		anim = Player.GetComponent<Animator> ();
		distToGround = Player.GetComponent<BoxCollider2D> ().bounds.extents.y;
	}

	void FixedUpdate () {
		//		// If colliding with the ground
		Bounds bounds = Player.GetComponent<BoxCollider2D> ().bounds;
		float length = distToGround + 0.15f;
//		Vector2 origin = new Vector2 (bounds.min.x, bounds.min.y) + (Vector2.right * Player.GetComponent<Rigidbody2D> ().velocity.x);
		Vector2 origin = Player.GetComponent<Rigidbody2D>().transform.position;
		RaycastHit2D[] hits = Physics2D.RaycastAll(origin, -Vector2.up, length);
		Debug.DrawRay(origin, -Vector2.up * (length), Color.red);

		bool hitGround = false;
		for (int i = 0; i < hits.Length; i++) {
			//Debug.Log (hits [i].collider.name);
			if (hits[i].collider.CompareTag ("Wall")) {
				//Debug.Log (hits[i].collider.name);
				hitGround = true;
			}
			if (hits [i].collider.name == "Killzone") {
				Player.SendMessage ("respawn");
			}
		}

		if (hitGround) {
			if (Player.GetComponent<PlayerController> ().triggeredJumping) {
				Debug.Log (Player.GetComponent<Rigidbody2D> ().velocity.y);
				// Recently jumped
				return;
			}
			anim.SetBool ("Jumping_Ascending", false);
			anim.SetBool ("Jumping_Descending", false);
			anim.SetBool ("Running", true);
			if (Player.GetComponent<PlayerController> ().stuckToWall) {
				Player.GetComponent<PlayerController> ().Flip ();
			}
			Player.SendMessage ("grounded", true);
			//Debug.Log ("We hit the ground!");
		} else if (Player.GetComponent<PlayerController> ().isGrounded) {
			anim.SetBool ("Running", false);
			Player.SendMessage ("grounded", false);
			if (!Player.GetComponent<PlayerController> ().Jumping () && Player.GetComponent<PlayerController> ().triggeredJumping) {
				Player.GetComponent<PlayerController> ().Flip ();
			}
		}	
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (!collider.CompareTag ("Wall"))
			return;
		
//		Debug.Log ("hit a wall");
//		Debug.Log (Player.GetComponent<PlayerController> ().isGrounded);
		if (!Player.GetComponent<PlayerController> ().stuckToWall && Player.GetComponent<PlayerController> ().isGrounded) {
			Player.GetComponent<PlayerController> ().Flip ();
			return;
		}

//		Debug.Log ("Potentially falling off a platform..");
//		Debug.Log (Player.GetComponent<PlayerController> ().Jumping ());

		if (!Player.GetComponent<PlayerController> ().isGrounded && !Player.GetComponent<PlayerController> ().Jumping() && !Player.GetComponent<PlayerController> ().triggeredJumping && !Player.GetComponent<PlayerController>().falling) {
			//Debug.Log (Player.GetComponent<Rigidbody2D> ().velocity.y);
			Player.GetComponent<PlayerController> ().Flip ();
		}

		Player.SendMessage("wallStick", true);
		Player.GetComponent<Animator> ().SetBool ("Wallsliding", true);
		Player.GetComponent<Animator> ().SetBool ("Running", false);
		Player.GetComponent<Animator> ().SetBool ("Jumping_Ascending", false);
		Player.GetComponent<Animator> ().SetBool ("Jumping_Descending", false);
	}

	private void OnTriggerExit2D(Collider2D collider) {
		if (!collider.CompareTag ("Wall") || Player.GetComponent<Rigidbody2D>().velocity.y != 0)
			return;

		Debug.Log (Player.GetComponent<Rigidbody2D> ().velocity.y);
		Debug.Log ("Left a wall!");
		Player.GetComponent<PlayerController> ().falling = true;
		Player.SendMessage ("wallStick", false);
	}
}
