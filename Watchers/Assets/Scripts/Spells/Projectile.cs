using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _projectileSpeed = 10f;
    [SerializeField] private Rigidbody2D _projectileRigidBody;

    public void Setup(float projectileDirection)
    {
        var rotation = projectileDirection > 0 ? 0 : 180;
        transform.Rotate(0, rotation, 0);
        _projectileRigidBody.velocity = transform.right * _projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        Destroy(gameObject);
    }
}
