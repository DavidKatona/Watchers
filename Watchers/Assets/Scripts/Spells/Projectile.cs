using Assets.Scripts.GameAssets;
using Assets.Scripts.Spells;
using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public event EventHandler<OnAreaWideDamageEventArgs> OnEnemyDamaged;
    [SerializeField] private float _projectileSpeed = 10f;
    [SerializeField] private float _projectileRadius = 1f;
    [SerializeField] private float _projectileDamageModifier = 1f;
    [SerializeField] private Rigidbody2D _projectileRigidBody;
    [SerializeField] private LayerMask _damagableLayer;

    public void Setup(float yAxisRotation, float zAxisRotation)
    {
        var yRotation = yAxisRotation > 0 ? 0 : 180;
        var zRotation = 0;
        if (zAxisRotation > 0)
        {
            zRotation = 90;
        }
        else if (zAxisRotation < 0)
        {
            zRotation = -90;
        }

        transform.Rotate(0, yRotation, zRotation);
        _projectileRigidBody.velocity = transform.right * _projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Instantiate(GameAssets.i.prefabAbyssBoltExplosionEffect, transform.position, Quaternion.identity);
        CinemachineShake.Instance.Shake(2f, 0.2f);
        GenerateAreaWideDamage(_projectileRadius, _damagableLayer);
    }

    private void GenerateAreaWideDamage(float radius, LayerMask damagableLayer)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapCircleAll(transform.position, radius, damagableLayer);

        if (objectsToHit.Length != 0)
        {
            OnEnemyDamaged?.Invoke(this, new OnAreaWideDamageEventArgs(objectsToHit, _projectileDamageModifier));
        }
    }
}
