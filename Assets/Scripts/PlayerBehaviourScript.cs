using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviourScript: MonoBehaviour {

    
    Rigidbody2D playerRigidBody;
    public float jumpTimer = 1.0f;
    Vector2 FacingDirection = Vector2.right;
    Vector2 moveDirection = Vector2.right;
    int moveSpeed = 2;
    float lockY;

    public static bool isGrounded;
    public bool stuckToWall = false;
    bool allowedToGlide = false;
    bool hasJumped = false;


    // Use this for initialization
    void Start () {
        playerRigidBody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {

        if (stuckToWall && !isGrounded)
        {
            transform.position = new Vector2(transform.position.x, lockY);
            lockY -= 1 * Time.deltaTime;
        }

        else
        {
            playerRigidBody.position += moveDirection * Time.deltaTime * moveSpeed;
        }

        if (jumpTimer <= 0)
        {
            allowedToGlide = false;
        }

        if (Input.GetButtonDown("Jump") && !hasJumped)
        {

            if (stuckToWall)
            {
                moveDirection = FacingDirection;
            }

            playerRigidBody.AddForce(new Vector2(0, 100));
            grounded(false);
            wallStick(false);
            allowedToGlide = true;
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
                playerRigidBody.AddForce(new Vector2(0, 10));
            }

            wallStick(false);
        }

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
        }

        else
        {
            playerRigidBody.gravityScale = 1;
        }
    }

    void grounded(bool arg)
    {
        isGrounded = arg;
        if (arg)
        {
            jumpTimer = 1.0f;
        }
        hasJumped = !arg;
    }

}
