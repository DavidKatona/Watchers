using Assets.Scripts.Cursor;
using Assets.Scripts.Damagables;
using Assets.Scripts.GameAssets;
using Assets.Scripts.Player.Combat;
using Assets.Scripts.SceneManagement;
using Assets.Scripts.Spells;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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

    [Header("Getting Hit")]
    [SerializeField] private Color _damagedColor;
    [SerializeField] UnityEvent OnDamaged;
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private bool _isInvulnerable;
    private bool _isFlashing;
    private float _invulnerabilityTime = 0.8f;

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
        _timeElapsedSinceLastAttack += Time.deltaTime;

        Recoil();
        if (!_playerBrain.GetStateManager().IsWallSliding) Attack();
        if (!_playerBrain.GetStateManager().IsWallSliding) CastAbyssBolt();
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

        var hitEffect = Instantiate(GameAssets.Instance.prefabHitEffect, attackDirection, Quaternion.identity);

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

    private void CastAbyssBolt()
    {
        // ToDo: Refactor this function heavily. Break it up into smaller parts and remove magic numbers.

        if (_playerBrain.GetInputManager().IsAbyssBoltPressed && _timeElapsedSinceLastAttack >= _timeBetweenAttacks && !_playerBrain.GetStateManager().IsDashing)
        {
            var manaCost = 20f;

            if (_statManager.CurrentMana < manaCost)
                return;

            _statManager.SetCurrentMana(_statManager.CurrentMana - manaCost);

            _timeElapsedSinceLastAttack = 0;
            _playerBrain.GetStateManager().IsRecoilingX = true;

            var horizontalDirection = _playerBrain.GetStateManager().IsLookingRight ? 1 : -1;
            var verticalDirection = 0;
            var attackTransform = _forwardAttackTransform;
            var yAxisInput = _playerBrain.GetInputManager().VerticalInputModifier;

            if (yAxisInput > 0)
            {
                verticalDirection = 1;
                attackTransform = _upwardsAttackTransform;
            }
            else if (yAxisInput < 0 && !_playerBrain.GetCollisionDetector().IsGrounded())
            {
                verticalDirection = -1;
                attackTransform = _downwardsAttackTransform;
            }

            CinemachineShake.Instance.Shake(2f, 0.1f);
            Instantiate(GameAssets.Instance.prefabAbyssBoltCastEffect, transform.position, Quaternion.identity);
            var objectToInstantiate = Instantiate(GameAssets.Instance.prefabAbyssBolt, attackTransform.position, Quaternion.identity);
            var projectile = objectToInstantiate.GetComponent<Projectile>();

            if (projectile != null)
            {
                projectile.Setup(horizontalDirection, verticalDirection);
                projectile.OnEnemyDamaged += Projectile_OnEnemyDamaged;
            }
        }
    }

    private void Projectile_OnEnemyDamaged(object sender, OnAreaWideDamageEventArgs e)
    {
        var collidedObjects = e.GetCollidedObjects();

        foreach (var obj in collidedObjects)
        {
            var outgoingDamage = _statManager.MagicalDamage * e.GetDamageModifier();
            var damagable = obj.GetComponent<IDamageable>();

            if (damagable != null)
            {
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
        StartCoroutine(DelayHitStop());

        float currentHealth = _statManager.CurrentHealth;
        float actualDamage = damage / 100 * (100 - _statManager.Armor);
        _statManager.SetCurrentHealth(currentHealth - actualDamage);

        // We make the player recoil.
        _playerBrain.GetStateManager().IsRecoilingX = true;
        _playerBrain.PlayerAnimator.SetTrigger("TakeDamage");

        // We add combat effects.
        Instantiate(GameAssets.Instance.prefabPlayerBeingHitEffect, transform.position, Quaternion.identity);
        CinemachineShake.Instance.Shake(2f, 0.5f);
        if (!_isFlashing) StartCoroutine(Flash());

        OnDamaged?.Invoke();

        if (Mathf.Approximately(_statManager.CurrentHealth, 0))
        {
            Die();
        }
    }

    private void Die()
    {
        Instantiate(GameAssets.Instance.prefabPlayerDeathEffect, transform.position, Quaternion.identity);
        var sceneSwitcherObject = new GameObject("AutoSceneSwitcher");
        var sceneSwitcherComponent = sceneSwitcherObject.AddComponent<AutoSceneSwitcher>();
        sceneSwitcherComponent.Setup(Scene.MainMenu, 3.0f);
        Destroy(gameObject);
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

    private IEnumerator DelayHitStop()
    {
        yield return new WaitForSeconds(0.1f);
        HitStop.Instance.Stop(0.2f);
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