using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private PlayerBrain _playerBrain;
    private Animator _playerAnimator;
    private Rigidbody2D _playerRigidBody2D;
    private PlayerInputManager _playerInputManager;
    private PlayerStateManager _playerStateManager;
    private PlayerCollisionDetector _playerCollisionDetector;

    private void Awake()
    {
        _playerAnimator = _playerBrain.PlayerAnimator;
        _playerRigidBody2D = _playerBrain.PlayerRigidBody2D;

        _playerInputManager = _playerBrain.GetInputManager();
        _playerStateManager = _playerBrain.GetStateManager();
        _playerCollisionDetector = _playerBrain.GetCollisionDetector();
    }


    private void Update()
    {
        ControlVerticalInput();
        SetGroundedState();
        SetFallingState();
        SetWallSlidingState();
        SetRunningState();
        SetJumpingState();
        SetDashingState();
    }

    private void ControlVerticalInput()
    {
        if (!_playerAnimator.GetBool("IsAttacking"))
        {
            var verticalInput1 = _playerInputManager.VerticalInputModifier;
            _playerAnimator.SetFloat("VerticalInput", verticalInput1);
        }
    }

    private void SetGroundedState()
    {
        if (_playerCollisionDetector.IsGrounded())
        {
            _playerAnimator.SetBool("IsGrounded", true);
            _playerAnimator.SetBool("IsFalling", false);
        }
        else
        {
            _playerAnimator.SetBool("IsGrounded", false);
        }
    }

    private void SetFallingState()
    {
        //Falling Animation
        if (_playerRigidBody2D.velocity.y < 0 && !_playerCollisionDetector.IsGrounded())
        {
            _playerAnimator.SetBool("IsFalling", true);
        }
        else
        {
            _playerAnimator.SetBool("IsFalling", false);
        }
    }

    private void SetWallSlidingState()
    {
        //Wall Sliding Animation
        if (_playerStateManager.IsWallSliding)
        {
            _playerAnimator.SetBool("IsWallSliding", true);
        }
        else
        {
            _playerAnimator.SetBool("IsWallSliding", false);
        }
    }

    private void SetRunningState()
    {
        //Running Animation
        if (_playerInputManager.HorizontalInputModifier != 0 && _playerCollisionDetector.IsGrounded())
        {
            _playerAnimator.SetBool("IsRunning", true);
        }
        else
        {
            _playerAnimator.SetBool("IsRunning", false);
        }
    }

    private void SetJumpingState()
    {
        //Jumping Animation
        if (_playerStateManager.IsJumping && !_playerStateManager.IsWallSliding && !_playerCollisionDetector.IsRoofed())
        {
            _playerAnimator.SetBool("IsJumping", true);
        }
        else
        {
            _playerAnimator.SetBool("IsJumping", false);
        }
    }

    private void SetDashingState()
    {
        if (_playerStateManager.IsDashing)
        {
            _playerAnimator.SetBool("IsDashing", true);
        }
        else
        {
            _playerAnimator.SetBool("IsDashing", false);
        }
    }
}