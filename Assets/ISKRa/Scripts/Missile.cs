using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _hitDistance = 0.2f;
    [SerializeField] private float _maxLifetime = 5f;

    private EnemyTarget _target;
    private int _damage;

    public void Initialize(EnemyTarget target, int damage)
    {
        _target = target;
        _damage = damage;
        Destroy(gameObject, _maxLifetime);
    }

    private void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 targetPosition = _target.AimPosition;
        Vector3 direction = targetPosition - transform.position;

        if (direction.sqrMagnitude <= _hitDistance * _hitDistance)
        {
            HitTarget();
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);

        if (direction.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }

    private void HitTarget()
    {
        if (_target.Durability != null)
        {
            _target.Durability.TakeDamage(_damage);
        }
        Destroy(gameObject);
    }
}
