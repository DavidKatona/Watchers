using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateList : MonoBehaviour
{
    public bool IsWalking { get; set; }
    public bool IsInteracting { get; set; }
    public bool IsLookingRight { get; set; }
    public bool IsJumping { get; set; }
    public bool IsDashing { get; set; }
    public bool IsTouchingWall { get; set; }
    public bool IsWallSliding { get; set; }
    public bool IsWallJumping { get; set; }
    public bool IsCasting { get; set; }
    public bool IsCastReleased { get; set; }
    public bool IsNearBench { get; set; }
    public bool IsOnBench { get; set; }
    public bool IsNearNPC { get; set; }
    public bool IsUsingNPC { get; set; }
}