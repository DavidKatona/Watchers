using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMovementHandler))]
[RequireComponent(typeof(PlayerInputManager))]
[RequireComponent(typeof(PlayerCollisionDetector))]
[RequireComponent(typeof(PlayerStateManager))]
[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerBrain : MonoBehaviour
{
    private PlayerMovementHandler _playerMovementHandler;
    public PlayerMovementHandler GetMovementHandler()
    {
        return _playerMovementHandler;
    }

    private PlayerInputManager _playerInputManager;
    public PlayerInputManager GetInputManager()
    {
        return _playerInputManager;
    }

    private PlayerCollisionDetector _playerCollisionDetector;
    public PlayerCollisionDetector GetCollisionDetector()
    {
        return _playerCollisionDetector;
    }

    private PlayerStateManager _playerStateManager;
    public PlayerStateManager GetStateManager()
    {
        return _playerStateManager;
    }

    private PlayerAnimationController _playerAnimationController;
    public PlayerAnimationController GetAnimationController()
    {
        return _playerAnimationController;
    }

    public Rigidbody2D PlayerRigidBody2D { get; private set; }
    public Animator PlayerAnimator { get; private set; }

    private void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        _playerMovementHandler = GetComponent<PlayerMovementHandler>();
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerCollisionDetector = GetComponent<PlayerCollisionDetector>();
        _playerStateManager = GetComponent<PlayerStateManager>();
        _playerAnimationController = GetComponent<PlayerAnimationController>();

        PlayerRigidBody2D = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
    }
}
