using Assets.Scripts.Damagables;
using Assets.Scripts.GameAssets;
using Assets.Scripts.Player.Combat;
using System;
using System.Collections;
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

    [Header("Gettting Hit")]
    [SerializeField] private Color _damagedColor;
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private bool _isInvulnerable;
    private bool _isFlashing;
    private float _invulnerabilityTime = 0.8f;

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
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = GetComponent<SpriteRenderer>().color;
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

            _playerBrain.PlayerAnimator.SetBool("IsAttacking", true);
            _playerBrain.PlayerAnimator.SetTrigger("Attack");

            // Forward Attack
            if (yAxisInput == 0 || yAxisInput < 0 && _playerBrain.GetCollisionDetector().IsGrounded())
            {
                PerformDirectionalAttack(_forwardAttackTransform.position, _forwardAttackRadius, AttackMotion.Horizontal, _attackableLayer);
            }

            // Upward Attack
            else if (yAxisInput > 0)
            {
                PerformDirectionalAttack(_upwardsAttackTransform.position, _upwardsAttackRadius, AttackMotion.Vertical, _attackableLayer);
            }

            // Downward Attack
            else if (yAxisInput < 0 && !_playerBrain.GetCollisionDetector().IsGrounded())
            {
                PerformDirectionalAttack(_downwardsAttackTransform.position, _downwardsAttackRadius, AttackMotion.Vertical, _attackableLayer);
            }
        }
    }

    private void PerformDirectionalAttack(Vector2 attackDirection, float radius, AttackMotion attackMotion, LayerMask attackableLayer)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapCircleAll(attackDirection, radius, attackableLayer);

        if (objectsToHit.Length == 0) return;

        var hitEffect = Instantiate(GameAssets.i.prefabHitEffect, attackDirection, Quaternion.identity);

        if (attackMotion == AttackMotion.Horizontal)
        {
            _playerBrain.GetStateManager().IsRecoilingX = true;
        }
        else if (attackMotion == AttackMotion.Vertical)
        {
            _playerBrain.GetStateManager().IsRecoilingY = true;
        }

        foreach (var obj in objectsToHit)
        {
            var outgoingDamage = _statManager.GetRandomPhysicalDamage();
            var damagable = obj.GetComponent<IDamageable>();

            if (damagable != null)
            {
                // Add screenshake.
                CinemachineShake.Instance.Shake(1f, 0.1f);

                // Handle damage calculations.
                damagable.TakeDamage((int)outgoingDamage);
                DamagePopup.Create(damagable.GetPosition(), (int)outgoingDamage);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (_isInvulnerable) return;

        // We make the player invulnerable for a certain amount of time, preventing further incoming damage and running the same coroutine twice.
        StartCoroutine(AddInvulnerabilityWindow(_invulnerabilityTime));

        float currentHealth = _statManager.CurrentHealth;
        _statManager.SetCurrentHealth(currentHealth - damage);

        // We make the player recoil.
        _playerBrain.GetStateManager().IsRecoilingX = true;

        // We add combat effects.
        Instantiate(GameAssets.i.prefabPlayerBeingHitEffect, transform.position, Quaternion.identity);
        CinemachineShake.Instance.Shake(2f, 0.5f);
         if (!_isFlashing) StartCoroutine(Flash());

        // We add the hitstop effect after all effects have been instantiated for a nice visual experience.
        HitStop.Instance.Stop(0.2f);

        // This might not even be needed.
        OnDamaged?.Invoke(this, EventArgs.Empty);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private IEnumerator Flash()
    {
        _isFlashing = true;
        _spriteRenderer.color = _damagedColor;
        yield return new WaitForSeconds(0.2f);
        _spriteRenderer.color = _originalColor;
        _isFlashing = false;
    }

    private IEnumerator AddInvulnerabilityWindow(float duration)
    {
        _isInvulnerable = true;
        yield return new WaitForSeconds(duration);
        _isInvulnerable = false;
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