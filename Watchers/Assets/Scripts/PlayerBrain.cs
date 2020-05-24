using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerBrain : MonoBehaviour
{
    [SerializeField] private PlayerMovementHandler _playerMovementHandler;
    public PlayerMovementHandler GetMovementHandler()
    {
        return _playerMovementHandler;
    }

    [SerializeField] private PlayerInputManager _playerInputManager;
    public PlayerInputManager GetInputManager()
    {
        return _playerInputManager;
    }

    [SerializeField] private PlayerCollisionDetector _playerCollisionDetector;
    public PlayerCollisionDetector GetCollisionDetector()
    {
        return _playerCollisionDetector;
    }

    [SerializeField] private PlayerStateManager _playerStateManager;
    public PlayerStateManager GetStateManager()
    {
        return _playerStateManager;
    }

    [SerializeField] private PlayerAnimationController _playerAnimationController;
    public PlayerAnimationController GetAnimationController()
    {
        return _playerAnimationController;
    }

    public Rigidbody2D PlayerRigidBody2D { get; private set; }
    public Animator PlayerAnimator { get; private set; }

    private void Awake()
    {
        PlayerRigidBody2D = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
    }
}
