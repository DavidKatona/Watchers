using Assets.Scripts.Damagables;
using System;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerBrain _playerBrain;
    [SerializeField] private StatManager _statManager;

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

    public event EventHandler OnDamaged;
    private float _timeElapsedSinceLastAttack;
    private float _horizontalRecoilSpeed = 5;
    private float _verticalRecoilSpeed = 10;
    private float _gravityScale;
    private int _horizontalRecoilSteps = 10;
    private int _stepsRecoiledHorizontally;
    private int _verticalRecoilSteps = 5;
    private int _stepsRecoiledVertically;

    private void Awake()
    {
        _gravityScale = GetComponent<Rigidbody2D>().gravityScale;
    }

    void Update()
    {
        Recoil();
        if (!_playerBrain.GetStateManager().IsWallSliding) Attack();
    }

    private void FixedUpdate()
    {
        if (_playerBrain.GetStateManager().IsRecoilingX && _stepsRecoiledHorizontally < _horizontalRecoilSteps)
        {
            _stepsRecoiledHorizontally++;
        }
        else
        {
            StopHorizontalRecoil();
        }

        if (_playerBrain.GetStateManager().IsRecoilingY && _stepsRecoiledVertically < _verticalRecoilSteps)
        {
            _stepsRecoiledVertically++;
        }
        else
        {
            StopVerticalRecoil();
        }

        if (_playerBrain.GetCollisionDetector().IsGrounded())
        {
            StopVerticalRecoil();
        }
    }

    private void Attack()
    {
        _timeElapsedSinceLastAttack += Time.deltaTime;

        if (_timeElapsedSinceLastAttack > _timeBetweenAttacks)
        {
            _playerBrain.PlayerAnimator.SetBool("IsAttacking", false);
        }

        float yAxisInput = _playerBrain.GetInputManager().VerticalInputModifier;

        if (_playerBrain.GetInputManager().IsAttackPressed && _timeElapsedSinceLastAttack >= _timeBetweenAttacks && !_playerBrain.GetStateManager().IsDashing)
        {
            _timeElapsedSinceLastAttack = 0;

            //ToDo: Extract attack variations to separate methods
            //Forward Attack
            if (yAxisInput == 0 || yAxisInput < 0 && _playerBrain.GetCollisionDetector().IsGrounded())
            {
                _playerBrain.PlayerAnimator.SetBool("IsAttacking", true);
                _playerBrain.PlayerAnimator.SetTrigger("Attack");
                Debug.Log("Attacking forwards...");
                Collider2D[] objectsToHit = Physics2D.OverlapCircleAll(_forwardAttackTransform.position, _forwardAttackRadius, _attackableLayer);

                if (objectsToHit.Length == 0) return;

                var outgoingDamage = _statManager.GetRandomPhysicalDamage();
                _playerBrain.GetStateManager().IsRecoilingX = true;

                foreach (var obj in objectsToHit)
                {
                    var damagable = obj.GetComponent<IDamageable>();

                    if (damagable != null)
                    {
                        // 2 is just a test number; Replace this later with actual damage stat.
                        damagable.TakeDamage((int) outgoingDamage);
                        DamagePopup.Create(damagable.GetPosition(), (int) outgoingDamage);
                    }
                }
            }

            //Upward Attack
            else if (yAxisInput > 0)
            {
                _playerBrain.PlayerAnimator.SetBool("IsAttacking", true);
                _playerBrain.PlayerAnimator.SetTrigger("Attack");
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
            else if (yAxisInput < 0 && !_playerBrain.GetCollisionDetector().IsGrounded())
            {
                _playerBrain.PlayerAnimator.SetBool("IsAttacking", true);
                _playerBrain.PlayerAnimator.SetTrigger("Attack");
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

    public void TakeDamage(float damage)
    {
        float currentHealth = _statManager.CurrentHealth;
        _statManager.SetCurrentHealth(currentHealth - damage);

        OnDamaged?.Invoke(this, EventArgs.Empty);
    }
    
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private void Recoil()
    {
        if (_playerBrain.GetStateManager().IsRecoilingX)
        {
            var recoilModifier = _playerBrain.GetStateManager().IsLookingRight ? -1 : 1;
            _playerBrain.PlayerRigidBody2D.velocity = new Vector2(_horizontalRecoilSpeed * recoilModifier, 0);
        }

        if (_playerBrain.GetStateManager().IsRecoilingY)
        {
            var recoilModifier = _playerBrain.GetInputManager().VerticalInputModifier < 0 ? 1 : -1;
            _playerBrain.PlayerRigidBody2D.velocity = new Vector2(_playerBrain.PlayerRigidBody2D.velocity.x, _verticalRecoilSpeed * recoilModifier);
            _playerBrain.PlayerRigidBody2D.gravityScale = 0;
        }
        else
        {
            _playerBrain.PlayerRigidBody2D.gravityScale = _gravityScale;
        }
    }

    private void StopHorizontalRecoil()
    {
        _stepsRecoiledHorizontally = 0;
        _playerBrain.GetStateManager().IsRecoilingX = false;
    }

    private void StopVerticalRecoil()
    {
        _stepsRecoiledVertically = 0;
        _playerBrain.GetStateManager().IsRecoilingY = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_forwardAttackTransform.position, _forwardAttackRadius);
        Gizmos.DrawWireSphere(_upwardsAttackTransform.position, _upwardsAttackRadius);
        Gizmos.DrawWireSphere(_downwardsAttackTransform.position, _downwardsAttackRadius);
    }
}