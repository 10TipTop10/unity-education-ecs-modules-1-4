using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected Transform _firePoint;
    [SerializeField] protected Transform _aimOrigin;
    [SerializeField] protected float _range = 6f;
    [SerializeField] protected float _maxAimAngle = 5f;
    [SerializeField] protected bool _verticalAim;

    public abstract void Tick(EnemyTarget target);

    protected bool CanAttack(EnemyTarget target)
    {
        if (target == null)
            return false;

        if (target.Durability == null)
            return false;

        if (target.Durability.CurrentDurability <= 0)
            return false;

        return IsTargetInRange(target) && IsAimedAtTarget(target);
    }

    private bool IsTargetInRange(EnemyTarget target)
    {
        float distanceSqr =
            (target.AimPosition - _firePoint.position).sqrMagnitude;

        return distanceSqr <= _range * _range;
    }

    private bool IsAimedAtTarget(EnemyTarget target)
    {
        Transform aimOrigin = _aimOrigin != null ? _aimOrigin : _firePoint;

        Vector3 targetDirection = target.AimPosition - aimOrigin.position;

        Vector3 weaponDirection = aimOrigin.forward;

        if (!_verticalAim)
        {
            targetDirection.y = 0f;
            weaponDirection.y = 0f;
        }

        if (targetDirection.sqrMagnitude < 0.001f) return true;

        float angle = Vector3.Angle(weaponDirection, targetDirection);

        return angle <= _maxAimAngle;
    }
}
