using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("X Axis Movement")]
    [SerializeField] private float moveSpeed = 25f; //Movement speed of the object/player
    [SerializeField] private float dashSpeed = 12f; //Speed used for dashing
    [SerializeField] private float dashSteps = 13f;
    [SerializeField] private float maxDashCooldown = 0.2f;
    private float horizontalMove = 0f; //Float variable containing value returned from the Input.GetAxisRaw function (no smoothing)

    [Header("Y Axis Movement")]
    [SerializeField] private float jumpSpeed = 12f; //Jump speed of the object/player
    [SerializeField] private int jumpSteps = 10;
    [SerializeField] private float fallSpeed = 10f; //Fall speed of the object/player
    [SerializeField] private float wallSlideSpeed = 1.0f;
    [SerializeField] private float wallJumpSpeed = 5f;
    [SerializeField] private float wallJumpSteps = 7f;

    [Header("Components")]
    [SerializeField] private PlayerStateManager playerStateManager; //Reference to the PlayerStateList class which behaves like a state machine for the player
    [SerializeField] private Rigidbody2D playerRigidbody2d; //Rigidbody component of the object this script is attached to

    [Header("Attacking")]
    [SerializeField] float timeBetweenAttack = 0.4f;
    [SerializeField] Transform attackTransform; // this should be a transform childed to the player but to the right of them, where they attack from.
    [SerializeField] float attackRadius = 1;
    [SerializeField] Transform downAttackTransform;//This should be a transform childed below the player, for the down attack.
    [SerializeField] float downAttackRadius = 1;
    [SerializeField] Transform upAttackTransform;//Same as above but for the up attack.
    [SerializeField] float upAttackRadius = 1;
    [SerializeField] LayerMask attackableLayer;

    [Header("Recoil")]
    [SerializeField] int recoilXSteps = 4;
    [SerializeField] int recoilYSteps = 10;
    [SerializeField] float recoilXSpeed = 45;
    [SerializeField] float recoilYSpeed = 45;

    [Header("Ground Checking")]
    [SerializeField] private Transform groundCheckTransform; //Reference to the transform doing ground checks on the player
    [SerializeField] private float groundCheckY = 0.2f; //How far on the Y axis the groundcheck Raycast goes
    [SerializeField] private float groundCheckX = 0.3f; //How far on the X axis the groundcheck Raycast goes
    [SerializeField] private LayerMask groundLayer; //What is ground?

    [Header("Roof Checking")]
    [SerializeField] private Transform roofCheckTransform; //Reference to the transform doing roof checks on the player
    [SerializeField] private float roofCheckY = 0.2f; //How far on the Y axis the roofcheck Raycast goes
    [SerializeField] private float roofCheckX = 0.3f; //How far on the X axis the roofcheck Raycast goes

    [Header("Wall Checking")]
    [SerializeField] private Transform wallCheckTransform; //Reference to the transform doing wall checks on the player
    [SerializeField] private float wallCheckX = 0.4f; //How far on the X axis the roofcheck Raycast goes

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

    private void Awake()
    {
        playerRigidbody2d = GetComponent<Rigidbody2D>();
        playerStateManager.IsLookingRight = true;
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
        if (IsGrounded())
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
            Vector3 targetVelocity = new Vector2(horizontalMove * 10f * Time.fixedDeltaTime, playerRigidbody2d.velocity.y);
            playerRigidbody2d.velocity = targetVelocity;
        }

            //If the absolute value of velocity X is greater than 0 than set player state "walking" to true
        if (Mathf.Abs(playerRigidbody2d.velocity.x) > 0 && IsGrounded())
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
        if (IsTouchingWall())
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

        //Flip wallcheck X and wall hop speed as well
        wallCheckX *= -1;
        
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
            if (stepsJumped < jumpSteps && !IsRoofed())
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
            if (stepsWallJumped < wallJumpSteps && !IsRoofed() && !playerStateManager.IsLookingRight)
            {
                playerRigidbody2d.velocity = new Vector2(wallJumpSpeed, jumpSpeed);
                stepsWallJumped++;
            }
            else if (stepsWallJumped < wallJumpSteps && !IsRoofed() && playerStateManager.IsLookingRight)
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

        if (!playerStateManager.IsDashing && IsGrounded()) //Prevent the player from dashing multiple times while in the air
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
        if (playerStateManager.IsTouchingWall && !IsGrounded() && playerRigidbody2d.velocity.y < 0)
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

    public bool IsGrounded()
    {
        if (Physics2D.Raycast(groundCheckTransform.position, Vector2.down, groundCheckY, groundLayer) ||
            Physics2D.Raycast(groundCheckTransform.position + new Vector3(-groundCheckX, 0), Vector2.down, groundCheckY, groundLayer) ||
            Physics2D.Raycast(groundCheckTransform.position + new Vector3(groundCheckX, 0), Vector2.down, groundCheckY, groundLayer))
        {
            return true;
        } 
        else
        {
            return false;
        }
    }

    public bool IsRoofed()
    {
        if (Physics2D.Raycast(roofCheckTransform.position, Vector2.up, roofCheckY, groundLayer) ||
            Physics2D.Raycast(roofCheckTransform.position + new Vector3(-roofCheckX, 0), Vector2.up, roofCheckY, groundLayer) ||
            Physics2D.Raycast(roofCheckTransform.position + new Vector3(roofCheckX, 0), Vector2.up, roofCheckY, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsTouchingWall()
    {
        if (Physics2D.Raycast(wallCheckTransform.position, transform.right, wallCheckX, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }   
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Draw lines for groundcheck raycasts
        Gizmos.DrawLine(groundCheckTransform.position, groundCheckTransform.position + new Vector3(0, -groundCheckY));
        Gizmos.DrawLine(groundCheckTransform.position + new Vector3(-groundCheckX, 0), groundCheckTransform.position + new Vector3(-groundCheckX, -groundCheckY));
        Gizmos.DrawLine(groundCheckTransform.position + new Vector3(groundCheckX, 0), groundCheckTransform.position + new Vector3(groundCheckX, -groundCheckY));

        //Draw lines for roofcheck raycasts
        Gizmos.DrawLine(roofCheckTransform.position, roofCheckTransform.position + new Vector3(0, roofCheckY));
        Gizmos.DrawLine(roofCheckTransform.position + new Vector3(-roofCheckX, 0), roofCheckTransform.position + new Vector3(-roofCheckX, roofCheckY));
        Gizmos.DrawLine(roofCheckTransform.position + new Vector3(roofCheckX, 0), roofCheckTransform.position + new Vector3(roofCheckX, roofCheckY));

        //Draw lines for wallcheck raycasts
        Gizmos.DrawLine(wallCheckTransform.position, new Vector3(wallCheckTransform.position.x + wallCheckX, wallCheckTransform.position.y, wallCheckTransform.position.z));
    }
}
