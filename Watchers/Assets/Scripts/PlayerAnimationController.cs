using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private PlayerBrain _playerBrain;

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
        if (_playerBrain.GetCollisionDetector().IsGrounded())
        {
            _playerBrain.PlayerAnimator.SetBool("IsGrounded", true);
            _playerBrain.PlayerAnimator.SetBool("IsFalling", false);
        }
        else
        {
            _playerBrain.PlayerAnimator.SetBool("IsGrounded", false);
        }
    }

    void SetFallingState()
    {
        //Falling Animation
        if (_playerBrain.PlayerRigidBody2D.velocity.y < 0 && !_playerBrain.GetCollisionDetector().IsGrounded())
        {
            _playerBrain.PlayerAnimator.SetBool("IsFalling", true);
        }
    }

    void SetRunningState()
    {
        //Running Animation
        if (Mathf.Abs(_playerBrain.PlayerRigidBody2D.velocity.x) > 0 && _playerBrain.GetCollisionDetector().IsGrounded())
        {
            _playerBrain.PlayerAnimator.SetBool("IsRunning", true);
        }
        else
        {
            _playerBrain.PlayerAnimator.SetBool("IsRunning", false);
        }
    }

    void SetJumpingState()
    {
        //Jumping Animation
        if (_playerBrain.GetStateManager().IsJumping && !_playerBrain.GetStateManager().IsWallSliding)
        {
            _playerBrain.PlayerAnimator.SetBool("IsJumping", true);
        }
        else
        {
            _playerBrain.PlayerAnimator.SetBool("IsJumping", false);
        }
    }

    void SetDashingState()
    {
        //Dash Animation
        if (_playerBrain.GetStateManager().IsDashing)
        {
            _playerBrain.PlayerAnimator.SetBool("IsDashing", true);
        } else
        {
            _playerBrain.PlayerAnimator.SetBool("IsDashing", false);
        }
    }
}
