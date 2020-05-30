using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionDetector : MonoBehaviour
{
    [SerializeField] private PlayerBrain _playerBrain;

    [Header("Groud Checking")]
    [SerializeField] private float _groundCheckLengthX = 0.3f;
    [SerializeField] private float _groundCheckLengthY = 0.2f;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Roof Checking")]
    [SerializeField] private float _roofCheckLengthX = 0.3f;
    [SerializeField] private float _roofCheckLengthY = 0.2f;

    [Header("Wall Checking")]
    [SerializeField] private float _wallCheckLengthX = 0.4f;

    public float GetWallCheckLengthX()
    {
        return _wallCheckLengthX;
    }

    public void SetWallCheckLengthX(float value)
    {
        _wallCheckLengthX = value;
    }

    [Header("Components")]
    [SerializeField] private Transform _groundCheckTransform;
    [SerializeField] private Transform _roofCheckTransform;
    [SerializeField] private Transform _wallCheckTransform;

    public bool IsGrounded()
    {
        if (Physics2D.Raycast(_groundCheckTransform.position, Vector2.down, _groundCheckLengthY, _groundLayer) ||
            Physics2D.Raycast(_groundCheckTransform.position + new Vector3(-_groundCheckLengthX, 0), Vector2.down, _groundCheckLengthY, _groundLayer) ||
            Physics2D.Raycast(_groundCheckTransform.position + new Vector3(_groundCheckLengthX, 0), Vector2.down, _groundCheckLengthY, _groundLayer))
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
        if (Physics2D.Raycast(_roofCheckTransform.position, Vector2.up, _roofCheckLengthY, _groundLayer) ||
            Physics2D.Raycast(_roofCheckTransform.position + new Vector3(-_roofCheckLengthX, 0), Vector2.up, _roofCheckLengthY, _groundLayer) ||
            Physics2D.Raycast(_roofCheckTransform.position + new Vector3(_roofCheckLengthX, 0), Vector2.up, _roofCheckLengthY, _groundLayer))
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
        if (Physics2D.Raycast(_wallCheckTransform.position, transform.right, _wallCheckLengthX, _groundLayer))
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
        Gizmos.DrawLine(_groundCheckTransform.position, _groundCheckTransform.position + new Vector3(0, -_groundCheckLengthY));
        Gizmos.DrawLine(_groundCheckTransform.position + new Vector3(-_groundCheckLengthX, 0), _groundCheckTransform.position + new Vector3(-_groundCheckLengthX, -_groundCheckLengthY));
        Gizmos.DrawLine(_groundCheckTransform.position + new Vector3(_groundCheckLengthX, 0), _groundCheckTransform.position + new Vector3(_groundCheckLengthX, -_groundCheckLengthY));

        //Draw lines for roofcheck raycasts
        Gizmos.DrawLine(_roofCheckTransform.position, _roofCheckTransform.position + new Vector3(0, _roofCheckLengthY));
        Gizmos.DrawLine(_roofCheckTransform.position + new Vector3(-_roofCheckLengthX, 0), _roofCheckTransform.position + new Vector3(-_roofCheckLengthX, _roofCheckLengthY));
        Gizmos.DrawLine(_roofCheckTransform.position + new Vector3(_roofCheckLengthX, 0), _roofCheckTransform.position + new Vector3(_roofCheckLengthX, _roofCheckLengthY));

        //Draw lines for wallcheck raycasts
        Gizmos.DrawLine(_wallCheckTransform.position, new Vector3(_wallCheckTransform.position.x + _wallCheckLengthX, _wallCheckTransform.position.y, _wallCheckTransform.position.z));
    }
}
