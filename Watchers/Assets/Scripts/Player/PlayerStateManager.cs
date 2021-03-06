﻿using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    [SerializeField] private PlayerBrain _playerBrain;

    public bool IsWalking { get; set; }
    public bool IsGrounded { get; set; }
    public bool IsInteracting { get; set; }
    public bool IsLookingRight { get; set; } = true;
    public bool IsJumping { get; set; }
    public bool IsDashing { get; set; }
    public bool IsTouchingWall { get; set; }
    public bool IsWallSliding { get; set; }
    public bool IsWallJumping { get; set; }
    public bool IsCasting { get; set; }
    public bool IsRecoilingX { get; set; }
    public bool IsRecoilingY { get; set; }
}