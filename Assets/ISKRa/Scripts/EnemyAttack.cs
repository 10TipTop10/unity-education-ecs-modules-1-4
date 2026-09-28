using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private EnemyMovement _enemyMovement;
    [SerializeField] private LineRenderer _attackBeam;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Transform _turret;
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _turretRotationSpeed = 180f;
    [SerializeField] private float _maxAimAngle = 5f;
    [SerializeField] private float _attackInterval = 1f;
    [SerializeField] private float _attackRange = 3f;
    [SerializeField] private float _targetHeightOffset = 0.5f;

    private float _nextAttackTime;
    private Durability _baseTarget;
    private Durability _playerTarget;

    private void Awake()
    {
        if (_attackBeam != null)
        {
            _attackBeam.enabled = false;
        }
    }

    private void Update()
    {
        Durability target = SelectTarget();

        if (target == null)
        {
            _enemyMovement.SetCombatState(false);
            UpdateAttackVisual(null, false);
            return;
        }

        float distance =
            Vector3.Distance(transform.position, target.transform.position);

        bool targetInRange = distance <= _attackRange;

        Vector3 targetPosition = target.transform.position + Vector3.up * _targetHeightOffset;

        RotateTurret(targetPosition);

        bool isAimedAtTarget = IsAimedAtTarget(targetPosition);

        bool canAttack = targetInRange && isAimedAtTarget;

        _enemyMovement.SetCombatState(targetInRange);
        UpdateAttackVisual(target, canAttack);

        if (!canAttack) return;
        if (Time.time < _nextAttackTime) return;

        target.TakeDamage(_damage);
        _nextAttackTime = Time.time + _attackInterval;
    }

    public void SetTargets(
        Durability baseTarget,
        Durability playerTarget)
    {
        _baseTarget = baseTarget;
        _playerTarget = playerTarget;
    }

    private void UpdateAttackVisual(
        Durability target,
        bool isAttacking)
    {
        if (_attackBeam == null || _firePoint == null)
            return;

        if (!isAttacking || target == null)
        {
            _attackBeam.enabled = false;
            return;
        }

        Vector3 targetPosition =
            target.transform.position +
            Vector3.up * _targetHeightOffset;

        _attackBeam.SetPosition(0, _firePoint.position);
        _attackBeam.SetPosition(1, targetPosition);
        _attackBeam.enabled = true;
    }

    private void RotateTurret(Vector3 targetPosition)
    {
        if (_turret == null) return;

        Vector3 direction = targetPosition - _turret.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        _turret.rotation = Quaternion.RotateTowards(_turret.rotation, targetRotation, _turretRotationSpeed * Time.deltaTime);
    }

    private bool IsAimedAtTarget(Vector3 targetPosition)
    {
        if (_firePoint == null) return false;

        Vector3 targetDirection = targetPosition - _firePoint.position;

        Vector3 weaponDirection = _firePoint.forward;

        targetDirection.y = 0f;
        weaponDirection.y = 0f;

        if (targetDirection.sqrMagnitude < 0.001f) return true;

        float angle = Vector3.Angle(weaponDirection, targetDirection);

        return angle <= _maxAimAngle;
    }

    private Durability SelectTarget()
    {
        if (_playerTarget != null &&
            _playerTarget.CurrentDurability > 0)
        {
            float playerDistance = Vector3.Distance(
                transform.position,
                _playerTarget.transform.position);

            if (playerDistance <= _attackRange)
                return _playerTarget;
        }

        if (_baseTarget != null &&
            _baseTarget.CurrentDurability > 0)
        {
            float baseDistance = Vector3.Distance(transform.position, _baseTarget.transform.position);

            if (baseDistance <= _attackRange)
                return _baseTarget;
        }

        return null;
    }
}
