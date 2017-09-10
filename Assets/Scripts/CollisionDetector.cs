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
			Debug.Log (hits [i].collider.name);
			if (hits[i].collider.CompareTag ("Wall")) {
				Debug.Log (hits[i].collider.name);
				hitGround = true;
			}
		}

		if (hitGround) {
			anim.SetBool ("Jumping_Ascending", false);
			anim.SetBool ("Jumping_Descending", false);
			anim.SetBool ("Running", true);
			Player.SendMessage ("grounded", true);
			Debug.Log ("We hit the ground!");
			if (Player.GetComponent<PlayerController> ().stuckToWall) {
				Player.GetComponent<PlayerController> ().Flip ();
			}
		} else {
			anim.SetBool ("Running", false);
			Player.SendMessage ("grounded", false);
		}			
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (!collider.CompareTag ("Wall"))
			return;
		Debug.Log ("hit a wall");

		if (!Player.GetComponent<PlayerController> ().stuckToWall && Player.GetComponent<PlayerController> ().isGrounded) {
			Player.GetComponent<PlayerController> ().Flip ();
			return;
		}

		Player.SendMessage("wallStick", true);
		Player.GetComponent<Animator> ().SetBool ("Wallsliding", true);
		Player.GetComponent<Animator> ().SetBool ("Running", false);
		Player.GetComponent<Animator> ().SetBool ("Jumping_Ascending", false);
		Player.GetComponent<Animator> ().SetBool ("Jumping_Descending", false);
	}

	private void OnTriggerExit2D(Collider2D collider) {
		if (!collider.CompareTag ("Wall"))
			return;

		Debug.Log ("Left a wall!");

		Player.SendMessage ("wallStick", false);

	}
}
