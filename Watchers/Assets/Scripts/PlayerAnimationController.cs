using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private PlayerBrain _playerBrain;

    private void Update()
    {
        SetGroundedState();
        SetFallingState();
        SetRunningState();
        SetJumpingState();
        SetDashingState();
        TriggerAttack();
    }

    private void SetGroundedState()
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

    private void SetFallingState()
    {
        //Falling Animation
        if (_playerBrain.PlayerRigidBody2D.velocity.y < 0 && !_playerBrain.GetCollisionDetector().IsGrounded())
        {
            _playerBrain.PlayerAnimator.SetBool("IsFalling", true);
        }
    }

    private void SetRunningState()
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

    private void SetJumpingState()
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

    private void SetDashingState()
    {
        //Dash Animation
        if (_playerBrain.GetStateManager().IsDashing)
        {
            _playerBrain.PlayerAnimator.SetBool("IsDashing", true);
        } 
        else
        {
            _playerBrain.PlayerAnimator.SetBool("IsDashing", false);
        }
    }

    private void TriggerAttack()
    {
        //Attack Animations
        if (_playerBrain.GetStateManager().IsAttacking)
        {
            _playerBrain.PlayerAnimator.SetBool("IsAttacking", true);
        }
        else
        {
            _playerBrain.PlayerAnimator.SetBool("IsAttacking", false);
        }
    }
}
