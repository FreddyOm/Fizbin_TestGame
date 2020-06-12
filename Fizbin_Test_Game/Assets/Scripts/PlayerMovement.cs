using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Names of the inputs
    enum InputKeys
    {
        left,
        right,
        jump,
        run
    }

    /* 
     *Corresponding keyCodes for the inputs
     *Has to be the same order as "InputKeys"
     */
    private KeyCode[] keyCodes = new KeyCode[]
    {
        KeyCode.A,              //left
        KeyCode.D,              //right
        KeyCode.Space,          //jump
        KeyCode.LeftShift       //run
    };

    // Variables
    private Rigidbody2D playerRigidbody;
    protected int groundLayer = 8;
    [Header("Movement")]
    [Tooltip("The acceleration of the player when walking.")]
    public float walkAcceleration = 20f;
    [Tooltip("The acceleration of the player when running.")]
    public float runAcceleration = 7f;
    public float maxPlayerWalkVelocity = 3f;
    public float maxPlayerRunVelocity = 4f;
    public float maxDecelerationDelta = .2f;
    bool isRunning = false;
    float horizontalMovement = 0;
    [Header("Jumping")]
    public bool canJump = true;
    public float jumpForce = 10f;
    bool isGrounded = false;


    void Start()
    {
        playerRigidbody = this.GetComponent<Rigidbody2D>();
    }

    // Read Inputs every frame
    void Update()
    {
        if (isGrounded)
        {
            InputDetection();
        }
    }

    //Movement and Deceleration
    private void FixedUpdate()
    {
        //horizontalMovement without running
        if(horizontalMovement != 0 && isGrounded && !isRunning && Mathf.Abs(playerRigidbody.velocity.x) < maxPlayerWalkVelocity)
        {
            playerRigidbody.AddForce(new Vector2(horizontalMovement * Time.fixedDeltaTime * 100, 0), ForceMode2D.Force);
        }
        else if (horizontalMovement != 0 && isGrounded && isRunning && Mathf.Abs(playerRigidbody.velocity.x) < maxPlayerRunVelocity)
        {
            playerRigidbody.AddForce(new Vector2(horizontalMovement * Time.fixedDeltaTime * 100, 0), ForceMode2D.Force);
        }
        else if (horizontalMovement == 0 && isGrounded)
        {
            Decelerate();
        }
    }
    public void InputDetection()
    {
        float movement = 0;

        //Check for Jump Input
        if (Input.GetKey(keyCodes[(int)InputKeys.jump]) && canJump && isGrounded)
        {
            //Jump
            Jump();
        }

        if (Input.GetKey(keyCodes[(int)InputKeys.left]) && isGrounded)
        {
            //Move left
            if (!isRunning)
            {
                movement += -walkAcceleration;
            }
            else
            {
                movement += -runAcceleration;
            }
        }
        if (Input.GetKey(keyCodes[(int)InputKeys.right]) && isGrounded)
        {
            //Move right
            if (!isRunning)
            {
                movement += walkAcceleration;
            }
            else
            {
                movement += runAcceleration;
            }
        }
        if (Input.GetKey(keyCodes[(int)InputKeys.run]) && isGrounded)
        {
            //Run
            isRunning = true;
        }else if(!Input.GetKey(keyCodes[(int)InputKeys.run]))
        {
            isRunning = false;
        }

        horizontalMovement = movement;

    }

    void Decelerate()
    {
        playerRigidbody.velocity = new Vector2(Mathf.MoveTowards(playerRigidbody.velocity.x, 0, maxDecelerationDelta), playerRigidbody.velocity.y);
    }


    void Jump()
    {
        //Jump
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0);
        playerRigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }


    //Checks, if player is on the ground
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.transform.gameObject.layer == groundLayer)
        {
            isGrounded = true;
        }
    }

    //Checks, if player is in the air
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.gameObject.layer == groundLayer)
        {
            isGrounded = false;
        }
    }
}
