using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    [SerializeField] private PlayerBrain _playerBrain;

    //X Axis Movement
    public float MovementSpeed { get; private set; } = 25.0f;
    public float DashSpeed { get; private set; }
    public float MaxDashSteps { get; private set; }
    public float MaxDashCooldown { get; private set; }

    [SerializeField] private float moveSpeed = 25f; //Movement speed of the object/player
    [SerializeField] private float dashSpeed = 12f; //Speed used for dashing
    [SerializeField] private float dashSteps = 13f;
    [SerializeField] private float maxDashCooldown = 0.2f;
    private float horizontalMove = 0f; //Float variable containing value returned from the Input.GetAxisRaw function (no smoothing)

    //Y Axis Movement
    [SerializeField] private float jumpSpeed = 12f; //Jump speed of the object/player
    [SerializeField] private int jumpSteps = 10;
    [SerializeField] private float fallSpeed = 10f; //Fall speed of the object/player
    [SerializeField] private float wallSlideSpeed = 1.0f;
    [SerializeField] private float wallJumpSpeed = 5f;
    [SerializeField] private float wallJumpSteps = 7f;

    [Header("Components")]
    [SerializeField] private PlayerStateManager playerStateManager; //Reference to the PlayerStateList class which behaves like a state machine for the player
    [SerializeField] private Rigidbody2D playerRigidbody2d; //Rigidbody component of the object this script is attached to

    private int stepsJumped = 0;
    private int stepsDashed = 0;
    private int stepsWallJumped = 0;
    private float currentDashCooldown = 0f;
    private float jumpBufferTime = 0.05f; //How long the game should remember in seconds that the player has pressed the jump button (used for jump buffering)
    private float jumpBufferCounter; //The counter for the jump buffer that's decreased each frame
    private float groundedBufferTime = 0.1f; //How long the game should remember in seconds that the player has touched the ground (used for coyote jumping)
    private float groundedBufferCounter; //The counter for the ground buffer that's decreased each frame
    private float jumpCancelTime = 0.11f;
    private float jumpCancelCounter;

    public float MaxJumpBuffer { get; private set; }
    public float CurrentJumpBuffer { get; private set; }
    public float MaxGroundBuffer { get; private set; }
    public float CurrentGroundBuffer { get; private set; }

    private void Awake()
    {
        playerRigidbody2d = GetComponent<Rigidbody2D>();
        _playerBrain.GetStateManager().IsLookingRight = true;
    }

    void Update()
    {
        GetInput(); //Get inputs from the player
        CheckWallSliding();
    }

    void FixedUpdate()
    {
        Move();
        Jump();
        WallJump();
        Dash();
    }

    void GetInput()
    {
        //Movement on the X axis
        horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;

        //Jumping
        groundedBufferCounter -= Time.deltaTime; //Decrease ground buffer counter each fame
        if (_playerBrain.GetCollisionDetector().IsGrounded())
        {
            groundedBufferCounter = groundedBufferTime;
            stepsJumped = 0;
        }

        jumpBufferCounter -= Time.deltaTime; //Decrease jump buffer counter each frame
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
            jumpCancelCounter = jumpCancelTime;
        }

        if (jumpBufferCounter > 0 && groundedBufferCounter > 0 && jumpCancelCounter > 0)
        {
            groundedBufferCounter = 0;
            jumpBufferCounter = 0;
            jumpCancelCounter = 0;
            playerStateManager.IsJumping = true;
        }

        jumpCancelCounter -= Time.deltaTime;
        if (!Input.GetButton("Jump") && playerStateManager.IsJumping)
        {
            if (jumpCancelCounter < -jumpCancelTime)
            {
                StopJumpQuick();
            }
        }

        //Wall Jumping
        if (Input.GetButtonDown("Jump") && playerStateManager.IsWallSliding)
        {
            playerStateManager.IsWallJumping = true;
        }

        //Dashing
        if (Input.GetButtonDown("Dash") && currentDashCooldown == 0f && stepsDashed < dashSteps && !playerStateManager.IsWallSliding)
        {
            playerStateManager.IsDashing = true;
            currentDashCooldown = 0.2f;
        }
    }

    void Move()
    {
        //Handles movement such as walking, jumping, etc and is called under FixedUpdate
        if (!playerStateManager.IsWallSliding)
        {
            var horizontalMovement = _playerBrain.GetInputManager().HorizontalInputModifier * MovementSpeed;
            Vector3 targetVelocity = new Vector2(horizontalMovement * 10f * Time.fixedDeltaTime, playerRigidbody2d.velocity.y);
            _playerBrain.PlayerRigidBody2D.velocity = targetVelocity;
        }

            //If the absolute value of velocity X is greater than 0 than set player state "walking" to true
        if (Mathf.Abs(playerRigidbody2d.velocity.x) > 0 && _playerBrain.GetCollisionDetector().IsGrounded())
        {
            playerStateManager.IsWalking = true;
        }
        else
        {
            playerStateManager.IsWalking = false;
        }
        
        //Flip the player depending on whether they are looking right or not
        if (horizontalMove > 0 && !playerStateManager.IsLookingRight)
        {
            Flip();
        } 
        else if (horizontalMove < 0 && playerStateManager.IsLookingRight)
        {
            Flip();
        }

        //Wallchecking to determine if the player is standing next to a wall
        if (_playerBrain.GetCollisionDetector().IsTouchingWall())
        {
            playerStateManager.IsTouchingWall = true;
        }
        else
        {
            playerStateManager.IsTouchingWall = false;
        }

        if (playerStateManager.IsWallSliding)
        {
            if (playerRigidbody2d.velocity.y < -wallSlideSpeed)
            {
                playerRigidbody2d.velocity = new Vector2(playerRigidbody2d.velocity.x, -wallSlideSpeed);
                stepsWallJumped = 0;
            }
        }

    }

    void Flip()
    {
        //Switch to true if playerState.lookingRight is false, and to true if playerState.lookingRight is false
        playerStateManager.IsLookingRight = !playerStateManager.IsLookingRight;

        //Captures the local scale of the transform and multiplies its X value by -1 returning -1 if it was 1 or 1 if it was -1
        Vector3 flippedScale = transform.localScale;
        flippedScale.x *= -1;
        transform.localScale = flippedScale;

        //Flip wallcheck X
        var wallCheckLengthX = _playerBrain.GetCollisionDetector().GetWallCheckLengthX();
        _playerBrain.GetCollisionDetector().SetWallCheckLengthX(wallCheckLengthX * -1);
        
        if (playerStateManager.IsWallJumping)
        {
            playerStateManager.IsWallJumping = false;
            stepsWallJumped = 7;
        }
    }

    void Jump()
    {
        if (playerStateManager.IsJumping)
        {
            if (stepsJumped < jumpSteps && !_playerBrain.GetCollisionDetector().IsRoofed())
            {
                playerRigidbody2d.velocity = new Vector2(playerRigidbody2d.velocity.x, jumpSpeed);
                stepsJumped++;
            }
            else
            {
                StopJumpSlow();
            }   
        }

        //This limits how fast the player can fall
        //Since platformers generally have increased gravity, you don't want them to fall so fast they clip trough all the floors
        if (playerRigidbody2d.velocity.y < -Mathf.Abs(fallSpeed))
        {
            playerRigidbody2d.velocity = new Vector2(playerRigidbody2d.velocity.x, Mathf.Clamp(playerRigidbody2d.velocity.y, -Mathf.Abs(fallSpeed), Mathf.Infinity));
        }
    }

    void WallJump()
    {
        if (playerStateManager.IsWallJumping)
        {
            if (stepsWallJumped < wallJumpSteps && !_playerBrain.GetCollisionDetector().IsRoofed() && !playerStateManager.IsLookingRight)
            {
                playerRigidbody2d.velocity = new Vector2(wallJumpSpeed, jumpSpeed);
                stepsWallJumped++;
            }
            else if (stepsWallJumped < wallJumpSteps && !_playerBrain.GetCollisionDetector().IsRoofed() && playerStateManager.IsLookingRight)
            {
                playerRigidbody2d.velocity = new Vector2(-wallJumpSpeed, jumpSpeed);
                stepsWallJumped++;
            }
            else             
            {
                StopJumpSlow();
            }
        }
    }

    void Dash()
    {
        if (playerStateManager.IsDashing) //Increase forward facing velocity if the player is in the dashing state
        {
            if (stepsDashed <= dashSteps)
            {
                if (playerStateManager.IsLookingRight)
                {
                    playerRigidbody2d.gravityScale = 0;
                    playerRigidbody2d.velocity = new Vector2(dashSpeed, 0);
                    stepsDashed++;
                }
                else if (!playerStateManager.IsLookingRight)
                {
                    playerRigidbody2d.gravityScale = 0;
                    playerRigidbody2d.velocity = new Vector2(-dashSpeed, 0);
                    stepsDashed++;
                }
            }
            else
            {
                playerRigidbody2d.gravityScale = 1;
                playerStateManager.IsDashing = false;
            }
        }

        if (!playerStateManager.IsDashing && _playerBrain.GetCollisionDetector().IsGrounded()) //Prevent the player from dashing multiple times while in the air
        {
            stepsDashed = 0;
        }

        if (!playerStateManager.IsDashing) //Decrease dash cooldown once the dashing animation is finished
        {
            currentDashCooldown -= Time.fixedDeltaTime;
            currentDashCooldown = Mathf.Clamp(currentDashCooldown, 0, maxDashCooldown);
        }
    }

    void CheckWallSliding()
    {
        if (playerStateManager.IsTouchingWall && !_playerBrain.GetCollisionDetector().IsGrounded() && playerRigidbody2d.velocity.y < 0)
        {
            playerStateManager.IsWallSliding = true;
        }
        else
        {
            playerStateManager.IsWallSliding = false;
        }
    }

    void StopJumpQuick()
    {
        stepsJumped = 0;
        playerStateManager.IsJumping = false;
        playerRigidbody2d.velocity = new Vector2(playerRigidbody2d.velocity.x, 0);
    }

    void StopJumpSlow()
    {
        stepsJumped = 0;
        stepsWallJumped = 0;
        playerStateManager.IsJumping = false;
        playerStateManager.IsWallJumping = false;
    }
}
