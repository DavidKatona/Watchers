using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField] private PlayerBrain _playerBrain;

    [Header("Components")]
    [SerializeField] private Transform _forwardAttackTransform;
    [SerializeField] private Transform _upwardsAttackTransform;
    [SerializeField] private Transform _downwardsAttackTransform;

    [Header("Attacking")]
    [SerializeField] private float _timeBetweenAttacks = 0.4f;
    [SerializeField] private float _forwardAttackRadius = 1;
    [SerializeField] private float _upwardsAttackRadius = 1;
    [SerializeField] private float _downwardsAttackRadius = 1;
    [SerializeField] private LayerMask _attackableLayer;
    private float _timeElapsedSinceLastAttack;

    void Update()
    {
        Attack();
    }

    private void Attack()
    {
        _timeElapsedSinceLastAttack += Time.deltaTime;

        float yAxisInput = _playerBrain.GetInputManager().VerticalInputModifier;

        if (_playerBrain.GetInputManager().IsAttackHeldDown && _timeElapsedSinceLastAttack >= _timeBetweenAttacks)
        {
            _timeElapsedSinceLastAttack = 0;

            //ToDo: Extract attack variations to separate methods
            //Forward Attack
            if (yAxisInput == 0 || yAxisInput < 0 && _playerBrain.GetCollisionDetector().IsGrounded())
            {
                //ToDo: Handle AnimationController
                Debug.Log("Attacking forwards...");
                Collider2D[] objectsToHit = Physics2D.OverlapCircleAll(_forwardAttackTransform.position, _forwardAttackRadius, _attackableLayer);

                if (objectsToHit.Length > 0)
                {
                    _playerBrain.GetStateManager().IsRecoilingX = true;
                }

                for (int i = 0; i < objectsToHit.Length; i++)
                {
                    Debug.Log($"{objectsToHit[i].name} has been hit.");
                }
            }

            //Upward Attack
            else if (yAxisInput > 0)
            {
                //ToDo: Handle AnimationController
                Debug.Log("Attacking upwards...");
                Collider2D[] objectsToHit = Physics2D.OverlapCircleAll(_upwardsAttackTransform.position, _upwardsAttackRadius, _attackableLayer);

                if (objectsToHit.Length > 0)
                {
                    _playerBrain.GetStateManager().IsRecoilingY = true;
                }

                for (int i = 0; i < objectsToHit.Length; i++)
                {
                    Debug.Log($"{objectsToHit[i].name} has been hit.");
                }
            }

            //Downward Attack
            else if (yAxisInput < 0)
            {
                //ToDo: Handle AnimationController
                Debug.Log("Attacking downwards...");
                Collider2D[] objectsToHit = Physics2D.OverlapCircleAll(_downwardsAttackTransform.position, _downwardsAttackRadius, _attackableLayer);

                if (objectsToHit.Length > 0)
                {
                    _playerBrain.GetStateManager().IsRecoilingY = true;
                }

                for (int i = 0; i < objectsToHit.Length; i++)
                {
                    Debug.Log($"{objectsToHit[i].name} has been hit.");
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_forwardAttackTransform.position, _forwardAttackRadius);
        Gizmos.DrawWireSphere(_upwardsAttackTransform.position, _upwardsAttackRadius);
        Gizmos.DrawWireSphere(_downwardsAttackTransform.position, _downwardsAttackRadius);
    }
}
