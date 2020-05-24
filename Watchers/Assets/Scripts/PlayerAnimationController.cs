using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Rigidbody2D playerRigidbody2d;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerStateManager playerStateManager;

    // Update is called once per frame
    void Update()
    {
        SetGroundedState();
        SetFallingState();
        SetRunningState();
        SetJumpingState();
        SetDashingState();
    }

    void SetGroundedState()
    {
        //Use the ground check method from the player movement script/component
        if (playerController.IsGrounded())
        {
            playerAnimator.SetBool("IsGrounded", true);
            playerAnimator.SetBool("IsFalling", false);
        }
        else
        {
            playerAnimator.SetBool("IsGrounded", false);
        }
    }

    void SetFallingState()
    {
        if (playerRigidbody2d.velocity.y < 0 && !playerController.IsGrounded())
        {
            playerAnimator.SetBool("IsFalling", true);
        }
    }

    void SetRunningState()
    {
        //Running Animation
        if (Mathf.Abs(playerRigidbody2d.velocity.x) > 0 && playerController.IsGrounded())
        {
            playerAnimator.SetBool("IsRunning", true);
        }
        else
        {
            playerAnimator.SetBool("IsRunning", false);
        }
    }

    void SetJumpingState()
    {
        //Jumping Animation
        if (playerStateManager.IsJumping && !playerStateManager.IsWallSliding)
        {
            playerAnimator.SetBool("IsJumping", true);
        }
        else
        {
            playerAnimator.SetBool("IsJumping", false);
        }
    }

    void SetDashingState()
    {
        //Dash Animation
        if (playerStateManager.IsDashing)
        {
            playerAnimator.SetBool("IsDashing", true);
        } else
        {
            playerAnimator.SetBool("IsDashing", false);
        }
    }
}
