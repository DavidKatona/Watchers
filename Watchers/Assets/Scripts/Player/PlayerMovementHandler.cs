using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    [SerializeField] private PlayerBrain _playerBrain;
    [SerializeField] private ParticleSystem _particleJumpUp; //Trail particles
    [SerializeField] private ParticleSystem _particleJumpDown; //Explosion particles

    //X Axis Movement
    private float _moveSpeed = 25.0f;
    private float _dashSpeed = 12.0f;
    private float _maxDashSteps = 13.0f;
    private int _stepsDashed = 0;
    private float _maxDashCooldown = 0.2f;
    private float _currentDashCooldown = 0f;

    //Y Axis Movement
    private float _jumpSpeed = 12.0f;
    private int _maxJumpSteps = 10;
    private int _stepsJumped = 0;
    private float _wallSlideSpeed = 1.0f;
    private float _wallJumpSpeed = 5.0f;
    private int _maxWallJumpSteps = 7;
    private int _stepsWallJumped = 0;
    private float _fallSpeed = 10.0f;

    //Movement Buffers
    private float _maxJumpBuffer = 0.05f;
    private float _currentJumpBuffer;
    private float _maxGroundBuffer = 0.1f;
    private float _currentGroundBuffer;
    private float _jumpCancelTime = 0.11f;
    private float _jumpCancelCounter;

    private void Update()
    {
        ManageMovementBuffers();
        SetWallSliding();
    }

    private void FixedUpdate()
    {
        if (!_playerBrain.GetStateManager().IsRecoilingX) Move();
        Jump();
        WallJump();
        Dash();
        LimitFallSpeed();
    }

    private void ManageMovementBuffers()
    {
        _currentGroundBuffer -= Time.deltaTime;
        if (_playerBrain.GetCollisionDetector().IsGrounded())
        {
            _currentGroundBuffer = _maxGroundBuffer;
            _stepsJumped = 0;
        }

        _currentJumpBuffer -= Time.deltaTime;
        if (_playerBrain.GetInputManager().IsJumpPressed)
        {
            _currentJumpBuffer = _maxJumpBuffer;
            _jumpCancelCounter = _jumpCancelTime;
        }

        if (_currentJumpBuffer > 0 && _currentGroundBuffer > 0 && _jumpCancelCounter > 0)
        {
            _currentGroundBuffer = 0;
            _currentJumpBuffer = 0;
            _jumpCancelCounter = 0;
            _playerBrain.GetStateManager().IsJumping = true;
        }

        _jumpCancelCounter -= Time.deltaTime;
        if (!_playerBrain.GetInputManager().IsJumpHeld && _playerBrain.GetStateManager().IsJumping)
        {
            if (_jumpCancelCounter < -_jumpCancelTime)
            {
                StopJump(true);
            }
        }

        if (_playerBrain.GetInputManager().IsJumpPressed && _playerBrain.GetStateManager().IsWallSliding)
        {
            _playerBrain.GetStateManager().IsWallJumping = true;
        }

        if (_playerBrain.GetInputManager().IsDashPressed && _currentDashCooldown == 0f && _stepsDashed < _maxDashSteps && !_playerBrain.GetStateManager().IsWallSliding)
        {
            _playerBrain.GetStateManager().IsDashing = true;
            _playerBrain.PlayerAnimator.SetTrigger("Dash");
            _currentDashCooldown = _maxDashCooldown;
        }
    }

    private void Move()
    {
        bool wasGrounded = _playerBrain.GetStateManager().IsGrounded;
        if (!wasGrounded && _playerBrain.GetCollisionDetector().IsGrounded())
        {
            if (!_playerBrain.GetStateManager().IsWallSliding && !_playerBrain.GetStateManager().IsDashing)
            {
                _particleJumpDown.Play();
            }
        }

        //Handles movement such as walking, jumping, etc and is called under FixedUpdate
        if (!_playerBrain.GetStateManager().IsWallSliding)
        {
            var horizontalMovement = _playerBrain.GetInputManager().HorizontalInputModifier * _moveSpeed;
            Vector2 targetVelocity = new Vector2(horizontalMovement * 10f * Time.fixedDeltaTime, _playerBrain.PlayerRigidBody2D.velocity.y);
            _playerBrain.PlayerRigidBody2D.velocity = targetVelocity;
        }

        //If the absolute value of velocity X is greater than 0 than set player state "walking" to true
        if (Mathf.Abs(_playerBrain.PlayerRigidBody2D.velocity.x) > 0 && _playerBrain.GetCollisionDetector().IsGrounded())
        {
            _playerBrain.GetStateManager().IsWalking = true;
        }
        else
        {
            _playerBrain.GetStateManager().IsWalking = false;
        }

        //Flip the player depending on whether they are looking right or left
        if (_playerBrain.GetInputManager().HorizontalInputModifier > 0 && !_playerBrain.GetStateManager().IsLookingRight && !_playerBrain.GetStateManager().IsWallJumping)
        {
            Flip();
        }
        else if (_playerBrain.GetInputManager().HorizontalInputModifier < 0 && _playerBrain.GetStateManager().IsLookingRight && !_playerBrain.GetStateManager().IsWallJumping)
        {
            Flip();
        }

        //Determine if the player is standing next to a wall
        if (_playerBrain.GetCollisionDetector().IsTouchingWall())
        {
            _playerBrain.GetStateManager().IsTouchingWall = true;
        }
        else
        {
            _playerBrain.GetStateManager().IsTouchingWall = false;
        }

        if (_playerBrain.GetStateManager().IsWallSliding)
        {
            if (_playerBrain.PlayerRigidBody2D.velocity.y < -_wallSlideSpeed)
            {
                _playerBrain.PlayerRigidBody2D.velocity = new Vector2(_playerBrain.PlayerRigidBody2D.velocity.x, -_wallSlideSpeed);
                _stepsWallJumped = 0;
            }
        }

        _playerBrain.GetStateManager().IsGrounded = _playerBrain.GetCollisionDetector().IsGrounded();
    }

    private void Flip()
    {
        //Switch to true if playerState.lookingRight is false, and to true if playerState.lookingRight is false
        _playerBrain.GetStateManager().IsLookingRight = !_playerBrain.GetStateManager().IsLookingRight;

        //Captures the local scale of the transform and multiplies its X value by -1
        Vector3 flippedScale = transform.localScale;
        flippedScale.x *= -1;
        transform.localScale = flippedScale;

        //Flip wallcheck X
        var wallCheckLengthX = _playerBrain.GetCollisionDetector().GetWallCheckLengthX();
        _playerBrain.GetCollisionDetector().SetWallCheckLengthX(wallCheckLengthX * -1);
    }

    private void Jump()
    {
        if (_playerBrain.GetCollisionDetector().IsGrounded() && _playerBrain.GetStateManager().IsJumping)
        {
            _particleJumpDown.Play();
            _particleJumpUp.Play();
        }

        if (_playerBrain.GetStateManager().IsJumping)
        {
            if (_stepsJumped < _maxJumpSteps && !_playerBrain.GetCollisionDetector().IsRoofed())
            {
                _playerBrain.PlayerRigidBody2D.velocity = new Vector2(_playerBrain.PlayerRigidBody2D.velocity.x, _jumpSpeed);
                _stepsJumped++;
            }
            else
            {
                StopJump(false);
            }
        }
    }

    private void LimitFallSpeed()
    {
        //This limits how fast the player can fall
        //Since platformers generally have increased gravity, you don't want them to fall so fast they clip trough all the floors
        if (_playerBrain.PlayerRigidBody2D.velocity.y < -Mathf.Abs(_fallSpeed))
        {
            _playerBrain.PlayerRigidBody2D.velocity = new Vector2(_playerBrain.PlayerRigidBody2D.velocity.x, Mathf.Clamp(_playerBrain.PlayerRigidBody2D.velocity.y, -Mathf.Abs(_fallSpeed), Mathf.Infinity));
        }
    }

    private void WallJump()
    {
        if (_playerBrain.GetStateManager().IsWallJumping)
        {
            if (_stepsWallJumped < _maxWallJumpSteps && !_playerBrain.GetCollisionDetector().IsRoofed() && !_playerBrain.GetStateManager().IsLookingRight)
            {
                _playerBrain.PlayerRigidBody2D.velocity = new Vector2(-_wallJumpSpeed, _jumpSpeed);
                _stepsWallJumped++;
            }
            else if (_stepsWallJumped < _maxWallJumpSteps && !_playerBrain.GetCollisionDetector().IsRoofed() && _playerBrain.GetStateManager().IsLookingRight)
            {
                _playerBrain.PlayerRigidBody2D.velocity = new Vector2(_wallJumpSpeed, _jumpSpeed);
                _stepsWallJumped++;
            }
            else
            {
                StopJump(false);
            }
        }
    }

    private void Dash()
    {
        if (_playerBrain.GetStateManager().IsDashing) //Increase forward facing velocity if the player is in the dashing state
        {
            if (_stepsDashed <= _maxDashSteps)
            {
                _playerBrain.PlayerRigidBody2D.gravityScale = 0;
                _playerBrain.PlayerRigidBody2D.velocity = new Vector2(_dashSpeed * transform.localScale.x, 0);
                _stepsDashed++;
            }
            else
            {
                _playerBrain.PlayerRigidBody2D.gravityScale = 1;
                _playerBrain.GetStateManager().IsDashing = false;
            }
        }

        if (!_playerBrain.GetStateManager().IsDashing && _playerBrain.GetCollisionDetector().IsGrounded()) //Prevent the player from dashing multiple times while in the air
        {
            _stepsDashed = 0;
        }

        if (!_playerBrain.GetStateManager().IsDashing) //Decrease dash cooldown once the dashing animation is finished
        {
            _currentDashCooldown -= Time.fixedDeltaTime;
            _currentDashCooldown = Mathf.Clamp(_currentDashCooldown, 0, _maxDashCooldown);
        }
    }

    private void SetWallSliding()
    {
        if (_playerBrain.GetStateManager().IsTouchingWall && !_playerBrain.GetCollisionDetector().IsGrounded() && _playerBrain.PlayerRigidBody2D.velocity.y < 0)
        {
            _playerBrain.GetStateManager().IsWallSliding = true;
        }
        else
        {
            _playerBrain.GetStateManager().IsWallSliding = false;
        }
    }

    private void StopJump(bool shouldStopInstantly)
    {
        _stepsJumped = 0;
        _playerBrain.GetStateManager().IsJumping = false;

        if (shouldStopInstantly)
        {
            _playerBrain.PlayerRigidBody2D.velocity = new Vector2(_playerBrain.PlayerRigidBody2D.velocity.x, 0);
        }
        else
        {
            _stepsWallJumped = 0;
            _playerBrain.GetStateManager().IsWallJumping = false;
        }
    }
}
