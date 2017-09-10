using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	Rigidbody2D playerRigidBody;
	public float jumpTimer = 1.0f;
	Vector2 FacingDirection = Vector2.right;
	Vector2 previousHeading;
	Vector3 previousEuler;
	public Vector3 lastOrbPosition;
	public float moveSpeed = 5;
	float lockY;

	public bool isGrounded;
	public bool stuckToWall = false;
	bool allowedToGlide = false;
	bool hasJumped = false;

	Animator anim;

	AudioSource jumpSound;

	void Awake () {
		//Camera.main.orthographicSize = Screen.height / 2;
	}

	float runDelay = 1.0f;

	// Use this for initialization
	void Start () {
		playerRigidBody = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator> ();
		jumpSound = GameObject.Find ("JumpSound").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (runDelay >= 0) {
			runDelay -= Time.deltaTime;
			return;
		}

		if (stuckToWall && !isGrounded && lockY != 0.0f)
		{
			transform.position = new Vector2(transform.position.x, lockY);
			lockY -= 1 * Time.deltaTime;
		}

		else
		{
			playerRigidBody.position += FacingDirection * Time.deltaTime * moveSpeed;
		}

		if (jumpTimer <= 0)
		{
			allowedToGlide = false;
		}

		if (Input.GetButtonDown("Jump") && !hasJumped)
		{
			jumpSound.Play ();
			if (stuckToWall) {
				Flip ();
			}
			anim.SetBool ("Running", false);
			playerRigidBody.AddForce(new Vector2(0, 200));
			grounded(false);
			wallStick(false);
			allowedToGlide = true;
			Debug.Log ("Jumped");
		}

		if (Input.GetButtonUp("Jump"))
		{
			allowedToGlide = false;
			lockY = transform.position.y;
		}

		if (Input.GetButton("Jump") && allowedToGlide)
		{
			jumpTimer -= 1 * Time.deltaTime;

			if (jumpTimer >= 0)
			{
				playerRigidBody.AddForce(new Vector2(0, 20));
			} 

			wallStick(false);
		}
//		Debug.Log (GetComponent<Rigidbody2D> ().velocity.y);
		if (GetComponent<Rigidbody2D> ().velocity.y > 0 && !isGrounded) {
			// Ascending
			anim.SetBool ("Jumping_Ascending", true);
			anim.SetBool ("Jumping_Descending", false);
		} else if (GetComponent<Rigidbody2D> ().velocity.y < 0 && !isGrounded) {
			// Descending
			anim.SetBool ("Jumping_Ascending", false);
			anim.SetBool ("Jumping_Descending", true);
		} else if (isGrounded) {
			anim.SetBool ("Jumping_Ascending", false);
			anim.SetBool ("Jumping_Descending", false);
			anim.SetBool ("Running", true);
			grounded (true);
		}

		return;
//		if (Input.GetAxisRaw ("Horizontal") > 0f) {
//			// Moving right
//			transform.Translate (Vector3.left * moveSpeed * Time.deltaTime);
//			transform.eulerAngles = new Vector3 (0f, 180f, 0f);
//		} else if (Input.GetAxisRaw ("Horizontal") < 0f) {
//			// Moving left
//			transform.Translate (Vector3.left * moveSpeed * Time.deltaTime);
//			transform.eulerAngles = new Vector3 (0f, 0f, 0f);
//		} else {
//			//anim.SetBool ("Running", false);
//		}
//
//		if (!grounded) {
//			Debug.Log ("Not grounded");
//			anim.SetBool ("Running", false);
//		}
//		
//		if (Input.GetKey(KeyCode.Space)) {
//			GetComponent<Rigidbody2D>().AddForce(new Vector3(0f, jumpForce, 0f));
//			grounded = false;
//			anim.SetBool ("Jumping_Ascending", true);
//		}
	}

	public void Flip() {
		FaceDirection (-FacingDirection);
		transform.eulerAngles = new Vector3 (0f, (transform.eulerAngles.y == 180f ? 0f : 180f), 0f);
		Light light = GetComponentInChildren<Light> ();
		light.transform.position = new Vector3 (light.transform.position.x, light.transform.position.y, -light.transform.position.z);
	}

	void FaceDirection(Vector2 arg1)
	{
		FacingDirection = arg1;
	}

	void wallStick(bool arg)
	{

		stuckToWall = arg;
		if (arg)
		{
			lockY = transform.position.y;
			playerRigidBody.gravityScale = 0;
			playerRigidBody.velocity = new Vector2(0,0);
			hasJumped = false;
			allowedToGlide = false;
			jumpTimer = 1.0f;
			Debug.Log ("No longer jumping..");
		}

		else
		{
			playerRigidBody.gravityScale = 1;
			lockY = 0.0f;
			anim.SetBool ("Wallsliding", false);
		}
	}

	void grounded(bool arg)
	{
		isGrounded = arg;
		if (arg) {
			Debug.Log ("No longer jumping..");
			jumpTimer = 1.0f;
		} else {
		}
	}

	void setOrb(Vector3 arg)
	{
		lastOrbPosition = arg;
		previousHeading = FacingDirection;
		previousEuler = transform.eulerAngles;

	}

	void respawn()
	{
		transform.position = lastOrbPosition;
		FacingDirection = previousHeading;
		transform.eulerAngles = previousEuler;
	
	}

	void FixedUpdate() {
		return;
//		Bounds bounds = GetComponent<BoxCollider2D> ().bounds;
//		RaycastHit2D hit = Physics2D.Raycast(new Vector2(bounds.min.x, bounds.min.y), -Vector2.up, 0.05f);
//		Debug.DrawLine(new Vector3(bounds.min.x, bounds.min.y, 0f), new Vector3(0, -0.05f, 0), Color.red);
//		bool hitGround = false;
//		if (hit != null) {
//			if (hit.collider != null) {
//				if (hit.collider.CompareTag ("Ground")) {
//					hitGround = true;
//					anim.SetBool ("Jumping_Ascending", false);
//					anim.SetBool ("Jumping_Descending", false);
//					anim.SetBool ("Running", true);
//				}
//			}
//		}
//		Debug.Log (GetComponent<Rigidbody2D> ().velocity);
//
//
//		if (GetComponent<Rigidbody2D> ().velocity.y > 0f) {
//			// Ascending
//			anim.SetBool ("Jumping_Ascending", true);
//			anim.SetBool ("Jumping_Descending", false);
//		} else if (GetComponent<Rigidbody2D> ().velocity.y < 0f) {
//			// Descending
//			anim.SetBool ("Jumping_Ascending", false);
//			anim.SetBool ("Jumping_Descending", true);
//		} else {
//			anim.SetBool ("Jumping_Ascending", false);
//			anim.SetBool ("Jumping_Descending", false);
//		}
//		grounded = hitGround;
	}
}
